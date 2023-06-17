using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnpointManager : MonoBehaviour
{
    private SpawnField[] SpawnFields { get; set; }
    private SpawnField[] PlayerSpawnFields { get; set; }
    private SpawnField[] EnemySpawnFields { get; set; }
    private List<GameObject> PlayerTeam { get; set; }
    private List<GameObject> EnemyTeam { get; set; }
    
    void Start()
    {
        SpawnFields = FindObjectsOfType<SpawnField>();
        PlayerSpawnFields = Array.FindAll(SpawnFields, obj => obj.playerField);
        EnemySpawnFields = Array.FindAll(SpawnFields, obj => !obj.playerField);
        
        print(EnemySpawnFields.Length);
        
        PlayerTeam = GameManager.Instance.playerTeam;
        EnemyTeam = GameManager.Instance.enemyTeam;
        
        print(EnemyTeam.Count);
        
        SpawnPlayers();
        SpawnEnemies();
    }
    private void SpawnPlayers()
    {
        int spawnFieldIndex = Random.Range(0, PlayerSpawnFields.Length);
        PlayerSpawnFields[spawnFieldIndex].SpawnCharacters(PlayerTeam);
    }
    private void SpawnEnemies()
    {
        int spawnFieldIndex = Random.Range(0, EnemySpawnFields.Length);
        EnemySpawnFields[spawnFieldIndex].SpawnCharacters(EnemyTeam);
    }
}
