using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreator : MonoBehaviour
{
    public static CharacterCreator Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        EventCharacter.OnClose += ClearCreatedCharacters;
    }

    private void OnDisable()
    {
        EventCharacter.OnClose -= ClearCreatedCharacters;
    }

    public enum UnitTag
    {
        Player,
        Enemy
    }

    public GameObject CopyCharacter(GameObject charObject, UnitTag unitTag)
    {
        GameObject character = Instantiate(Resources.Load<GameObject>("CharInstantiate"), Instance.transform);
        CharacterController controller = charObject.GetComponent<CharacterController>();
        
        if (unitTag == UnitTag.Player)
        {
            character.tag = "Player";
            character.AddComponent<CharacterController>().Stats = controller.Stats;
            character.GetComponent<CharacterController>().Initialize();
            character.GetComponent<TechHandler>().Techinques.Clear();
            character.GetComponent<TechHandler>().Techinques = charObject.GetComponent<TechHandler>().Techinques;
        }
        
        if (unitTag == UnitTag.Enemy)
        {
            character.tag = "NPC";
            character.AddComponent<CharacterController>().Stats = controller.Stats;
            character.GetComponent<CharacterController>().Initialize();
            character.GetComponent<TechHandler>().Techinques.Clear();
            character.GetComponent<TechHandler>().Techinques = charObject.GetComponent<TechHandler>().Techinques;
        }
        
        return character;
    }
    
    public GameObject CreateCharacter(CharStats stats, UnitTag unitTag)
    {
        GameObject character = Instantiate(Resources.Load<GameObject>("CharInstantiate"), Instance.transform);
        if (unitTag == UnitTag.Player)
        {
            character.tag = "Player";
            print(character);
            character.AddComponent<CharacterController>().Stats = stats;
            character.GetComponent<CharacterController>().Initialize();
        }

        if (unitTag == UnitTag.Enemy)
        {
            character.tag = "NPC";
            character.AddComponent<NPCController>().Stats = stats;
            character.GetComponent<CharacterController>().Initialize();
        }
        return character;
    }

    
    
    
    public GameObject CreateCharacter(GameObject character, CharType type)
    {
        GameObject newCharacter = Instantiate(character);
        newCharacter.GetComponent<CharacterController>().CharType = type;
        return newCharacter;
    }

    private void ClearCreatedCharacters()
    {
        while (transform.childCount > 0) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
