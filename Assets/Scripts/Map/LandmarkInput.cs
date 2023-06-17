using UnityEngine;
using UnityEngine.InputSystem;

public class LandmarkInput : MonoBehaviour
{
    private void OnEnable()
    {
        CanUse = true;
        Landmark.OnComplete += Enable;
        Landmark.OnEnter += Disable;
    }

    private bool CanUse { get; set; }
    
    public void OnSubmit()
    {
        if (!CanUse || !LandmarkManager.Instance.current.IsActive || LandmarkManager.Instance.current.IsCompleted) return;
        
        LandmarkManager.Instance.current.EnterLandmark();
    }
    
    public void OnNavigate(InputValue value)
    {
        if (!CanUse) return;
        
        Landmark current = LandmarkManager.Instance.current;
        
        int x = (int) value.Get<Vector2>().x;
        int y = (int) value.Get<Vector2>().y;
        Landmark selected = current;
        
        if (x == -1)
        {
            if (current.previousLandmark == null) return;
            selected = current.previousLandmark;
        }
        else if (x == 1)
        {
            if (current.nextLandmarks.Count == 0) return;
            selected = current.nextLandmarks[0];
        }
        else if (y == 1)
        {
            if (current.NeighborAbove == null) return;
            selected = current.NeighborAbove;
        }
        else if (y == -1)
        {
            if (current.NeighborBelow == null) return;
            selected = current.NeighborBelow;
        }

        LandmarkManager.Instance.SelectLandmark(selected);
    }

    private void Disable()
    {
        CanUse = false;
    }

    private void Enable()
    {
        CanUse = true;
    }
}
