using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;

    public Sound[] sounds;

    private AudioSource source;

	// Use this for initialization
	void Awake () {
	    if (Instance == null)
	    {
	        Instance = this;
	    }
	    else
	    {
	        Destroy(gameObject);
	    }

	    foreach (var sound in sounds)
	    {
	        AudioSource source = gameObject.AddComponent<AudioSource>();
	        source.clip = sound.soundClip;
	        source.volume = sound.volume;
	        source.pitch = sound.pitch;
	        source.loop = sound.loop;
        }
	}

    public void playSound(string name, float delay = 0f)
    {
        Sound s = Array.Find(sounds, sound => string.Equals(sound.name.ToLower(), name.ToLower()));
        if (s != null)
        {
            if (delay > 0)
            {
                s.source.PlayDelayed(delay);
            }
            else
            {
                s.source.Play();
            }
        }
        else
        {
            Debug.LogError(name + " sound not found!");
        }
    }

    public void stopSound(string name, bool pause = false)
    {
        Sound s = Array.Find(sounds, sound => string.Equals(sound.name.ToLower(), name.ToLower()));
        if (s != null)
        {
            if (!pause)
            {
                s.source.Stop();
                return;
            }
            if (s.source.isPlaying)
            {
                s.source.Pause();
            }
            else
            {
                s.source.Play();
            }
        }
        else
        {
            Debug.LogError(name + " sound not found!");
        }
    }
}
