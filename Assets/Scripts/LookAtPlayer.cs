using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour {

    public Camera cam;

    // Update is called once per frame
    void Update()
    {
        Vector3 toCam = cam.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(new Vector3(toCam.x, 0, toCam.z));
    }
}
