using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EventCharacterReward : EventCharacter
{
    public override void Open()
    {
        Characters.Clear();
        foreach (GameObject character in GameManager.Instance.enemyTeam)
        {
            NPCController npc = character.GetComponent<NPCController>();
            GameObject newChar = CharacterCreator.Instance.CopyCharacter(npc.gameObject, CharacterCreator.UnitTag.Player);
            
            Characters.Add(newChar);
        }
        
        base.Open();
    }
    protected override GameObject GetCharacter()
    {
        int random = Random.Range(0, Characters.Count);
        return Characters[random];
    }
}
