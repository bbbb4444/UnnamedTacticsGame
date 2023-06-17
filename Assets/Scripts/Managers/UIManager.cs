using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ScreenType
{
    ActionMenu,
    ActionMenuTech,
    PauseMenu,
    Ready,
    DarkEvent,
}
public class UIManager : MonoBehaviour
{
   
    private Dictionary<ScreenType, UIScreen> _screens;
    
    public ScreenType LastScreen { get; set; }
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
        StartCoroutine(OpenScreenDelay(targetScreen));
    }

    private IEnumerator OpenScreenDelay(ScreenType targetScreen)
    {
        yield return new WaitForSeconds(0f);
        foreach (var screen in _screens)
        {
            if (screen.Value.ScreenType != targetScreen)
            {
                screen.Value.Close();
            }
            else
            {
                LastScreen = screen.Value.ScreenType;
                screen.Value.Open();
            }
        }
    }
    public void CloseScreen(ScreenType targetScreen)
    {
        if (_screens.TryGetValue(targetScreen, out UIScreen uiScreen))
        {
            uiScreen.Close();
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
