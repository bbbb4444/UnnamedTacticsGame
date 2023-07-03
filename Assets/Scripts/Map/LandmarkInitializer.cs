using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


/// <summary>
/// Controls and does some initialization of the landmark randomization system.
/// </summary>
public class LandmarkInitializer : MonoBehaviour
{
    public bool root;
    private enum LandmarkType
    {
        Battle,
        Event
    }

    private LandmarkType _landmarkType;
    public Landmark landmark;
    [SerializeField] public List<LandmarkInitializer> nextInitializers = new();
    [SerializeField] public List<LandmarkInitializer> prevInitializers = new();
    
    private void Start()
    {
        GetComponent<Renderer>().enabled = false;
        _landmarkType = GetRandomLandmarkType();
        InitializeLandmark(_landmarkType);
    }

    /// <summary>
    /// </summary>
    /// <returns>A random landmarkType num, weighted.</returns>
    private LandmarkType GetRandomLandmarkType()
    {
        int num = Random.Range(0, 10);
        return num > 6 ? LandmarkType.Event : LandmarkType.Battle;
    }

    /// <summary>
    /// Initializes the chosen landmark type.
    /// </summary>
    /// <param name="landmarkType">The landmarkType to initialize.</param>
    private void InitializeLandmark(LandmarkType landmarkType)
    {
        switch (landmarkType)
        {
            case LandmarkType.Battle:
                SetupBattle();
                break;
            case LandmarkType.Event:
                SetupEvent();
                break;
        }
    }

    private void SetupBattle()
    {
        landmark = this.AddComponent<BattleLandmark>();
    }

    private void SetupEvent()
    {
        landmark = this.AddComponent<EventLandmark>();
    }
}
