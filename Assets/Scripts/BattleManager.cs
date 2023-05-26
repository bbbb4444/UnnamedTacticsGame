using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static void BasicAttack(CharacterController attacker, CharacterController defender)
    {
        float attackerStr = attacker.GetStats().GetStat(Stat.Strength);
        float defenderDef = defender.GetStats().GetStat(Stat.Defense);

        float percentDiff = Mathf.Abs(attackerStr - defenderDef) / (attackerStr + defenderDef) / 2;
        print("Percent diff: " + percentDiff);
    }
}
