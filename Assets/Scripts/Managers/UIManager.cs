using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

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
    public InputSystemUIInputModule inputModule;
    public InputActionAsset player;
    public InputActionAsset npc;

    public void DisablePlayerInput()
    {
        if (inputModule.actionsAsset != npc) inputModule.actionsAsset = npc;
    }

    public void EnablePlayerInput()
    {
        if (inputModule.actionsAsset != player) inputModule.actionsAsset = player;
    }
    
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
        
        inputModule = EventSystem.current.GetComponent<InputSystemUIInputModule>();
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
        // Disabling player input (for NPC turns) is achieved by changing the actionmap to an empty one.
        // This introduces various bugs such as input being permanently disabled and not being able to select buttons.
        // This band-aid fixes the bugs.
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
