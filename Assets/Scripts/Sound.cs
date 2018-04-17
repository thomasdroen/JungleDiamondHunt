using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip soundClip;

    [HideInInspector] public AudioSource source;
    public AudioMixerGroup audioMixerOutput;

    public bool loop;

    [Range(0,1)]
    public float volume = 1;
    [Range(0.1f, 3)]
    public float pitch = 1;
}
