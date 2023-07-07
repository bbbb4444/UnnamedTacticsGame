using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerTeamHolder;
    
    [SerializeField] private List<GameObject> playerTeamPrefabs;
    
    public List<GameObject> playerTeam;
    public List<CharacterController> playerControllers;
    public Dictionary<GameObject, CharacterController> playerObjectToController = new();
    public List<GameObject> enemyTeam;
    
    public static GameManager Instance;

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

    private void Start()
    {
        if (playerTeam.Count == 0)
        {
            UIManager.Instance.OpenScreen(ScreenType.ChooseYourFirstCharacter);
        }
    }

    public void InitializePlayerTeam()
    {
        foreach (GameObject prefab in playerTeamPrefabs)
        {
            GameObject player = Instantiate(prefab, transform);
            
            //player.SetActive(false);
            playerTeam.Add(player);
        }
        
        foreach (GameObject player in playerTeam)
        {
            CharacterController controller = player.GetComponent<CharacterController>();

            playerControllers.Add(controller);
        }
    }

    public void AddPlayer(GameObject player)
    {
        if (!playerTeam.Contains(player))
        {
            GameObject p = Instantiate(player, playerTeamHolder.transform);
            playerTeam.Add(p);
            playerControllers.Add(p.GetComponent<CharacterController>());
        }
    }
    public void RemovePlayer(CharacterController player)
    {
        if (playerControllers.Contains(player)) playerControllers.Remove(player);
    }
    public void WinBattle()
    {
        LandmarkManager.Instance.gameObject.SetActive(true);

        foreach (GameObject player in playerTeam)
        {
            player.transform.position = playerTeamHolder.transform.position;
        }
        
        SceneSwitcher.SwitchToScene("Map1");
        LandmarkManager.Instance.WinLandmark();
    }

    public void LoseBattle()
    {
        
    }

    
    
}
