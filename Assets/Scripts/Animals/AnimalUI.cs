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
    public Text continueText;
    [Space]
    public List<Quest> quests;
    [Space]
    public Text collectibleCounterText;
    public CollectibleSpawns collectibleSpawns;
    [Space]
    public GameObject minimap;
    [HideInInspector]
    private int collectibleCounter;
    private int collectiblesNeeded;

    private Quest activeQuest;
    private bool inMenu = false;

    private RectTransform animalTextUI;
    private float originalUIWidth;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            collectibleCounter = 0;
        }
    }

    void Start()
    {
        hideResultUI();
        collectibleCounterText.gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(false);
        animalTextUI = QuestDescription.transform.parent.GetComponent<RectTransform>();
        originalUIWidth = animalTextUI.sizeDelta.x;
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

            if (activeQuest == null)
            {
                SelectRandomQuest();
            }
            else
            {
                updateQuest();
            }

            inMenu = !inMenu;
        }
    }

    private void updateQuest()
    {
        if (!isQuestion(activeQuest))
        {
            CollectionQuest colQuest = (CollectionQuest)activeQuest;
            if (collectibleCounter >= collectiblesNeeded)
            {
                Debug.Log(colQuest.finishWithQuestText);
                QuestDescription.text = colQuest.finishWithQuestText;
                collectibleCounterText.gameObject.SetActive(false);
                finishQuest();
            }
            else
            {
                string notEnoughString = colQuest.notEnough.Replace("$", "" + collectibleCounter);
                QuestDescription.text = notEnoughString;
            }
        }
    }

    public void LoadQuest(Quest quest)
    {
        Debug.Log(numberOfQuestsNeeded);
        if(numberOfQuestsNeeded <= 0)
        {
            dontNeedMore();
            return;
        }

        activeQuest = quest;

        hideResultUI();

        QuestDescription.text = quest.questDescription;

        if (isQuestion(quest))
        {
            Question question = (Question)quest;

            new System.Random().Shuffle(question.answers);

            toggleButtons(true, true);

            animalTextUI.sizeDelta = new Vector2(originalUIWidth, animalTextUI.sizeDelta.y);

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

            buttonGroup.spacing = 3 + (buttonHeight * (buttons.Length - question.answers.Length)) / 3;

        }
        else
        {
            CollectionQuest colQuest = (CollectionQuest)quest;
            collectibleCounter = 0;
            collectiblesNeeded = colQuest.numberOfCollectibles;
            updateCollectibleUI();
            collectibleSpawns.spawnCollectibles(colQuest.numberOfCollectibles);
            toggleButtons(false, true);
        }
    }

    private void dontNeedMore()
    {
        toggleButtons(false, true);
        hideResultUI();
        QuestDescription.gameObject.SetActive(true);
        QuestDescription.text = "I have nothing for you to do.";
    }

    public void SelectRandomQuest()
    {
        if (quests.Count > 0)
        {
            LoadQuest(quests[Mathf.RoundToInt(UnityEngine.Random.value * (quests.Count - 1))]);
        }
        else
        {
            dontNeedMore();
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
            toggleButtons(false, false);
            Question question = (Question)activeQuest;
            //extraInfo.text = question.answers[index].extraInfo;
            QuestDescription.text = question.answers[index].extraInfo;
            continueText.transform.parent.gameObject.SetActive(true);
            if (question.answers[index].correct)
            {
                correctImage.gameObject.SetActive(true);
                continueText.text = "Continue";
                finishQuest();
            }
            else
            {
                wrongImage.gameObject.SetActive(true);
                continueText.text = "Try again";
                activeQuest = null;
            }
            
        }
    }

    public void pressContinue()
    {
        if (activeQuest == null && numberOfQuestsNeeded > 0)
        {
            SelectRandomQuest();
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

    void toggleButtons(bool show, bool panel)
    {
        buttons[0].transform.parent.gameObject.SetActive(show);
        
        if(panel)
        {
            buttons[0].transform.parent.parent.gameObject.SetActive(show);
            animalTextUI.sizeDelta = new Vector2(800, animalTextUI.sizeDelta.y);
        }

    }

    void hideResultUI()
    {
        wrongImage.gameObject.SetActive(false);
        correctImage.gameObject.SetActive(false);
        continueText.transform.parent.gameObject.SetActive(false);
    }

    public void collectCollectible()
    {
        collectibleCounter++;
        updateCollectibleUI();
    }

    private void updateCollectibleUI()
    {
        collectibleCounterText.gameObject.SetActive(true);
        collectibleCounterText.text = "" + collectibleCounter + "/" + collectiblesNeeded;
    }

    private void finishQuest()
    {
        if(activeQuest != null)
        {
            numberOfQuestsNeeded--;
            quests.Remove(activeQuest);
            activeQuest = null;
        }
    }

}
