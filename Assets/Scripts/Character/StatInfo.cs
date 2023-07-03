using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
[IncludeInSettings(true)]
public class StatInfo
{
    [SerializeField]
    public Stat statType;
    [SerializeField]
    public float statValue;

    public StatInfo Clone()
    {
        return new StatInfo
        {
            statType = this.statType,
            statValue = this.statValue
        };
    }
}
