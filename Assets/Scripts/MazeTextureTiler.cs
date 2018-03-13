using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MazeTextureTiler : MonoBehaviour
{
    [Range(0.1f, 2)]
    public float relativeScale = 1f;

    private void Start()
    {
        GetComponent<Renderer>().material.mainTextureScale = transform.lossyScale * relativeScale;
    }
}


