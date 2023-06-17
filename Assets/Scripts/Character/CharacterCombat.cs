using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterCombat : MonoBehaviour
{
    private CharStats _stats;
    private CharacterController _character;
    private TechHandler _techHandler;
    private TileSelector _tileSelector;
    
    private void Start()
    {
        _tileSelector = GetComponent<TileSelector>();
        _character = GetComponent<CharacterController>();
        _techHandler = GetComponent<TechHandler>();
        _stats = _character.GetStats();
    }

    // Find targets
    
    // Targeting

    public void TechTarget(Technique tech)
    {
        int range = tech.range;
        RangeStyle rangeStyle = tech.rangeStyle;
        int height = 100;
        
        _tileSelector.FindSelectableTiles(height, range, null, Tile.State.Tech, rangeStyle);
        if (tech.LOS)
        {
            Vector3 losPosition = _character.LosPos;
            _tileSelector.RemoveNoLOSTiles(losPosition);
        }
    }
    
    // Battle
    public void TechAttack(Technique tech, CharacterController attacker, List<CharacterController> targets)
    {
        //Animate()
        foreach (CharacterController target in targets)
        {
            BattleManager.TechAttack(tech, attacker, target);
        }
    }
    
    public void TakeDamage(float dmg)
    {
        int animationLength = 1;
        _stats.AddStat(Stat.Health, -dmg);
        StartCoroutine(TakeDamageAnimate(dmg, animationLength));
    }

    IEnumerator TakeDamageAnimate(float dmg, int animationLength)
    {
        _character.gui.TakeDamageAnimate(-dmg, animationLength);
        yield return new WaitForSeconds(animationLength);
        
        
        if (_stats.GetStat(Stat.Health) <= 0) _character.Die();
    }
    public void BasicAttack(CharacterController attacker, CharacterController defender)
    {
        //BattleManager.BasicAttack(attacker, defender);
    }
}
