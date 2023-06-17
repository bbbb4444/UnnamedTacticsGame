using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AIHandler : MonoBehaviour
{
    enum AIStyle
    {
        HeavyHits,
        Ranged
    }
    
    [SerializeField]
    private AIStyle aiStyle;
    private NPCMovement _movement;
    private CharacterController _controller;
    private TechHandler _techHandler;
    private CharacterCombat _combat;

    private bool _outOfRange;
    
    public static UnityAction PrepareTech;
    public static UnityAction PrepareMove;

    public static UnityAction SelectTech;
    
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _techHandler = GetComponent<TechHandler>();
        _movement = GetComponent<NPCMovement>();
        _combat = GetComponent<CharacterCombat>();
    }


    public IEnumerator BeginPhase()
    {
        Technique tech = FindTech(aiStyle);
        _techHandler.SelectedTech = tech;
        
        _controller.FindNearestTarget();
        GameObject target = _controller.Target;
        Transform targetTransform = target.transform;
        
        if (_controller.CanOtherAction && !_outOfRange)
        {
            if (tech.LOS)
            {
                TileSelector targetTileSelector = target.GetComponent<TileSelector>();
                
                if (_controller.tileSelector.HaveLOS(_controller.LosPos, targetTileSelector.GetTargetTile(target).posLOS))
                {
                    print("Have los to target");
                    if (Vector3.Distance(transform.position, targetTransform.position) <= tech.range)
                    {
                        PrepareTech?.Invoke();
                        yield return new WaitForSeconds(1);
                        SelectTech?.Invoke();
                        yield return new WaitUntil(() => _controller.Ready);
                    }
                }
            }
            
            else if (Vector3.Distance(transform.position, targetTransform.position) <= tech.range)
            {
                print("Distance: " + Vector3.Distance(transform.position, targetTransform.position));
                print("Tech range: " + tech.range);
                PrepareTech?.Invoke();
                yield return new WaitForSeconds(1);
                SelectTech?.Invoke();
                yield return new WaitUntil(() => _controller.Ready);
            }
            else
            {
                _outOfRange = true;
                _controller.Ready = true;
            }
        }
        else if (_controller.CanMove)
        {
            _outOfRange = false;
            
            PrepareMove?.Invoke();
            yield return new WaitForSeconds(1);
            Move();
            yield return new WaitUntil(() => _controller.Ready);
        }
        else
        {
            _controller.RemoveAction(1);
            _controller.Ready = true;
        }
    }

    private Technique FindTech(AIStyle style)
    {
        return style switch
        {
            AIStyle.HeavyHits => FindStrongestTech(),
            AIStyle.Ranged => FindStrongestTech(),
            _ => FindStrongestTech()
        };
    }
    private Technique FindStrongestTech()
    {
        Technique strongestTech = _techHandler.Techinques[0];
        
        foreach (Technique tech in _techHandler.Techinques)
        {
            if (tech.power >= strongestTech.power)
            {
                strongestTech = tech;
            }
        }
        
        return strongestTech;
    }
    
    void Attack()
    {
        print("ATTACK");
        _controller.BasicAttack(_controller, _controller.Target.GetComponent<CharacterController>());
        print("Waiting to be finished attacking...");
    }

    void Move()
    {
        print("MOVE");
        _movement.MoveToTarget(_controller.Target);
    }
}
