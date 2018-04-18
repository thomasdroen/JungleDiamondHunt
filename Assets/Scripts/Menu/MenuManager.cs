using System;
using System.Collections.Generic;
using System.Collections;
using Assets.Scripts.Puzzle;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Assets.Scripts.Menu
{
    public class MenuManager : MonoBehaviour
    {

        public static MenuManager Instance;

        public global::Menu[] menus;
        private Stack<global::Menu> activeMenues;
        [Space] public GameObject[] objectsToHide;

        [HideInInspector]
        public bool isGameFinished = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            activeMenues = new Stack<global::Menu>();
            pauseGame(false);
        }

        //private void Start()
        //{
        //    foreach (var menu in menus)
        //    {
        //        menu.CloseMenu();
        //    }
        //}

        private void Update()
        {
            if (CrossPlatformInputManager.GetButtonDown("Cancel") && !isGameFinished)
            {
                if (activeMenues.Count < 1)
                {
                    OpenMenu("Pause");
                }
                else
                {
                    CloseMenu();
                }
            }
        }

        public void OpenMenu(string menuName)
        {
            global::Menu correctMenu = Array.Find(menus, menu => string.Equals(menu.menuName, menuName, StringComparison.CurrentCultureIgnoreCase));
            if (correctMenu != null)
            {
                if (activeMenues.Count > 0)
                {
                    activeMenues.Peek().CloseMenu();
                }
                else
                {
                    pauseGame(true);
                }
                activeMenues.Push(correctMenu);
                correctMenu.OpenMenu();
            }
            else
            {
                Debug.LogWarning(menuName + " menu not found!");
            }
        }

        public void CloseMenu()
        {
            if (activeMenues.Count > 0)
            {
                activeMenues.Pop().CloseMenu();
                if (activeMenues.Count > 0)
                {
                    activeMenues.Peek().OpenMenu();
                }
                else
                {
                    pauseGame(false);
                }
            }
        }

        private void pauseGame(bool pause)
        {
            if (pause)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = pause;
                ToggleObjectsToHide(!pause);
            }
            else
            {
                Time.timeScale = 1;
                if (!PuzzleUI.Instance.UIOpened)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = pause;
                    ToggleObjectsToHide(!pause);
                }
            }


        }

        public void FinishGame()
        {
            isGameFinished = true;
            OpenMenu("Victory");
            Time.timeScale = 1;
            StartCoroutine(showCredits(4));

        }

        private void ToggleObjectsToHide(bool show)
        {
            foreach (var o in objectsToHide)
            {
                o.SetActive(show);
            }
        }


        public IEnumerator showCredits(float time)
        {
            float startTime = Time.time;
            while (startTime + time > Time.time)
            {
                yield return null;
            }
            OpenMenu("EndCredit");
        }
    }
}

