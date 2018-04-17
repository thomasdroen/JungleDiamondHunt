using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger : MonoBehaviour {

	public AudioClip jungle;
	public AudioClip tribe;
	public AudioClip endSong;

    private AudioSource source;

	// Use this for initialization
	void Awake ()
	{
	    source = GetComponent<AudioSource>();
	    source.clip = jungle;
        source.Play();
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.J))
		{

			if (source.clip == jungle)
			{
                source.Stop();
			    source.clip = tribe;
                source.Play();

			}

			else
			{

				source.Stop();
			    source.clip = jungle;
                source.Play();

			}

		}
		//if (Input.GetKeyUp (KeyCode.Escape)) {
		//	if (!jungle.isPlaying || !tribe.isPlaying) {
		//		tribe.Play ();
		//	}
		//}
	}

    public void Play()
    {
        if (source.clip != null)
        {
            source.UnPause();
        }
    }

    public void Pause()
    {
        source.Pause();
    }

	public void Stop()
	{
		source.Stop ();
	}

	public void EndGame(){
		source.Stop();
		source.clip = endSong;
		source.Play();
		source.loop = false;
	}

}
