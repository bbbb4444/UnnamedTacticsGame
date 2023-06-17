using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
[IncludeInSettings(true)]
public class TechInfo
{
    [SerializeField]
    public Tech tech;
    [SerializeField]
    public float statValue;
}
