using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

	private int layerMask = 1 << 8;
    private Camera cam;

	// Use this for initialization
	void Start ()
	{
        cam = Camera.main;
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
		Physics.SphereCast (transform.position, 0.25f, cam.transform.forward, out rHit, 3, layerMask);
		Debug.DrawRay (transform.position, cam.transform.forward * 3, Color.red, 1);
        Debug.DrawRay (transform.position+cam.transform.up*0.25f, cam.transform.forward * 3, Color.red, 1);
        Debug.DrawRay (transform.position-cam.transform.up*0.25f, cam.transform.forward * 3, Color.red, 1);
        if (rHit.collider) {
            AnimalLookAtPlayer animal = rHit.collider.gameObject.GetComponent<AnimalLookAtPlayer>();
            TransitionStart start = rHit.collider.gameObject.GetComponent<TransitionStart>();

            if (animal)
            {
                animal.AnimalPressed();
                return;
            }

            else if (rHit.collider.gameObject.GetComponent<TransitionStart>())
            {
                start.onPlayerInteract(gameObject);
            }
		}
	}
}
