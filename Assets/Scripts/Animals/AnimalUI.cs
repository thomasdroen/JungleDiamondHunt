using System.Collections.Generic;
using Assets.Scripts.Animals.Quests;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Animals
{
    public class AnimalUI : global::Menu
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
        public Button backButton;
        [Space]
        public List<Quest> quests;
        [Space]
        public Text collectibleCounterText;
        public CollectibleSpawns collectibleSpawns;
        [Space]
        public GameObject minimap;

        public GateScript gate;
        [HideInInspector]
        private int collectibleCounter;
        private int collectiblesNeeded;

        private Quest activeQuest;
        private bool inMenu = false;

        private RectTransform questDescriptionPanel;
        private float originalUIWidth;

        protected override void Awake()
        {
            base.Awake();
            if (instance == null)
            {
                instance = this;
                collectibleCounter = 0;
            }
        }

        private void Start()
        {
            questDescriptionPanel = QuestDescription.transform.parent.GetComponent<RectTransform>();
            originalUIWidth = questDescriptionPanel.sizeDelta.x;
        }

        public override void OpenMenu()
        {
            transform.GetChild(0).gameObject.SetActive(true);
            if (activeQuest == null)
            {
                SelectRandomQuest();
            }
            else
            {
                updateQuest();
            }
        }

        public override void CloseMenu()
        {
            hideResultUI();
            transform.GetChild(0).gameObject.SetActive(false);
        
            //originalUIWidth = animalTextUI.sizeDelta.x;
        }

    


        //public void openUI()
        //{
        //    if (inMenu)
        //    {
        //        transform.GetChild(0).gameObject.SetActive(false);
        //        Cursor.lockState = CursorLockMode.Locked;
        //        Cursor.visible = false;
        //        Time.timeScale = 1f;
        //        minimap.SetActive(true);

        //        inMenu = !inMenu;
        //    }
        //    else
        //    {
        //        Time.timeScale = 0f;
        //        Cursor.lockState = CursorLockMode.None;
        //        Cursor.visible = true;
        //        minimap.SetActive(false);



        //        inMenu = !inMenu;
        //    }
        //}

        private void updateQuest()
        {
            backButton.gameObject.SetActive(true);
            if (!isQuestion(activeQuest))
            {

                CollectionQuest colQuest = (CollectionQuest) activeQuest;
                if (collectibleCounter >= collectiblesNeeded)
                {
                    Debug.Log(colQuest.finishWithQuestText);
                    QuestDescription.text = colQuest.finishWithQuestText;
                    collectibleCounterText.gameObject.SetActive(false);
                    continueText.transform.parent.gameObject.SetActive(true);
                    backButton.gameObject.SetActive(false);
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
            backButton.gameObject.SetActive(true);
            if (numberOfQuestsNeeded <= 0)
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

                questDescriptionPanel.sizeDelta = new Vector2(originalUIWidth, questDescriptionPanel.sizeDelta.y);

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
            QuestDescription.text = "You have completed all my quests. The tower is open.";
            gate.toggleGate(true);
            backButton.gameObject.SetActive(true);
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
            SelectRandomQuest();
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
                questDescriptionPanel.sizeDelta = new Vector2(800, questDescriptionPanel.sizeDelta.y);
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
}
