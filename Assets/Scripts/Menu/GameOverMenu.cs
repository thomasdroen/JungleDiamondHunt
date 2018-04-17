using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;

public class GameOverMenu: Menu
{

    public float restartedDelay = 5f;

    Animator anim;
    float restartTimer;


    public override void OpenMenu()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        anim.SetTrigger("GameOver");
    }

    public override void CloseMenu()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
