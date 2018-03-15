using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(AudioSource),typeof(CapsuleCollider))]
public class GuardLookAtPlayer : MonoBehaviour {


	private AudioSource audioSource;
	private SpriteRenderer spriteRenderer;


	public GameObject guardUI;
	public static bool guardIsActive = false;

	public GameObject miniMap;

	private bool inMenu = false;


	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
		spriteRenderer = GetComponent<SpriteRenderer>();

	}

	// Update is called once per frame
	void Update () {
		transform.LookAt(Camera.main.transform.position, Vector3.up);
	

	}
	public void GuardPressed(){
		if (inMenu)
		{
			guardUI.SetActive(false);
			guardIsActive = false;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			Time.timeScale = 1f;
			miniMap.SetActive(true);

			inMenu = !inMenu;
		}
		else
		{
			guardUI.SetActive(true);
			Time.timeScale = 0f;
			guardIsActive = true;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			miniMap.SetActive(false);

			inMenu = !inMenu;
		}

	}
}
