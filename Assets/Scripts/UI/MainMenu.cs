using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UIScreen
{
    [SerializeField] private Button startButton;
    // Start is called before the first frame update
    void Start()
    {
        startButton.Select();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartButtonClicked()
    {
        SceneSwitcher.SwitchToScene("Map1");
        UIManager.Instance.CloseAllScreens();
    }
}
