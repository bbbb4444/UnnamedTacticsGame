using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum Stat
{
    Health,
    Strength,
    Defense,
    Speed
}


[CreateAssetMenu(menuName = "Custom/Char Stats")]
public class CharStats : ScriptableObject
{
    private TextAsset _typeEffectiveText;
    private TextAsset _typeNamesText;

    public List<Technique> movePool = new();
    public List<StatInfo> baseStatInfo = new List<StatInfo>();
    private List<StatInfo> statInfo = new List<StatInfo>();
    

    private int[,] _typeEffective;
    private string[,] _typeNames;

    [SerializeField] public CharType type;
    //[SerializeField] private Element element;
    //[SerializeField] private Augment augment;

    
    public float GetBaseStat(Stat stat)
    {
        foreach (StatInfo s in baseStatInfo)
        {
            if (s.statType == stat) return s.statValue;
        }

        Action<object> logError = Debug.LogError;
        logError("No stat value found for " + stat + " in " + this.name);
        return 0;
    }  
    
    public float GetStat(Stat stat)
    {
        foreach (StatInfo s in statInfo)
        {
            if (s.statType == stat) return s.statValue;
        }

        Action<object> logError = Debug.LogError;
        logError("No stat value found for " + stat + " in " + this.name);
        return 0;
    }

    public void SetStat(Stat stat, float value)
    {
        foreach (StatInfo s in statInfo)
        {
            if (s.statType == stat)
            {
                s.statValue = value;
                return;
            }
        }

        Action<object> logError = Debug.LogError;
        logError("No stat value found for " + stat + " in " + this.name);
    }

    
    public void AddStat(Stat stat, float value)
    {
        foreach (StatInfo s in statInfo)
        {
            if (s.statType == stat)
            {
                s.statValue += value;
            } 
        }
    }
    private void Awake()
    {
        foreach (StatInfo stat in baseStatInfo)
        {
            
            statInfo.Add(stat.Clone());
        }
    }
}
