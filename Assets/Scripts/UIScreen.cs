using UnityEngine;
public class UIScreen : MonoBehaviour
{
    public ScreenType ScreenType;
    private Canvas canvas;
 
    public bool IsOpen { get { return canvas.enabled; }}
 
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }
 
    public void Open()
    {
        if (!canvas.enabled)
        {
            canvas.enabled = true;
        }
    }
 
    public void Close()
    {
        if (canvas.enabled)
        {
            canvas.enabled = false;
        }
    }
 
}