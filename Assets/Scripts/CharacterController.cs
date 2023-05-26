using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private CharStats stats;
    private bool _turn;
    public Renderer Renderer;
    public float minY = 0;
    private CharacterMovement _movement;
    

    
    public static event UnityAction OnLeftOverActions;
    public static event UnityAction OnEndMove;
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
        CanOtherAction = true;
        stats = Instantiate(stats);
        
        TurnManager.AddUnit(this);
        _movement = GetComponent<CharacterMovement>();
        Renderer = GetComponent<Renderer>();
        minY = Renderer.bounds.min.y;
    }

    public virtual void FindNearestTarget()
    {
        print("Empty");
    }
    
    // Turns
    public virtual void BeginTurn()
    {
        
    }
    public virtual void EndTurn()
    {
        _movement.ResetSelectableTilesList();
        Actions = 2;
        CanMove = true;
        _turn = false;
        TurnManager.EndTurn();
    }
    public virtual void RemoveAction(int num)
    {
        Actions -= num;
        if (Actions <= 0)
        {
            print("Ending Turn_________________________");
            EndTurn();
        }
        else if (CompareTag("Player"))
        {
            OnLeftOverActions?.Invoke();
        }
    }

    
    // Move
    public void MoveToTile(Tile tile)
    {
        print("Move to Tile: " + tile.transform.position);
        GetMovement().MoveTo(tile);

    }
    public virtual void EndMove()
    {
        OnEndMove?.Invoke();
        CanMove = false;
        RemoveAction(1);
    }
    public CharacterMovement GetMovement()
    {
        return _movement;
    }
    
    
    // Battle
    public void TakeDamage(float dmg)
    {
        stats.SetStat(Stat.Health, dmg);
    }
    public void BasicAttack(CharacterController attacker, CharacterController defender)
    {
        BattleManager.BasicAttack(attacker, defender);
    }

}
