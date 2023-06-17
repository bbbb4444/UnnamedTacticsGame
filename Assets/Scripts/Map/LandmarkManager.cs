using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LandmarkManager : MonoBehaviour
{
    [SerializeField] private CursorMovement cursor;
    private LandmarkInput _landmarkInput;
    
    private Landmark[] _landmarks;
    [SerializeField] public Landmark current;
    
    public static LandmarkManager Instance;

    private void OnEnable()
    {
 
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        _landmarkInput = GetComponent<LandmarkInput>();
        _landmarks = FindObjectsOfType<Landmark>();
        SelectLandmark(current);
        current.Activate();
        current.Select();
    }

    public void SelectLandmark(Landmark landmark)
    {
        FindCursor();
        current.ResetColor();
        current = landmark;
        current.Select();
        cursor.MoveTo(current.gameObject, 0);
    }

    public void WinLandmark()
    {
        current.Complete();
    }

    private void FindCursor()
    {
        if (cursor == null)
        {
            cursor = FindObjectOfType<CursorMovement>();
            print(cursor);
        }
    }
    private void HideLandmarks()
    {
        foreach (Landmark landmark in _landmarks)
        {
            landmark.enabled = false;
        }
    }
    private void ShowLandmarks()
    {
        foreach (Landmark landmark in _landmarks)
        {
            landmark.enabled = true;
        }
    }
}
