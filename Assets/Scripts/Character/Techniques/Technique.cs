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
    [SerializeField] public Tech tech;
    [SerializeField] public string techName;
    [SerializeField] public CharType type;
    [SerializeField] public float power;
    [SerializeField] public int pp;
    [SerializeField] public int cooldown;
    [SerializeField] public int acc;
    [SerializeField] public int range;
    [SerializeField] public float AOE;
    [SerializeField] public bool LOS;
    
}
