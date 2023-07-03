using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Custom/Effectiveness")]
public class Effectiveness : ScriptableObject
{
    [SerializeField] public BattleManager.Effective effectiveness;
    [SerializeField] public float multiplier;
    [SerializeField] public Color color;
}