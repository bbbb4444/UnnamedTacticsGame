using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EventCharacter : Event
{
    protected int Options = 3;
    protected List<GameObject> Characters = new();
    protected List<CharacterController> CharacterControllers = new();
    
    

    public override void Open()
    {
        base.Open();
        Characters = BuildCharacterList();
        foreach (GameObject character in Characters)
        {
            CharacterControllers.Add(character.GetComponent<CharacterController>());
        }
        PopulateOptions();
    }
    protected virtual GameObject GetCharacter()
    {
        return new GameObject();
    }
    
    protected List<GameObject> BuildCharacterList()
    {
        List<GameObject> uniqueCharacters = new();
        List<GameObject> characters = new();
        
        for (int i = 0; i < Options; i++)
        {
            GameObject character;
            
            do { character = GetCharacter();
            } while (uniqueCharacters.Contains(character) && uniqueCharacters.Count < Characters.Count); //TODO: If multiple instances of a single creature are in a battle, this might mess up
            
            characters.Add(Instantiate(character, CharacterCreator.Instance.transform));
            uniqueCharacters.Add(character);
        }

        return characters;
    }
    public void OnOption1Clicked()
    {
        print("test");
        GameManager.Instance.AddPlayer(Characters[0]);
        Close();
    }
    public void OnOption2Clicked()
    {
        GameManager.Instance.AddPlayer(Characters[1]);
        Close();
    }
    public void OnOption3Clicked()
    {
        GameManager.Instance.AddPlayer(Characters[2]);
        Close();
    }
    
    private void PopulateOptions()
    {
        for (int i = 0; i < optionButtons.Count; i++)
        {
            OptionCharacter option = optionButtons[i].GetComponent<OptionCharacter>();
            CharType type = CharacterControllers[i].CharType;
            CharStats stats = CharacterControllers[i].Stats;
            List<Technique> techs = CharacterControllers[i].TechHandler.Techinques;
            
            option.SetName(CharacterControllers[i].Name);
            option.SetType1(type);
            option.SetStats(stats);
            option.SetTechs(techs);
        }
    }
}
