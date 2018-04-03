using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip soundClip;

    [HideInInspector] public AudioSource source;

    public bool loop;

    [Range(0,1)]
    public float volume;
    [Range(0.1f, 3)]
    public float pitch;
}
