using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	public static bool GameIsPaused = false; 
	public GameObject pauseMenuUI;
	public GameObject musicChanger;

    void Start()
    {
        Resume();
    }

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (GameIsPaused) {
				Resume ();
			} else {
				Pause ();
			}
		
		}
	}
		public void Resume()
		{
		pauseMenuUI.SetActive (false);
		Time.timeScale = 1;
		GameIsPaused = false;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		musicChanger.SetActive (true);
		}
		void Pause()
		{
		pauseMenuUI.SetActive (true);
		Time.timeScale = 0f;
		GameIsPaused = true;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		musicChanger.SetActive (false);
		}

	public void loadMenu()
	{
		GameIsPaused = false;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Time.timeScale = 1f;

		SceneManager.LoadScene ("MainMenu");

	}
	public void quitGame()
	{
		#if UNITY_EDITOR
		// Application.Quit() does not work in the editor so
		// UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
