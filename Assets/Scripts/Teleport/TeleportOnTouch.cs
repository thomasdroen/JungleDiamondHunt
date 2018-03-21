using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportOnTouch : MonoBehaviour
{
    public Transform teleportLocation;
    public GameObject player;

    public void onPlayerTouch(Collider other)
    {
        if (gameObject.activeSelf && other.gameObject == player)
        {
            player.transform.position = teleportLocation.position;
        }
    }
}
