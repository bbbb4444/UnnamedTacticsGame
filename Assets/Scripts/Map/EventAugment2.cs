using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EventAugment2 : Event
{
    [SerializeField] protected Augment augment;

    public List<CharType> newTypes = new();
    public List<CharacterController> randomMembers = new();
    public List<int> randomMemberIndices = new();
    
    public override void Open()
    {
        base.Open();
        
        randomMembers = SelectRandomPartyMembers(2);
        
        
        for (int i = 0; i < randomMembers.Count; i++)
        {
            Option option = optionButtons[i].GetComponent<Option>();
            CharType type = randomMembers[i].CharType;
            CharType newType;
            if (augment == Augment.Light) newType = type.lightType;
            else if (augment == Augment.Dark) newType = type.darkType;
            else newType = type.darkType;
            
            newTypes.Add(newType);
            
            option.SetName(randomMembers[i].Name);
            option.SetType1(type);
            option.SetType2(newType);
        }
        
    }

    public void OnOption1ButtonClicked()
    {
        randomMembers[0].CharType = newTypes[0];
        GameObject augmentedPlayer = randomMembers[0].gameObject;

        int index = randomMemberIndices[0];
        
        GameManager.Instance.playerTeam[index] = augmentedPlayer;
        GameManager.Instance.playerControllers[index] = randomMembers[0];

        Exit();
    }
    public void OnOption2ButtonClicked()
    {
        randomMembers[1].CharType = newTypes[1];
        GameObject augmentedPlayer = randomMembers[1].gameObject;
        
        int index = randomMemberIndices[0];
        
        GameManager.Instance.playerTeam[index] = augmentedPlayer;
        GameManager.Instance.playerControllers[index] = randomMembers[1];

        Exit();
    }

    private void Exit()
    {
        newTypes.Clear();
        randomMembers.Clear();
        randomMemberIndices.Clear();
        
        UIManager.Instance.CloseAllScreens();
        LandmarkManager.Instance.WinLandmark();
    }
    
    private List<CharacterController> SelectRandomPartyMembers(int amount)
    {
        List<CharacterController> players = GameManager.Instance.playerControllers;
        players = players.FindAll(obj => obj.CharType.augment == Augment.None);
        
        
        List<CharacterController> selectedPlayers = new();
        for (int i = 0; i < amount; i++)
        {
            int rInt = Random.Range(0, players.Count);
            CharacterController rCharacter = players[rInt];
            randomMemberIndices.Add(GameManager.Instance.playerControllers.IndexOf(rCharacter));
            selectedPlayers.Add(rCharacter);
            if (players.Count > 1) players.Remove(players[rInt]);
        }

        return selectedPlayers;
    }
    
    
    private void UpdateOptions()
    {
        foreach (Button option in optionButtons)
        {
            
        }
    }
}
