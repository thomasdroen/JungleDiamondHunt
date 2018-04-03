using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleActivator : MonoBehaviour
{
    public GameObject puzzleUI;

    public Transform cameraToTransform;
    public Transform cameraFromTransform;

    private Vector3 originalCamPos;
    private Quaternion originalCamRot;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    public void startPuzzleUI()
    {
        originalCamPos = cam.gameObject.transform.position;
        originalCamRot = cam.gameObject.transform.rotation;
        StartCoroutine(translateCameraToTarget(0.75f));
    }

    IEnumerator translateCameraToTarget(float time)
    {
        float startTime = Time.time;
        Vector3 velocity = Vector3.zero;
        while (Time.time < startTime + time)
        {
            float interpolateValue = (Time.time - startTime) / time;
            float smoothedInterpolatedValue = Mathf.Atan(interpolateValue * 3.43f - 0.9f) * 0.52f + 0.38f;
            cam.transform.position = Vector3.Lerp(originalCamPos, cameraToTransform.position, smoothedInterpolatedValue);
            cam.transform.rotation = Quaternion.Lerp(originalCamRot, cameraToTransform.rotation, smoothedInterpolatedValue);
            yield return null;
        }
        cam.transform.position = cameraToTransform.position;
        cam.transform.rotation = cameraToTransform.rotation;
        Debug.Log("finished");
    }

}
