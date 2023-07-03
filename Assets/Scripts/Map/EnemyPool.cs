using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyPool : MonoBehaviour
{
    public enum Pool
    {
        GrassEasy,
        GrassHard
    }
    public static EnemyPool Instance;
    [SerializeField] private CharacterRoster grass;
    
    private void Awake()
    {
        Instance = this;
    }

    public List<GameObject> GetRandomEnemies(Pool pool, int amount)
    {
        List<GameObject> selectedEnemies = new();
        List<GameObject> enemyPool = new();

        switch (pool)
        {
            case Pool.GrassEasy:
                enemyPool = grass.characters.ToList();
                break;
            case Pool.GrassHard:
                enemyPool = grass.characters.ToList();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pool), pool, null);
        }
        
        for (int i = 0; i < amount; i++)
        {
            int randInt = Random.Range(0, enemyPool.Count);
            GameObject enemy = enemyPool[randInt];
            selectedEnemies.Add(enemy);
            if (enemyPool.Count > 1) enemyPool.Remove(enemy);
        }

        return selectedEnemies;
    }
}
