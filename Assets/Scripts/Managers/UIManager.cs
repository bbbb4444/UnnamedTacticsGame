using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ScreenType
{
    None,
    MainMenu,
    ActionMenu,
    ActionMenuTech,
    PauseMenu,
    Ready,
    DarkEvent1,
    DarkEvent2,
    LightEvent1,
    LightEvent2,
    ChooseYourFirstCharacter,
    ActionUnitInfo,
    ChooseYourRewardCharacter
}
public class UIManager : MonoBehaviour
{
   
    private Dictionary<ScreenType, UIScreen> _screens;
    
    public static UIManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        
        _screens = new Dictionary<ScreenType, UIScreen>();
        var allScreens = FindObjectsOfType<UIScreen>();
        foreach (UIScreen screen in allScreens)
        {
            _screens.Add(screen.ScreenType, screen);
        }
    }
 
    public void OpenScreen(ScreenType targetScreen)
    {
        foreach (var screen in _screens)
        {
            if (screen.Value.ScreenType != targetScreen)
            {
                screen.Value.Close();
            }
            else
            {
                screen.Value.Open();
            }
        }
        // UIScreens enable and disable the Player Input component when they open and close.
        // For some reason, this causes input to stop being registered by the EventSystem unless it is toggled off and on.
        // This is why this code is needed.
        EventSystem.current.currentInputModule.enabled = false;
        EventSystem.current.currentInputModule.enabled = true;
    }
    
    public void OpenScreenAdditive(ScreenType targetScreen)
    {
        if (_screens[targetScreen].IsOpen) return;
        _screens[targetScreen].Open();
    }

    public void CloseScreen(ScreenType targetScreen)
    {
        if (_screens.TryGetValue(targetScreen, out UIScreen uiScreen))
        {
            uiScreen.Close();
        }
    }
    public void CloseAllScreens()
    {
        foreach (var screen in _screens)
        {
            if (screen.Value.IsOpen) screen.Value.Close();
        }
    }
    
    public bool IsScreenOpen(ScreenType targetScreen)
    {
        bool isOpened = false;
        foreach (var screen in _screens)
        {
            if (screen.Value.ScreenType == targetScreen)
            {
                isOpened = screen.Value.IsOpen;
            }
        }

        return isOpened;
    }
    
}
