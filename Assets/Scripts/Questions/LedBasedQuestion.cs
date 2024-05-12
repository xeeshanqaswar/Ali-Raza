using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LedBasedQuestion : MonoBehaviour
{

    public TextMeshProUGUI lessonNameText;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI questionTypeText;
    public TextMeshProUGUI option1;
    public TextMeshProUGUI option2;
    public TextMeshProUGUI option3;
    public TextMeshProUGUI option4;
    public Button[] optionButtons;
    [SerializeField] RefrenceManager refrenceManager;
    public Color correctAnserColor;
    Button selectedButton;

    public Sprite selectedOptionSprite, selectedOptionCircle, unselectedOptionCircle, unselectedOptionSprite;
    public Color selectedOptionTextColor, unselectedOptionTextColor;

    private void Awake()
    {
        refrenceManager = RefrenceManager.instance;
    }

    private void Start()
    {
        foreach (var button in optionButtons)
        {
            button.onClick.AddListener(() => OnOptionButtonClick(button));
        }
    }

    public void SetQuestionLed(string lesson, string Quesiton, string a, string b, string c, string d, 
                               string questionType, int correctOption)
    {
        CurrentOptionHighlighter(correctOption);
        questionText.text = Quesiton;
        lessonNameText.text = lesson;
        //string qTYpe = refrenceManager.uIManager.AddSpaceBeforeCapitalLetters(questionType, true);
        questionTypeText.text = questionType;
        //questionTypeText.text = questionname;

        string[] optionsArray = new string[] { a, b, c, d };
        TextMeshProUGUI[] optionsTextArray = new TextMeshProUGUI[] { option1, option2, option3, option4 };

        for (int i = 0; i < optionsArray.Length; i++)
        {
            if (string.IsNullOrEmpty(optionsArray[i]))
            {
                optionButtons[i].gameObject.SetActive(false);


            }
            else
            {
                optionsTextArray[i].text = optionsArray[i];
            }
        }

    }

    public async void OnSelectButtonListner()
    {
        
       await refrenceManager.ledRefrence.ChangeToDefaultColor();
        RefrenceManager.instance.questionManager.ledRefrence.StopAllFlashing();
        List<int> options = new List<int>();
        options.Add(optionNumber);
        refrenceManager.questionManager.NextQuestion(options);
        ///CameraController.instance.ResetPositions();
        ///
        RefrenceManager.instance.locateTypeQuestions.ReasignDefaultTagsObjects();
        RefrenceManager.instance.locateTypeQuestions.HighlitObjectsSetToOff();
        //refrenceManager.uIManager.questionSelectBtn.interactable = false;
        refrenceManager.uIManager.SelectBtnBlocker(true);
        EventsHandler.CallOnDisableOption();
        options.Clear();
        RefrenceManager.instance.taskCompleted = true;

        Destroy(this.gameObject);

    }

    public void ResetPositions()
    {
       
        CameraController.ResettingCamera?.Invoke();
    }

    int optionNumber;

    public void OnOptionButtonClick(Button clickedButton)
    {

        SoundManager.manager.ButtonSound();

        // Deselect the previously selected button
        if (selectedButton != null)
        {
            selectedButton.GetComponent<Image>().sprite = unselectedOptionSprite;
            selectedButton.transform.GetChild(1).GetComponent<Image>().sprite = unselectedOptionCircle;
            selectedButton.transform.GetChild(1).transform.GetChild(0).
                                         GetComponent<TextMeshProUGUI>().color = unselectedOptionTextColor;

        }


        optionNumber = Array.IndexOf(optionButtons, clickedButton);

        // Select the new button
        selectedButton = clickedButton;

        selectedButton.GetComponent<Image>().sprite = selectedOptionSprite;

        selectedButton.transform.GetChild(1).GetComponent<Image>().sprite = selectedOptionCircle;

        //change text color of option circle
        selectedButton.transform.GetChild(1).transform.GetChild(0).
                                            GetComponent<TextMeshProUGUI>().color = selectedOptionTextColor;


        //Enable the confirmation button
        //refrenceManager.uIManager.questionSelectBtn.interactable = true;
        refrenceManager.uIManager.SelectBtnBlocker(false);
    }

    public void CurrentOptionHighlighter(int number)
    {
        //optionButtons[number].GetComponent<Image>().color = correctAnserColor;
    }

    private void OnDestroy()
    {
        
    }

   

}
