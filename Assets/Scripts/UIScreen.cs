using UnityEngine;
public class UIScreen : MonoBehaviour
{
    public ScreenType ScreenType;
    private Canvas canvas;
 
    public bool IsOpen { get { return canvas.enabled; }}
 
    protected virtual void Awake()
    {
        canvas = GetComponent<Canvas>();
    }
 
    public virtual void Open()
    {
        if (!canvas.enabled)
        {
            canvas.enabled = true;
        }
    }
 
    public virtual void Close()
    {
        if (canvas.enabled)
        {
            canvas.enabled = false;
        }
    }
 
}