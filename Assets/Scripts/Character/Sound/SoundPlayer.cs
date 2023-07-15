using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    private AudioSource _audio;
    public TechHandler techHandler;
    
    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void PlayTech()
    {
        print("Ttestst");
        _audio.Play();
    }
}
