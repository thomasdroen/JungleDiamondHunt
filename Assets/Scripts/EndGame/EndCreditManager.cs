using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;

public class EndCreditManager : MonoBehaviour {

	public MusicChanger musicChanger; 
	public GameOverManager gameOver;
	public GameObject endCreditUI;
	RigidbodyFirstPersonController player;


	void Start () {
		player = RigidbodyFirstPersonController.player;

	}
	
	// Update is called once per frame
	void Update () {

		
	}
}
