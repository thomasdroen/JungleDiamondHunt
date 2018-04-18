using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class LionPickupLine : MonoBehaviour
{
	private GameObject theCollider;

	void OnTriggerEnter(Collider other)
	{
		theCollider = other.gameObject;
		if (theCollider == RigidbodyFirstPersonController.player.gameObject)
		{
			AudioManager.Instance.PlaySound("Lion");
		}
	}
}