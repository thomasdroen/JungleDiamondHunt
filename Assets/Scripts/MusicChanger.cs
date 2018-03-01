using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger : MonoBehaviour {

	public AudioSource jungle;
	public AudioSource tribe;

	// Use this for initialization
	void Start () {
		jungle.Play ();
		
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.J))
		{

			if (jungle.isPlaying)
			{

				jungle.Stop();

				tribe.Play();

			}

			else
			{

				tribe.Stop();

				jungle.Play();

			}

		}
		if (Input.GetKeyUp (KeyCode.Escape)) {
			if (!jungle.isPlaying || !tribe.isPlaying) {
				tribe.Play ();
			}
		}
	}
}
