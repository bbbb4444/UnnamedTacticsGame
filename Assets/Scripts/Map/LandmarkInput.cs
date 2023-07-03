using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LandmarkInput : MonoBehaviour
{
    private void OnEnable()
    {
        CanUse = true;
        Landmark.OnComplete += Enable;
        Landmark.OnEnter += Disable;
        
        EventCharacter.OnOpen += Disable;
        EventCharacter.OnClose += Enable;
    }

    private void OnDisable()
    {
        Landmark.OnComplete -= Enable;
        Landmark.OnEnter -= Disable;
        
        EventCharacter.OnOpen -= Disable;
        EventCharacter.OnClose -= Enable;
    }

    private bool CanUse { get; set; }
    
    public void OnSubmit()
    {
        if (!CanUse/* || !LandmarkManager.Instance.current.IsActive || LandmarkManager.Instance.current.IsCompleted*/) return;
        
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
            if (current.prevLandmarks.Count == 0) return;
            selected = current.prevLandmarks[0];
        }
        else if (x == 1)
        {
            if (current.nextLandmarks.Count == 0) return;
            selected = current.nextLandmarks.Count == 2 ? current.nextLandmarks[1] : current.nextLandmarks[0];
        }
        else if (y == 1)
        {
            if (current.NeighborAbove == null)
            {
                if (current.nextLandmarks.Count == 0) return;
                selected = current.nextLandmarks[0];
            }
            else selected = current.NeighborAbove;
        }
        else if (y == -1)
        {
            if (current.NeighborBelow == null)
            {
                if (current.nextLandmarks.Count  < 2) return;
                selected = current.nextLandmarks[1];
            }
            else selected = current.NeighborBelow;
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
