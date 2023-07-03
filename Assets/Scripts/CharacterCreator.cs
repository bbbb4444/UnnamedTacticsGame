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
    
    public GameObject CreateCharacter(CharStats stats, UnitTag unitTag)
    {
        GameObject character = Resources.Load<GameObject>("CharInstantiate");
        if (unitTag == UnitTag.Player)
        {
            character.tag = "Player";
            character.AddComponent<CharacterController>().Stats = stats;
        }

        if (unitTag == UnitTag.Enemy)
        {
            character.tag = "NPC";
            character.AddComponent<NPCController>().Stats = stats;
        }
        return character;
    }

    private void ClearCreatedCharacters()
    {
        while (transform.childCount > 0) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
