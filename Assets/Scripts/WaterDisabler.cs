using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityStandardAssets.Water;

public class WaterDisabler : MonoBehaviour
{

    public Camera mainCam;
    public Camera minimapCam;

    private MeshRenderer waterMesh;
    private OldWater water;
    private int TerrainLayerMask = 1 << 9;

    public bool useLookDirection = true; // checks if the main camera looks at the water and disables it. not useful for large bodies of water.

    public float lookAngle = 100;
    public float radius;
    public float reflectiveThreshold = 20;

    void Start()
    {
        waterMesh = GetComponent<MeshRenderer>();
        water = GetComponent<OldWater>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector2 thisPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 minimapPos = new Vector2(minimapCam.gameObject.transform.position.x, minimapCam.gameObject.transform.position.z);

        float dist = Vector2.Distance(thisPos, minimapPos);

        if (dist < radius)
        {
            waterMesh.enabled = true;
            return;
        }

        if (useLookDirection)
        {
            Vector3 fromCamToWater = (transform.position - mainCam.gameObject.transform.position).normalized;
            if (Vector3.Angle(mainCam.gameObject.transform.forward, fromCamToWater) > lookAngle)
            {
                waterMesh.enabled = false;
                return;
            }
        }

        waterMesh.enabled = transform.position.y <= mainCam.gameObject.transform.position.y;
        if (!waterMesh.enabled)
        {
            return;
        }

        if (transform.GetComponentsInChildren<Transform>().Length > 0)
        {
            

            foreach (Transform location in transform.GetComponentsInChildren<Transform>())
            {
                Vector3 fromMainCamToThis = location.position - mainCam.gameObject.transform.position;
                float distance = fromMainCamToThis.magnitude;
                Ray ray = new Ray(mainCam.gameObject.transform.position, fromMainCamToThis);
                RaycastHit rHit;
                
                if (!Physics.Raycast(ray, out rHit, distance, TerrainLayerMask))
                {
                    waterMesh.enabled = true;
                    return;
                }
            }
            waterMesh.enabled = false;
        }

    }
}
