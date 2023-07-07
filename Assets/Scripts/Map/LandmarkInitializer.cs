using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using LandmarkType = LandmarkManager.LandmarkType;

/// <summary>
/// Controls and does some initialization of the landmark randomization system.
/// </summary>
public class LandmarkInitializer : MonoBehaviour
{
    public bool root;
    public bool boss;
    
    private LandmarkType _landmarkType;
    public Landmark landmark;
    [SerializeField] public List<LandmarkInitializer> nextInitializers = new();
    [SerializeField] public List<LandmarkInitializer> prevInitializers = new();
    
    public void Initialize()
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
        LandmarkType type;
        if (root) type = LandmarkType.Battle;
        else if (boss) type = LandmarkType.Boss;
        else type = LandmarkManager.Instance.landmarkSequence[0];
        
        LandmarkManager.Instance.landmarkSequence.RemoveAt(0);
        return type;
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
            case LandmarkType.Boss:
                SetupBoss();
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
    
    private void SetupBoss()
    {
        landmark = this.AddComponent<BossLandmark>();
    }

}
