using UnityEngine;
public enum Tech
{
    None,
    BasicWater1,
    BasicPlant1
}


[CreateAssetMenu(menuName = "Custom/Technique")]
public class Technique : ScriptableObject
{
    public enum Target
    {
        Enemy,
        Ally
    }
    public enum Status
    {
        None,
        Poison,
        HealOverTime
    }

    
    [SerializeField] public Target target;
    [SerializeField] public string techName;
    [SerializeField] public CharType type;
    [SerializeField] public float power;
    [SerializeField] public int heal;
    [SerializeField] public int healOverTime;
    [SerializeField] public int healOverTimeTurns;
    [SerializeField] public Status status;
    [SerializeField] public int pp;
    [SerializeField] public int cooldownMax;
    [HideInInspector] public int cooldownCur = 0;
    [SerializeField] public int acc;
    [SerializeField] public int range;
    [SerializeField] public float AOE;
    [SerializeField] public bool LOS;
    
}
