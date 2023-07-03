using System.Collections;
using UnityEngine;


public class EventLandmark : Landmark
{
    private enum EventType
    {
        Light,
        Dark
    }

    [SerializeField] private EventType eventType;
    public override void EnterLandmark()
    {
        base.EnterLandmark();
        StartCoroutine(OpenScreenAfterDelay());
    }
    
    private IEnumerator OpenScreenAfterDelay()
    {
        yield return new WaitForSeconds(0f);
        switch (eventType)
        {
            case EventType.Light:
                UIManager.Instance.OpenScreen(ScreenType.LightEvent1);
                break;
            case EventType.Dark:
                UIManager.Instance.OpenScreen(ScreenType.DarkEvent1);
                break;
            default:
                break;
        }
        
    }
    
    protected override void CreateAppearance()
    {
        Appearance = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        base.CreateAppearance();
    }
}
