using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class WaterAudioZone : MonoBehaviour
{
	private GameObject theCollider;

    void OnTriggerEnter(Collider other)
    {
		theCollider = other.gameObject;
		if (theCollider == RigidbodyFirstPersonController.player.gameObject)
        {
            GetComponent<AudioSource>().Play();
            GetComponent<AudioSource>().loop = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
		theCollider = other.gameObject;
		if (theCollider == RigidbodyFirstPersonController.player.gameObject)
        {
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().loop = false;
        }
    }
}