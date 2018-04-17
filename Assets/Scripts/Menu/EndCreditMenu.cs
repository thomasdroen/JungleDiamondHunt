using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;

public class EndCreditMenu : Menu {

	public GameObject endCreditUI;


	void Start () {
	}
	
	public override void OpenMenu(){
		endCreditUI.SetActive (true);
		GetComponent<Animator>().enabled = true;
		StartCoroutine (goToMenu (22));
	}

	public override void CloseMenu(){
		endCreditUI.SetActive (false);
	}

	IEnumerator goToMenu(float time){
		float startTime = Time.time;
		while (startTime + time > Time.time) {
			yield return null;
		}
		SceneManager.LoadScene ("MainMenu");
	}
}
