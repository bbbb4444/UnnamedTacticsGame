using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Custom/ChararacterRoster")]
public class CharacterRoster : ScriptableObject
{
    [SerializeField] public List<GameObject> characters;
}