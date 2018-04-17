using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondManager : MonoBehaviour {

	public delegate void onTriggerEnter ();
	public event onTriggerEnter onEnter;
	public GameObject Player;
	// Use this for initialization
	void Start () {
		
	}
	void OnTriggerEnter(Collider other){
		if (other.gameObject == Player && onEnter != null) {
			onEnter ();
		}
	}
	
}
