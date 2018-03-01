using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class FovBySpeed
{
    public Camera camera;
    private float sprintTargetFov;
    public float maxFov = 120;
    public AnimationCurve FovToSpeedCurve = new AnimationCurve();

    private float originalFov;
    private float walkSpeed;
    private float sprintSpeed;
    private float lastSprintInput = 0;

    public void Setup(Camera cam, float walkSpd, float sprintSpd)
    {
        CheckStatus(cam);
        camera = cam;
        originalFov = cam.fieldOfView;
        sprintTargetFov = originalFov + 10;

        walkSpeed = walkSpd;
        sprintSpeed = sprintSpd;

        if (FovToSpeedCurve.length == 0)
        {
            UpdateFovValues(0);
        }
    }

    public void UpdateFovValues(float run)
    {

        float walkSpdSqrd = walkSpeed * walkSpeed;
        float sprintSpdSqrd = sprintSpeed * sprintSpeed;
        float sprintWalkDifference = sprintSpdSqrd - walkSpdSqrd;

        if (FovToSpeedCurve.length == 0)
        {
            Keyframe[] ks = new Keyframe[2];
            ks[0] = new Keyframe(sprintSpdSqrd + sprintWalkDifference / 10,
                (run > 0) ? sprintTargetFov / originalFov : 1/*, 0, Mathf.PI / (3 * 500)*/);
            ks[1] = new Keyframe(sprintSpdSqrd + sprintWalkDifference * 15, maxFov / originalFov);
            FovToSpeedCurve = new AnimationCurve(ks);
        }
        else
        {
            FovToSpeedCurve.MoveKey(0, new Keyframe((run > 0) ? sprintSpdSqrd + sprintWalkDifference / 10 : walkSpdSqrd + sprintWalkDifference / 10, (run > 0) ? sprintTargetFov / originalFov : 1));
        }

    }

    private void CheckStatus(Camera cam)
    {
        if (cam == null)
        {
            throw new Exception("Camera is null. Please provide a camera in the inspector.");
        }
    }

    public void UpdateFov(bool isGrounded, float currentSpeedSqr)
    {
        float run = Input.GetAxisRaw("Run");
        if (isGrounded)
        {
            if (run != lastSprintInput)
            {
                UpdateFovValues(run);
                lastSprintInput = run;
            }

            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView,
                originalFov * (run > 0 && currentSpeedSqr > walkSpeed * 3 ? sprintTargetFov / originalFov : 1), 0.25f);
        }
        else
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, originalFov * FovToSpeedCurve.Evaluate(currentSpeedSqr),
                0.25f);
        }
    }

}



