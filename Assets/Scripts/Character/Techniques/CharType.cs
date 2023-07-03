using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Type")]
public class CharType : ScriptableObject
{
    [SerializeField] public Element element;
    [SerializeField] public Augment augment;
    [SerializeField] public Type type;
    [SerializeField] public CharType lightType;
    [SerializeField] public CharType darkType;
    [SerializeField] public Sprite icon;
    [SerializeField] public List<Color> colors;
    [SerializeField] public List<Effectiveness> effectiveness;
    [SerializeField] public List<Bonus> bonuses;
}