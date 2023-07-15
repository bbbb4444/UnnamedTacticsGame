using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterCombat : MonoBehaviour
{
    private CharStats _stats;
    private CharacterController _character;
    private TechHandler _techHandler;
    private TileSelector _tileSelector;
    private Animator _animator;
    
    public void Initialize()
    {        
        _tileSelector = GetComponent<TileSelector>();
        _character = GetComponent<CharacterController>();
        _techHandler = GetComponent<TechHandler>();
        _animator = GetComponentInChildren<Animator>();
        _stats = _character.Stats;
    }
    
    // Find targets
    
    // Targeting

    public void TechTarget(Technique tech)
    {
        int range = tech.range;
        //RangeStyle rangeStyle = tech.rangeStyle;
        
        _tileSelector.FindSelectableTechTiles(range, null);
        if (tech.LOS)
        {
            Vector3 losPosition = _character.LosPos;
            _tileSelector.RemoveNoLOSTiles(losPosition);
        }
    }
    public void TechTarget(Technique tech, Tile centerTile)
    {
        int range = tech.range;
        //RangeStyle rangeStyle = tech.rangeStyle;
        int height = 100;
        
        _tileSelector.FindSelectableTechTiles(range, centerTile);
        if (tech.LOS)
        {
            Vector3 losPosition = _character.LosPos;
            _tileSelector.RemoveNoLOSTiles(losPosition);
        }
    }
    // Battle
    
    public void TakeHeal(float heal, string techAnimation)
    {
        int animationLength = 1;
        _stats.AddStat(Stat.Health, heal);
        StartCoroutine(TakeDamageAnimate(-heal, animationLength, techAnimation));
    }
    
    public void TakeDamage(float dmg, string techAnimation)
    {
        int animationLength = 1;
        _stats.AddStat(Stat.Health, -dmg);
        StartCoroutine(TakeDamageAnimate(dmg, animationLength, techAnimation));
    }

    IEnumerator TakeDamageAnimate(float dmg, int animationLength, string techAnimation)
    {
        _animator.Play(techAnimation);
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName(techAnimation)); // Wait until tech animation starts
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f); // Wait until tech animation ends
        
        _character.gui.AnimateHPBar(-dmg, animationLength);
        yield return new WaitForSeconds(animationLength);
        
        TurnManager.GetActivePlayer().EndOtherActionPhase();
        if ((int) _stats.GetStat(Stat.Health) <= 0) _character.Die();
    }
}
