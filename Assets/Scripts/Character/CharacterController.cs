using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private CharStats stats;
    [SerializeField] private CharType type;
    public SoundPlayer soundPlayer;
    private TechHandler _techHandler;
    [HideInInspector] public TileSelector tileSelector;
    [HideInInspector] public CharColor charColor;
    [HideInInspector] public CharacterCombat combat;
    [HideInInspector] public Transform tform;
    [HideInInspector] public Collider ccollider;
    [HideInInspector] public float minY = 0;
    protected CharacterMovement Movement;
    [HideInInspector] public CharacterGUI gui;
    
    public static event UnityAction OnPhaseStart;
    public static event UnityAction OnEndMovePhase;
    public static event UnityAction OnEndOtherActionPhase;
    public static event UnityAction OnTechTarget;

    public Vector3 LosPos // The vector3 used as the origin of line-of-sight checking
    {
        get
        {
            Vector3 losPos = new Vector3(tform.position.x, ccollider.bounds.max.y, tform.position.z);
            return losPos;
        }
    }
    private Vector3 StartingPosition { get; set; }
    public string Name { get; set; }
    public bool Ready { get; set; }
    public int Actions { get; set; }
    public bool CanMove { get; set; }
    public bool CanOtherAction { get; set; }
    public GameObject Target { get; set; }

    public CharStats Stats
    {
        get
        {
            return stats;
        }
        set
        {
            stats = Instantiate(value);
        }
    }

    public CharType CharType
    {
        get => type;
        set
        {
            type = value;
            charColor.UpdateColor(value);
        }
    }

    public TechHandler TechHandler { get; set; }

    private void OnEnable()
    {
        TurnManager.OnBattleEnter += AddToTurnManager;
    }
    private void OnDisable()
    {
        TurnManager.OnBattleEnter -= AddToTurnManager;
    }
    protected virtual void Awake()
    {
        Name = RandomName.GetRandomName();
        
        gui = GetComponentInChildren<CharacterGUI>();
        
        ccollider = GetComponent<Collider>();
        tform = GetComponent<Transform>();
        //soundPlayer = GetComponent<SoundPlayer>();
        
        TryGetThenAddComponents();

        if (stats) Initialize();
    }

    public void Initialize()
    {
        Stats = Stats;
        if (!CharType) CharType = stats.type;
        combat.Initialize();
        charColor.Initialize();
        Movement.Initialize();
        TechHandler.Initialize();
        gui.charStats = stats;
        gui.Initialize();
        
    }
    
    private void TryGetThenAddComponents()
    {
        combat = GetComponent<CharacterCombat>();
        tileSelector = GetComponent<TileSelector>();
        charColor = GetComponent<CharColor>();
        TechHandler = GetComponent<TechHandler>();
        //soundPlayer.techHandler = TechHandler;

        if (CompareTag("Player"))
            Movement = GetComponent<CharacterMovement>() ?? gameObject.AddComponent<CharacterMovement>();
    }
    
    protected virtual void Start()
    {
        Actions = 2;
        CanMove = true;
        CanOtherAction = true;
        
        minY = ccollider.bounds.min.y;
    }

    public void BeginTurn()
    {
        TechHandler.ReduceCooldowns();
        StartingPosition = tform.position;
        Ready = true;
    }
    
    private void Update()
    {
        if (Ready && !TurnManager.IsGameOver()) BeginPhase();
    }

    private void AddToTurnManager()
    {
        TurnManager.AddUnit(this);
    }
    
    public virtual void FindNearestTarget()
    {
        print("Empty");
    }
    
    // Turns
    protected void InvokeOnPhaseStart()
    {
        OnPhaseStart?.Invoke();
    }
   public virtual void BeginPhase()
    {
        Ready = false;
        if (Actions > 0)
        {
            UIManager.Instance.OpenScreen(ScreenType.ActionMenu);
            OnPhaseStart?.Invoke();
        }
        else EndTurn();
        
    }

   public void ResetBattle()
   {
       print(Stats.GetStat(Stat.Health));
       Stats.SetStat(Stat.Health, Stats.GetBaseStat(Stat.Health));
       gui.ResetHP();
   }
   
   public void ResetTurn()
   {
       Actions = 2;
       CanMove = true;
       CanOtherAction = true;
   }
   
    public void ResetMovement()
    {
        Actions = 2;
        CanMove = true;
        tform.position = StartingPosition;
    }
    public void EndTurn()
    {
        Actions = 2;
        CanMove = true;
        CanOtherAction = true;
        TurnManager.EndTurn();
    }
    public void RemoveAction(int num)
    {
        Actions -= num;
        print(this.tag + " : " + Actions);
    }


    // Move
    public void MoveToTile(Tile tile)
    {
        print("Move to Tile: " + tile.transform.position);
        GetMovement().MoveTo(tile);

    }
    public void EndMovePhase()
    {
        Ready = true;
        CanMove = false;
        RemoveAction(1);
        OnEndMovePhase?.Invoke();
    }
    public CharacterMovement GetMovement()
    {
        return Movement;
    }

    // Attack
    public void TechTarget(Technique tech)
    {
        OnTechTarget?.Invoke();
        combat.TechTarget(tech);
    }

    public void Die()
    {
        Ready = false;
        TurnManager.RemoveUnit(this);
        print("dead");
        

        gameObject.SetActive(false);
    }

    public void EndOtherActionPhase()
    {
        Ready = true;
        CanOtherAction = false;
        RemoveAction(1);
        OnEndOtherActionPhase?.Invoke();
    }



}
