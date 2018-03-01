using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyRotation : MonoBehaviour {

    public Transform target;
	
	// Update is called once per frame
	void LateUpdate () {
        transform.rotation = Quaternion.Euler(0, 0, -target.rotation.eulerAngles.y);
	}
}
