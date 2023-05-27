using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum Stat
{
    Health,
    Strength,
    Defense,
    Skill
}


[CreateAssetMenu(menuName = "Folder 1/Char Stats")]
public class CharStats : ScriptableObject
{
    private TextAsset _typeEffectiveText;
    private TextAsset _typeNamesText;
    
    [SerializeField]
    public List<StatInfo> baseStatInfo = new List<StatInfo>();
    [SerializeField]
    public List<StatInfo> statInfo = new List<StatInfo>();
    

    private int[,] _typeEffective;
    private string[,] _typeNames;

    [SerializeField] [ReadOnly] private Type type;
    [SerializeField] private Element element;
    [SerializeField] private Augment augment;

    
    
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
        statInfo = baseStatInfo;
    }
}
