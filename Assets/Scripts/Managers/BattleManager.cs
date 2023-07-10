using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleManager : MonoBehaviour
{
    public enum Effective
    {
        Ultra = 2,
        Super = 1,
        Neutral = 0,
        Weakly = -1,
        SuperWeakly = -2,
        Immune = -3
    }

    public static void PerformTech(Technique tech, CharacterController attacker, List<CharacterController> targets)
    {
        foreach (CharacterController target in targets)
        {
            if (tech.target == Technique.Target.Ally)
            {
                if (!AccuracyCheck(tech))
                {
                    target.combat.TakeHeal(0);
                    return;
                }
                
                float heal = CalculateHeal(tech, attacker, target);
                target.combat.TakeHeal(heal);
            }
            
            else if (tech.target == Technique.Target.Enemy)
            {
                if (!AccuracyCheck(tech))
                {
                    target.combat.TakeDamage(0);
                    return;
                }
                
                float damage = CalculateDamage(tech, attacker, target);
                target.combat.TakeDamage(damage);
            }
        }
    }

    private static bool AccuracyCheck(Technique tech)
    {
        float accRoll = Random.value * 100f;
        return accRoll <= tech.acc;
    }

    public static float CalculateHeal(Technique tech, CharacterController attacker, CharacterController defender)
    {
        float techHeal = tech.heal;
        return techHeal;
    }
    
    public static float CalculateDamage(Technique tech, CharacterController attacker, CharacterController defender)
    {
        float techPower = tech.power/100f;
        float attackerStr = attacker.Stats.GetStat(Stat.Strength);
        float defenderDef = defender.Stats.GetStat(Stat.Defense);
        float damage;

        if (attackerStr > defenderDef)
        {
            damage = (attackerStr * 2 - defenderDef) * techPower;
        }
        else
        {
            damage = (attackerStr * attackerStr / defenderDef) * techPower;
        }

        if (tech.type == attacker.CharType) damage *= 1.1f;
        
        damage *= GetTypeEffectiveness(tech, defender);
        
        return damage;
    }

    
    private static float GetTypeEffectiveness(Technique tech, CharacterController defender)
    {
        Type defenderType = defender.CharType.type;
        Effectiveness effectiveness = tech.type.effectiveness[(int) defenderType];
        return effectiveness.multiplier;
    }
    
    public static float GetTypeEffectiveness(Technique tech, CharType defendType)
    {
        Type defenderType = defendType.type;
        Effectiveness effectiveness = tech.type.effectiveness[(int) defenderType];
        return effectiveness.multiplier;
    }
    public static Color GetEffectivenessColor(Technique tech, CharType defendType)
    {
        Type defenderType = defendType.type;
        Effectiveness effectiveness = tech.type.effectiveness[(int) defenderType];
        return effectiveness.color;
    }
}
