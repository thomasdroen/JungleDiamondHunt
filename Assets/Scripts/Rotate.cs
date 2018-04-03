using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    public float degreesPerSecond = 10;
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up, degreesPerSecond * Time.deltaTime);
	}
}
