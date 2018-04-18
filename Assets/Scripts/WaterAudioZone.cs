using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class WaterAudioZone : MonoBehaviour
{
    private GameObject theCollider;
    public HeadBob playerHeadBob;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == RigidbodyFirstPersonController.player.gameObject)
        {
            if (playerHeadBob)
            {
                playerHeadBob.motionBob.inWater = true;
                AudioManager.Instance.PlaySound("Splash");
            }
            else
            {
                Debug.LogError("Headbob not set!");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == RigidbodyFirstPersonController.player.gameObject)
        {
            if (playerHeadBob)
            {
                playerHeadBob.motionBob.inWater = false;
            }
            else
            {
                Debug.LogError("Headbob not set!");
            }
        }
    }
}