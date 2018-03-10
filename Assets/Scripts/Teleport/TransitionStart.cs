using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionStart : MonoBehaviour
{

    public Transform teleportLocation;
    public bool interactable;

    public void onPlayerInteract(GameObject player)
    {
        if (interactable)
        {
            player.transform.position = teleportLocation.position;
        }
    }
}
