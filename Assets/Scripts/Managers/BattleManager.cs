using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleManager : MonoBehaviour
{
    public static void TechAttack(Technique tech, CharacterController attacker, CharacterController defender)
    {
        float accRoll = Random.value*100f;
        if (accRoll > tech.acc)
        {
            defender.combat.TakeDamage(0);
            return;
        }

        float techPower = tech.power/100f;
        float attackerStr = attacker.GetStats().GetStat(Stat.Strength);
        float defenderDef = defender.GetStats().GetStat(Stat.Defense);
        float damage;

        if (attackerStr > defenderDef)
        {
            damage = (attackerStr * 2 - defenderDef) * techPower;
        }
        else
        {
            damage = (attackerStr * attackerStr / defenderDef) * techPower;
        }

        damage *= GetTypeEffectiveness(tech, defender);
        
        defender.combat.TakeDamage(damage);
    }

    private static float GetTypeEffectiveness(Technique tech, CharacterController defender)
    {
        Type defenderType = defender.GetStats().type.type;
        float effectiveness = tech.type.effectiveness[(int) defenderType];
        
        switch (effectiveness)
        {
            case 2:
                effectiveness = 2.25f;
                break;
            case 1:
                effectiveness = 1.5f;
                break;
            case 0:
                effectiveness = 1f;
                break;
            case -1:
                effectiveness = 0.75f;
                break;
            case -2:
                effectiveness = 0.5625f;
                break;
            case -3:
                effectiveness = 0;
                break;
            default:
                print("type effectiveness not supported");
                throw new Exception();
        }

        return effectiveness;
    }
}
