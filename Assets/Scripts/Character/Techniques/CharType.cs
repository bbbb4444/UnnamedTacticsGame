using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Type")]
public class CharType : ScriptableObject
{
    [SerializeField] public Type type;
    [SerializeField] public Sprite icon;
    [SerializeField] public List<Color> colors;
    [SerializeField] public List<int> effectiveness;
    [SerializeField] public List<Bonus> bonuses;
}
