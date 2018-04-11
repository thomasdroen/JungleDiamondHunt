using UnityEngine;
using System.Collections;

public class LionPickupLine : MonoBehaviour
{
    private string theCollider;

    void OnTriggerEnter(Collider other)
    {
        theCollider = other.tag;
        if (theCollider == "Player")
        {
            GetComponent<AudioSource>().Play();
            GetComponent<AudioSource>().loop = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        theCollider = other.tag;
        if (theCollider == "Player")
        {
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().loop = false;
        }
    }
}