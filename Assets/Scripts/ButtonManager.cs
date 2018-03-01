using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonManager : MonoBehaviour {

	public void playBtn(string newScene)
	{
		SceneManager.LoadScene (newScene);
		if (newScene.Equals("MainMenu")) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	public void quitGameBtn()
	{
		#if UNITY_EDITOR
		// Application.Quit() does not work in the editor so
		// UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}

	public void genderSelect()
	{
		SceneManager.LoadScene("Level 1");
	}
}