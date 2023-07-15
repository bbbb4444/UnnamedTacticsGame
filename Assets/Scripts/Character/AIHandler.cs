using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private TileSelector _tileSelector;
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
        _tileSelector = GetComponent<TileSelector>();
        _combat = GetComponent<CharacterCombat>();
    }

    /*public IEnumerator BeginPhase()
    {
        _controller.FindNearestTarget();
        GameObject target = _controller.Target;
        CharacterController targetController = TurnManager.PlayerObjectToController[target];
        CharType targetType = targetController.CharType;
        Transform targetTransform = target.transform;

        List<Technique> techniquePriority = CreateTechniquePriority(targetType);
        
        
        _combat.TechTarget(techniquePriority[0]);
        List<CharacterController> inRangeCharacters = _tileSelector.GetTargetedCharacters();
        if (inRangeCharacters.Contains(targetController));//attack

        else // If you moved, can you be in range then?
        {
            _tileSelector.ResetSelectableTiles();
            _movement.FindSelectableTiles();
            _movement.CalculatePath();
            Tile newCenter = _movement.actualTargetTile;
            _combat.TechTarget(techniquePriority[0], newCenter);
            if (inRangeCharacters.Contains(targetController))
            {
                PrepareMove?.Invoke();
                yield return new WaitForSeconds(1);
                Move();
                yield return new WaitUntil(() => _controller.Ready);
            }
        }
            

        yield return new WaitForSeconds(1);
    }
*/
    private List<Technique> CreateTechniquePriority(CharType targetType)
    {
        List<Technique> availableTechs = _techHandler.Techinques;
        availableTechs.Sort((obj1, obj2) =>
        {
            float obj1Effectiveness = BattleManager.GetTypeEffectiveness(obj1, targetType);
            float obj2Effectiveness = BattleManager.GetTypeEffectiveness(obj2, targetType);

            float modifiedPower1 = obj1.power * obj1Effectiveness;
            float modifiedPower2 = obj2.power * obj2Effectiveness;

            return modifiedPower2.CompareTo(modifiedPower1);
        });
        
        return availableTechs;
    }
    public IEnumerator BeginPhase()
    {
        _controller.FindNearestTarget();
        GameObject target = _controller.Target;
        CharacterController targetController = TurnManager.PlayerObjectToController[target];
        CharType targetType = targetController.CharType;
        List<Technique> techsOrdered = CreateTechniquePriority(targetType);
        //Technique tech = FindTech(aiStyle, targetType);
        Technique tech = techsOrdered[0];
        _techHandler.SelectedTech = tech;
        
        Transform targetTransform = target.transform;
        
        _combat.TechTarget(tech);
        
        List<CharacterController> inRangeCharacters = _tileSelector.GetTargetedCharacters();
        _tileSelector.ResetSelectableTiles();

        if (_controller.CanOtherAction && !_controller.CanMove)
        {
            
        }
        
        if (_controller.CanOtherAction && inRangeCharacters.Contains(targetController))
        {
            PrepareTech?.Invoke();
            yield return new WaitForSeconds(1);
            SelectTech?.Invoke();
            yield return new WaitUntil(() => _controller.Ready);
        }
        else if (!_controller.CanMove)
        {
            foreach (Technique technique in techsOrdered)
            {
                _combat.TechTarget(technique);
                inRangeCharacters = _tileSelector.GetTargetedCharacters();
                _tileSelector.ResetSelectableTiles();
                if (inRangeCharacters.Contains(targetController))
                {
                    _techHandler.SelectedTech = technique;
                    PrepareTech?.Invoke();
                    yield return new WaitForSeconds(1);
                    SelectTech?.Invoke();
                    yield return new WaitUntil(() => _controller.Ready);
                }
                else
                {
                    _controller.RemoveAction(1);
                    _controller.Ready = true;
                }
            }
        }
        /*
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
        */
        else if (_controller.CanMove)
        {
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

    private Technique FindTech(AIStyle style, CharType targetType)
    {
        return style switch
        {
            AIStyle.HeavyHits => FindStrongestTech(targetType),
            AIStyle.Ranged => FindStrongestTech(targetType),
            _ => FindStrongestTech(targetType)
        };
    }
    private Technique FindStrongestTech(CharType targetType)
    {
        List<Technique> techsOrdered = CreateTechniquePriority(targetType);
        return techsOrdered[0];
    }

    void Move()
    {
        print("MOVE");
        _movement.MoveToTarget(_controller.Target);
    }
}
