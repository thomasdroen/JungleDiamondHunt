﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class SlowdownField : MonoBehaviour
{

    //public GameObject player;
    [Range(0.05f,0.95f)]
    public float amount = 0.5f;
    private RigidbodyFirstPersonController PlayerController;

    private void Start()
    {
        PlayerController = RigidbodyFirstPersonController.player;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.activeSelf && other.gameObject == PlayerController.gameObject)
        {
            PlayerController.movementSettings.WaterMultiplier = amount;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (gameObject.activeSelf && other.gameObject == PlayerController.gameObject)
        {
            PlayerController.movementSettings.WaterMultiplier = 1.0f;
        }
    }
}