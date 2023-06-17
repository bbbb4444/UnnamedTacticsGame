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
        switch (eventType)
        {
            case EventType.Light:
                break;
            case EventType.Dark:
                UIManager.Instance.OpenScreen(ScreenType.DarkEvent);
                break;
            default:
                break;
        }
        base.EnterLandmark();
    }
}
