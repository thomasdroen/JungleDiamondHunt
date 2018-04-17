using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : Menu
{
    public GameObject pauseMenuUI;
 

    //public void ShowUI(bool show)
    //{
    //    transform.GetChild(0).gameObject.SetActive(show);
    //}

    //public void Resume()
    //{
    //    pauseMenuUI.SetActive(false);
    //    Time.timeScale = 1;
    //    GameIsPaused = false;
    //    Cursor.lockState = CursorLockMode.Locked;
    //    Cursor.visible = false;
    //    musicChanger.Play();
    //}

    //void Pause()
    //{
    //    pauseMenuUI.SetActive(true);
    //    //Time.timeScale = 0f;
    //    //GameIsPaused = true;
    //    //Cursor.lockState = CursorLockMode.None;
    //    //Cursor.visible = true;
    //}

    public void loadMenu()
    {
        //GameIsPaused = false;
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
        //Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");

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


    public override void OpenMenu()
    {
        pauseMenuUI.SetActive(true);
    }

    public override void CloseMenu()
    {
        pauseMenuUI.SetActive(false);
    }
}
