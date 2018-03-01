using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(AudioSource),typeof(CapsuleCollider))]
public class AnimalLookAtPlayer : MonoBehaviour {

    public AnimalPreset animalPreset;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

	public static bool animalIsActive = false;



    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (animalPreset != null)
        {
            spriteRenderer.sprite = animalPreset.image;
            audioSource.clip = animalPreset.sound;

            Debug.Log(animalPreset.name + " the " + animalPreset.species);
        }
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(Camera.main.transform.position, Vector3.up);

    }
	public void AnimalPressed(){
        AnimalUI.instance.openUI();	    
	}
}
