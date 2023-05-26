using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ScreenType
{
    ActionMenu,
    PauseMenu
}
public class UIManager : MonoBehaviour
{
   
    private static Dictionary<ScreenType, UIScreen> _screens;
 
    private void Awake()
    {
        _screens = new Dictionary<ScreenType, UIScreen>();
 
        var allScreens = FindObjectsOfType<UIScreen>();
        foreach (UIScreen screen in allScreens)
        {
            _screens.Add(screen.ScreenType, screen);
        }
    }
 
    public static void OpenScreen(ScreenType targetScreen)
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
    }

    public static void CloseScreen(ScreenType targetScreen)
    {
        if (_screens.TryGetValue(targetScreen, out UIScreen uiScreen))
        {
            uiScreen.Close();
        }
    }
}
