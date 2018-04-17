using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;

    public Sound[] sounds;
    private List<Sound> musicSounds;

    private AudioSource source;

    private int currentMusicIndex = 0;

    // Use this for initialization
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        musicSounds = new List<Sound>();

        foreach (var sound in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            sound.source = source;
            if (sound.audioMixerOutput != null)
            {
                source.outputAudioMixerGroup = sound.audioMixerOutput;
                if (sound.audioMixerOutput.name == "Music")
                {
                    musicSounds.Add(sound);
                }
            }
            source.clip = sound.soundClip;
            source.volume = sound.volume;
            source.pitch = sound.pitch;
            source.loop = sound.loop;

        }
    }

    private void Start()
    {
        PlayNextSong();
    }

    private void PlayNextSong()
    {
        if (musicSounds.Count <= 0)
        {
            Debug.LogError("No songs found!");
        }

        musicSounds[currentMusicIndex].source.Stop();
        currentMusicIndex = currentMusicIndex + 1 >= musicSounds.Count ? 0 : currentMusicIndex + 1;
        musicSounds[currentMusicIndex].source.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            PlayNextSong();
        }
    }


    public void PlaySound(int index, float delay = 0f)
    {
        if (index < 0 || index >= sounds.Length)
        {
            Debug.LogError("Sound not found, index invalid!");
            return;
        }

        if (delay > 0)
        {
            sounds[index].source.PlayDelayed(delay);
        }
        else
        {
            sounds[index].source.Play();
        }
    }

    public void PlaySound(string name, float delay = 0f)
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

    public void StopSound(string name, bool pause = false)
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

    public void StopMusic()
    {
        foreach (var musicSound in musicSounds)
        {
            musicSound.source.Stop();
        }
    }
}
