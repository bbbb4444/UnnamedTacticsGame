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
        LandmarkInitializer[] initializers = FindObjectsOfType<LandmarkInitializer>();
        SetupLandmarkConnections(initializers);
        current = GetRoot(initializers);
        
        _landmarkInput = GetComponent<LandmarkInput>();
        _landmarks = FindObjectsOfType<Landmark>();
        SelectLandmark(current);
        current.Activate();
        current.Select();
    }

    private void SetupLandmarkConnections(LandmarkInitializer[] initializers)
    {
        foreach (LandmarkInitializer initializer in initializers)
        {
            Landmark landmark = initializer.landmark;

            foreach (LandmarkInitializer nextInitializer in initializer.nextInitializers)
            {
                Landmark nextLandmark = nextInitializer.landmark;
                landmark.nextLandmarks.Add(nextLandmark);
            }
            foreach (LandmarkInitializer prevInitializer in initializer.prevInitializers)
            {
                Landmark prevLandmark = prevInitializer.landmark;
                landmark.prevLandmarks.Add(prevLandmark);
            }
        }
    }

    private Landmark GetRoot(LandmarkInitializer[] initializers)
    {
        foreach (LandmarkInitializer initializer in initializers)
        {
            if (initializer.root) return initializer.landmark;
        }

        return null;
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
