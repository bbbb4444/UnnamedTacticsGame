using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private CharStats stats;
    public CharacterCombat combat;
    public Renderer Renderer;
    public float minY = 0;
    private CharacterMovement _movement;
    public CharacterGUI gui;
    
    public static event UnityAction OnTurnStart;
    public static event UnityAction OnPhaseStart;
    public static event UnityAction OnLeftOverActions;
    public static event UnityAction OnEndMovePhase;
    public static event UnityAction OnEndOtherActionPhase;
    public static event UnityAction OnTurnEnd;
    
    public bool Ready { get; set; }
    public int Actions { get; set; }
    public bool CanMove { get; set; }
    public bool CanOtherAction { get; set; }
    public GameObject Target { get; set; }
    public CharStats GetStats()
    {
        return stats;
    }

    private void Awake()
    {
        stats = Instantiate(stats);
        gui = GetComponentInChildren<CharacterGUI>();
        gui.charStats = stats;
    }

    protected virtual void Start()
     {
         Ready = false;
         Actions = 2;
         CanMove = true;
         CanOtherAction = true;
         
         
        TurnManager.AddUnit(this);
        combat = GetComponent<CharacterCombat>();
        _movement = GetComponent<CharacterMovement>();
        Renderer = GetComponent<Renderer>();
        minY = Renderer.bounds.min.y;
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
    public void StartTurn()
    {
        BeginPhase();
    }

    public virtual void BeginPhase()
    {
        Ready = false;
        if (Actions > 0)
        {
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
        BeginPhase();
    }

    public void EndOtherActionPhase()
    {
        Ready = true;
        CanOtherAction = false;
        RemoveAction(1);
        OnEndOtherActionPhase?.Invoke();
        BeginPhase();
    }
    public CharacterMovement GetMovement()
    {
        return _movement;
    }
    
    public void BasicAttack(CharacterController attacker, CharacterController defender)
    {
        combat.BasicAttack(attacker, defender);
        EndOtherActionPhase();
    }
    


}
