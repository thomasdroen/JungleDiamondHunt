using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour {

    public Camera cam;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cam.transform.position, Vector3.up);

    }
}
