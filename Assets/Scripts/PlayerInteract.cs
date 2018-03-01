using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

	private int layerMask = 1 << 8;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyUp (KeyCode.E)) {
			Interact ();
		}
		
	}

	public void Interact ()
	{
		RaycastHit rHit;
		Physics.Raycast (transform.position, transform.forward, out rHit, 3f, layerMask);
		Debug.DrawRay (transform.position, transform.forward * 3, Color.red, 1);
		if (rHit.collider != null) {
			rHit.collider.gameObject.GetComponent<AnimalLookAtPlayer> ().AnimalPressed ();
		}
	}
}
