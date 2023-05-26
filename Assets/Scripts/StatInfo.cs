using System.Collections;
using System.Collections.Generic;
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
}
