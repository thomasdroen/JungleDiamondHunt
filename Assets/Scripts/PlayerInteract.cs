using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

	private int layerMask = 1 << 8;
    private Camera cam;

    public float sphereCastRadius = 0.1f;
    public float sphereCastLength = 2;

	// Use this for initialization
	void Start ()
	{
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.E)) {
			Interact ();
		}
		
	}

	public void Interact ()
	{
	    Debug.DrawRay(cam.transform.position, cam.transform.forward * sphereCastLength, Color.red, 1);
	    Debug.DrawRay(cam.transform.position + cam.transform.up * sphereCastRadius, cam.transform.forward * sphereCastLength, Color.red, 1);
	    Debug.DrawRay(cam.transform.position - cam.transform.up * sphereCastRadius, cam.transform.forward * sphereCastLength, Color.red, 1);

        RaycastHit rHit;
		
		
        if (Physics.SphereCast(cam.transform.position, sphereCastRadius, cam.transform.forward, out rHit, sphereCastLength, layerMask)) {
            AnimalScript animal = rHit.collider.gameObject.GetComponent<AnimalScript>();
            

            if (animal)
            {
                animal.AnimalPressed();
                return;
            }
            TransitionStart start = rHit.collider.gameObject.GetComponent<TransitionStart>();
            if (start)
            {
                start.onPlayerInteract(gameObject);
                return;
            }

            var puzzle = rHit.collider.transform.parent.GetComponent<PuzzleUI>();
            if (puzzle)
            {
                GetComponent<RigidbodyFirstPersonController>().enabled = false;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                puzzle.OpenPuzzleUi();
            }
		}
	}
}

