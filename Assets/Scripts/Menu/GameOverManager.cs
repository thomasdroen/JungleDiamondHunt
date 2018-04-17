using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;

public class GameOverManager : MonoBehaviour
{

    public float restartedDelay = 5f;
    public DiamondManager diamond;
    RigidbodyFirstPersonController player;

    public MusicChanger musicChanger;

    Animator anim;
    float restartTimer;

    void Start()
    {
        player = RigidbodyFirstPersonController.player;

    }

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        diamond.onEnter += gameOver;
    }

    void OnDisable()
    {

        diamond.onEnter -= gameOver;

    }


    public void gameOver()
    {


        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        musicChanger.EndGame();
        anim.SetTrigger("GameOver");
        player.enabled = false;
        //Time.timeScale = 0f;



        /*restartTimer += Time.deltaTime;

		if(restartTimer >= restartedDelay){
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}*/
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
