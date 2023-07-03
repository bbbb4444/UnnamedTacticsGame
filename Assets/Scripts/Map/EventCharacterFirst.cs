using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EventCharacterFirst : EventCharacter
{
    [SerializeField] private CharacterRoster roster;
    

    public override void Open()
    {
        base.Open();
    }
    

    protected override GameObject GetCharacter()
    {
        int random = Random.Range(0, roster.characters.Count);
        return roster.characters[random];
    }
}
