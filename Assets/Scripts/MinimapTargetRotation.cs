using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapTargetRotation : MonoBehaviour
{

    public Transform target;
    public bool turnMinimap = true;
    [Space]
    public RawImage minimap;
    public Image minimapBorder;
    public Image characterArrow;

    void Start()
    {
        if (target == null) { throw new Exception("Target not set. Please set target in inspector"); }
        if (minimap == null) { throw new Exception("Minimap not set. Please set minimap in inspector"); }
        if (minimapBorder == null) { throw new Exception("Minimap border not set. Please set minimap border in inspector"); }
        if (characterArrow == null) { throw new Exception("Character arrow not set. Please set character arrow in inspector"); }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (turnMinimap)
        {
            minimap.rectTransform.rotation = Quaternion.Euler(0, 0, target.rotation.eulerAngles.y);
            minimapBorder.rectTransform.rotation = Quaternion.Euler(0, 0, target.rotation.eulerAngles.y);
        }
        else
        {
            characterArrow.rectTransform.rotation = Quaternion.Euler(0, 0, -target.rotation.eulerAngles.y);
        }
    }
}
