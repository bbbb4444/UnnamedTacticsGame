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
    public static event UnityAction OnTurnStart;
    public static event UnityAction OnPhaseStart;
    public static event UnityAction OnLeftOverActions;
    public static event UnityAction OnEndMovePhase;
    public static event UnityAction OnEndOtherActionPhase;
    public static event UnityAction OnTurnEnd;
    public int Actions { get; set; }
    public bool CanMove { get; set; }
    public bool CanOtherAction { get; set; }
    public GameObject Target { get; set; }
    public CharStats GetStats()
    {
        return stats;
    }
    
     protected virtual void Start()
     {
         Actions = 2;
         CanMove = true;
         
        stats = Instantiate(stats);
        OnEndMovePhase += BeginPhase;
        OnEndOtherActionPhase += BeginPhase;
        
        TurnManager.AddUnit(this);
        combat = GetComponent<CharacterCombat>();
        _movement = GetComponent<CharacterMovement>();
        Renderer = GetComponent<Renderer>();
        minY = Renderer.bounds.min.y;
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
        if (Actions > 0)
        {
            OnPhaseStart?.Invoke();
        }
        else EndTurn();
        
    }
    public virtual void EndTurn()
    {
        Actions = 2;
        CanMove = true;
        CanOtherAction = true;
        TurnManager.EndTurn();
    }
    public virtual void RemoveAction(int num)
    {
 
        Actions -= num;
  
    }

    
    // Move
    public void MoveToTile(Tile tile)
    {
        print("Move to Tile: " + tile.transform.position);
        GetMovement().MoveTo(tile);

    }
    public virtual void EndMovePhase()
    {
        CanMove = false;
        RemoveAction(1);
        OnEndMovePhase?.Invoke();
    }

    public virtual void EndOtherActionPhase()
    {
        CanOtherAction = false;
        OnEndOtherActionPhase?.Invoke();
        RemoveAction(1);
    }
    public CharacterMovement GetMovement()
    {
        return _movement;
    }
    
    public void BasicAttack(CharacterController attacker, CharacterController defender)
    {
        combat.BasicAttack(attacker, defender);
    }
    


}
