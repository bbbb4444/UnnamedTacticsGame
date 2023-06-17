using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public List<GameObject> playerTeam;
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

    public void WinBattle()
    {
        SceneSwitcher.SwitchToScene("Map1");
        LandmarkManager.Instance.WinLandmark();
    }

    public void LoseBattle()
    {
        
    }

}
