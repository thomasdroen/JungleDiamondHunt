using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;

public class EndCreditManager : MonoBehaviour {

	public MusicChanger musicChanger; 
	public GameOverManager gameOver;
	public GameObject endCreditUI;

	void Awake () {
	}
	
	// Update is called once per frame
	void Update () {
		if (gameOver.checkIfGameIsOver.Equals(true)) {
			endCreditUI.SetActive(true);
		}
		
	}
}
