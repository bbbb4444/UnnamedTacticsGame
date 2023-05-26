using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private GameObject[] players;
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {
        foreach (GameObject player in players)
        {
            player.transform.position = transform.position;
        }
    }
}
