using System.Collections.Generic;
using UnityEngine;

public class SpawnField : MonoBehaviour
{
    private Spawnpoint[] Spawnpoints { get; set; }
    public bool playerField;
    void Awake()
    {
        Spawnpoints = GetComponentsInChildren<Spawnpoint>();
        Shuffle(Spawnpoints);
    }

    public void SpawnCharacters(List<GameObject> characters)
    {
        for (var i = 0; i < characters.Count; i++)
        {
            GameObject character = characters[i];
            if (character == null) return;

            if (character.CompareTag("Player"))
            {
                print(character.transform.position);
                character.transform.position = Spawnpoints[i].SpawnPos;
                print(character.transform.position);
            }
            else if (character.CompareTag("NPC"))
            {
                character.transform.position = Spawnpoints[i].SpawnPos;
            }
        }
    }
    void Shuffle(Spawnpoint[] array)
    {
        int length = array.Length;
        for (int i = 0; i < length - 1; i++)
        {
            // Generate a random index between i and the end of the array
            int randomIndex = Random.Range(i, length);
            
            // Swap the elements at positions i and randomIndex
            (array[i], array[randomIndex]) = (array[randomIndex], array[i]);
        }
    }
}
