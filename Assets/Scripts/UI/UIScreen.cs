using UnityEngine;
using UnityEngine.InputSystem;

public class UIScreen : MonoBehaviour
{
    public ScreenType ScreenType;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private PlayerInput playerInput;
    
    [SerializeField] public ScreenType nextScreen;
    [SerializeField] public ScreenType lastScreen;
    
    public bool IsOpen { get { return canvas.enabled; }}
 
    protected virtual void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        TryGetComponent(out playerInput);
    }
    
    
    public virtual void Open()
    {
        if (!canvas.enabled)
        {
            canvas.enabled = true;
            canvasGroup.interactable = true;
            if (playerInput) playerInput.ActivateInput();
        }
    }
 
    public virtual void Close()
    {
        if (canvas.enabled)
        {
            canvas.enabled = false;
            canvasGroup.interactable = false;
            if (playerInput) playerInput.DeactivateInput();
            
        }
    }
 
}