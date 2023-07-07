using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class UIScreen : MonoBehaviour
{
    public ScreenType ScreenType;
    private Canvas canvas;
    protected CanvasGroup canvasGroup;
    
    [SerializeField] public ScreenType nextScreen;
    [SerializeField] public ScreenType lastScreen;
    
    public bool IsOpen { get { return canvas.enabled; }}
 
    protected virtual void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    
    
    public virtual void Open()
    {
        canvas.enabled = true;
        canvasGroup.interactable = true;
    }
 
    public virtual void Close()
    {
        if (canvas.enabled)
        {
            canvas.enabled = false;
            canvasGroup.interactable = false;
        }
    }
 
}