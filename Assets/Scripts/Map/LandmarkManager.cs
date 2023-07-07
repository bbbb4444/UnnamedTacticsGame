using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LandmarkManager : MonoBehaviour
{
    public enum LandmarkType
    {
        Battle,
        Event,
        Boss
    }
    
    [SerializeField] private CursorMovement cursor;
    
    private Landmark[] _landmarks;
    private LandmarkInitializer[] _initializers;
    public List<LandmarkType> landmarkSequence;
    private Dictionary<LandmarkType, int> _landmarkWeights = new Dictionary<LandmarkType, int>
    {
        { LandmarkType.Battle, 9 },
        { LandmarkType.Event, 5 }
    };
    [SerializeField] public Landmark current;
    
    public static LandmarkManager Instance;
    


 
    
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
        _initializers = FindObjectsOfType<LandmarkInitializer>();
        InitializeLandmarkSequence();
        InitializeLandmarks();
        SetupLandmarkConnections(_initializers);
        current = GetRoot(_initializers);
        
        _landmarks = FindObjectsOfType<Landmark>();
        SelectLandmark(current);
        current.Activate();
        current.Select();
    }

    private void InitializeLandmarkSequence()
    {
        int sumWeights = _landmarkWeights.Values.Sum();
        int totalLandmarks = _initializers.Length;

        float battlePercent = (float) _landmarkWeights[LandmarkType.Battle] / sumWeights;
        float eventPercent = (float) _landmarkWeights[LandmarkType.Event] / sumWeights;

        int totalBattleLandmarks = (int) Mathf.Round(battlePercent * totalLandmarks);
        int totalEventLandmarks = (int) Mathf.Round(eventPercent * totalLandmarks);

        print("B: " + totalBattleLandmarks + "__ E: " + totalEventLandmarks);
        
        for (int i = 0; i < totalBattleLandmarks; i++)
        {
            landmarkSequence.Add(LandmarkType.Battle);
        }
        for (int i = 0; i < totalEventLandmarks; i++)
        {
            landmarkSequence.Add(LandmarkType.Event);
        }
        
        Shuffle(landmarkSequence);
    }

    private void InitializeLandmarks()
    {
        foreach (LandmarkInitializer initializer in _initializers)
        {
            initializer.Initialize();
        }
    }
    
    private void Shuffle(List<LandmarkType> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
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
        enabled = false;
    }
    private void ShowLandmarks()
    {
        enabled = true;
    }
}
