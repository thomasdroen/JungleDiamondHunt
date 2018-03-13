using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(Sprite))]
public class Collectible : MonoBehaviour {

    public GameObject player;

    private Sprite foodSprite;

    private void Start()
    {
        if(player == null)
        {
            player = Camera.main.transform.parent.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(gameObject.activeSelf && other.gameObject == player)
        {
            AnimalUI.instance.collectCollectible();
            gameObject.SetActive(false);
        }
    }

    public void setSprite(Sprite sprite)
    {
        foodSprite = sprite;
    }
}
