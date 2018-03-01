using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AnimalUI : MonoBehaviour
{


    public static AnimalUI instance;

    public int numberOfQuestsNeeded;
    [Space]
    public Text QuestDescription;
    [Space]
    public Button[] buttons;
    public VerticalLayoutGroup buttonGroup;
    [Space]
    public Image correctImage;
    public Image wrongImage;
    public Text extraInfo;
    public Text continueText;
    [Space]
    public Quest[] quests;
    [Space]
    public GameObject minimap;

    private Quest activeQuest;
    private bool inMenu = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        hideResultUI();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void openUI()
    {
        if (inMenu)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
            minimap.SetActive(true);

            inMenu = !inMenu;
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            minimap.SetActive(false);

            selectRandomQuest();

            inMenu = !inMenu;
        }
    }

    public void LoadQuest(Quest quest)
    {
        if(numberOfQuestsNeeded <= 0)
        {
            dontNeedMore();
            return;
        }

        activeQuest = quest;

        hideResultUI();

        if (isQuestion(quest))
        {
            Debug.Log("question loaded");
            Question question = (Question)quest;
            QuestDescription.text = question.questDescription;

            Shuffle(new System.Random(), question.answers);

            int i = 0;
            foreach (Button button in buttons)
            {
                if (i < question.answers.Length)
                {
                    buttons[i].gameObject.SetActive(true);
                    buttons[i].GetComponentInChildren<Text>().text = question.answers[i].answerText;
                }
                else
                {
                    buttons[i].gameObject.SetActive(false);
                }
                i++;
            }
            RectTransform buttonRect = buttons[0].GetComponent<RectTransform>();
            float buttonHeight = buttonRect.sizeDelta.y;

            buttonGroup.spacing = (buttonHeight * (buttons.Length - question.answers.Length)) / 3;

        }
    }

    private void dontNeedMore()
    {
        hideButtons();
        hideResultUI();
        QuestDescription.gameObject.SetActive(true);
        QuestDescription.text = "I have nothing for you to do.";
        continueText.transform.parent.gameObject.SetActive(true);
        continueText.text = "Leave";
    }

    public void selectRandomQuest()
    {
        if (quests.Length > 0)
        {
            LoadQuest(quests[Mathf.RoundToInt(UnityEngine.Random.value * (quests.Length - 1))]);
        }
        else
        {
            Debug.LogError("No quests added!");
        }
    }

    private void Shuffle<T>(System.Random rng, T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }

    public void AnswerQuestion(int index)
    {
        if(activeQuest == null)
        {
            Debug.LogError("Active quest is null!");
            return;
        }
        if (isQuestion(activeQuest))
        {
            hideButtons();
            Question question = (Question)activeQuest;
            extraInfo.gameObject.SetActive(true);
            extraInfo.text = question.answers[index].extraInfo;
            continueText.transform.parent.gameObject.SetActive(true);
            if (question.answers[index].correct)
            {
                correctImage.gameObject.SetActive(true);
                continueText.text = "Continue";
                numberOfQuestsNeeded--;
            }
            else
            {
                wrongImage.gameObject.SetActive(true);
                continueText.text = "Try again";
            }
            
        }
    }

    public void pressContinue()
    {
        if (numberOfQuestsNeeded > 0)
        {
            selectRandomQuest();
        }
        else
        {
            openUI();
        }
    }

    bool isQuestion(Quest quest)
    {
        return quest.GetType() == typeof(Question);
    }

    void hideButtons()
    {
        foreach(Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }
    }

    void hideResultUI()
    {
        wrongImage.gameObject.SetActive(false);
        correctImage.gameObject.SetActive(false);
        continueText.transform.parent.gameObject.SetActive(false);
        extraInfo.gameObject.SetActive(false);
    }

    

}
