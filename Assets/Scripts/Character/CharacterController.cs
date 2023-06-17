using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private CharStats stats;
    private TechHandler _techHandler;
    public TileSelector tileSelector;
    public CharacterCombat combat;
    public Transform tform;
    [FormerlySerializedAs("Collider")] public Collider ccollider;
    public float minY = 0;
    private CharacterMovement _movement;
    public CharacterGUI gui;
    
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
    public bool Ready { get; set; }
    public int Actions { get; set; }
    public bool CanMove { get; set; }
    public bool CanOtherAction { get; set; }
    public GameObject Target { get; set; }
    public CharStats GetStats()
    {
        return stats;
    }
    public TechHandler GetTechHandler()
    {
        return _techHandler;
    }
    private void Awake()
    {
        stats = Instantiate(stats);
        gui = GetComponentInChildren<CharacterGUI>();
        gui.charStats = stats;
        TurnManager.AddUnit(this);
    }

    protected virtual void Start()
     {
         Actions = 2;
         CanMove = true;
         CanOtherAction = true;
         
         
        
        combat = GetComponent<CharacterCombat>();
        tileSelector = GetComponent<TileSelector>();
        _techHandler = GetComponent<TechHandler>();
        _movement = GetComponent<CharacterMovement>();
        ccollider = GetComponent<Collider>();
        tform = GetComponent<Transform>();
        minY = ccollider.bounds.min.y;
    }

    private void Update()
    {
        if (Ready) BeginPhase();
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
        return _movement;
    }
    
    // Attack
    public void BasicAttack(CharacterController attacker, CharacterController defender)
    {
        combat.BasicAttack(attacker, defender);
    }

    public void TechTarget(Technique tech)
    {
        OnTechTarget?.Invoke();
        combat.TechTarget(tech);
    }

    public void TechAttack(List<CharacterController> targets)
    {
        List<CharacterController> enemyTargets =
            new List<CharacterController>(targets).FindAll(obj => !obj.CompareTag(tag));
        combat.TechAttack(_techHandler.SelectedTech, this, enemyTargets);
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
