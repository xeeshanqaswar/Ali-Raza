using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FlagQuestions : Questoin
{
    public Button resetButton;
    int optionNumber;



    private new void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        //Will be enable on client's requirement

        //refrenceManager.outlineSelection.isHover = true;
        //refrenceManager.outlineSelection.isHoverableQuestions = true;

        resetButton = refrenceManager.uIManager.resetPosButton;

    }
    
    public  void SetQuestionText(string lesson, string Quesiton, string type, string option1, string option2, string option3, string option4,List<string> highlightedPoints,List<int> correctOptions, bool ischange = false)
    {

        allowMultipleSelection = correctOptions.Count > 1;
        CurrentOptionHighlighter(correctOptions);
        lessonName.text = lesson;
        question.text = Quesiton;
        string QuestionType = refrenceManager.uIManager.AddSpaceBeforeCapitalLetters(type, ischange);
        questionTypeText.text = QuestionType;
        int num = 4;
        if (string.IsNullOrEmpty(option1))
        {
            Debug.Log("we are removing this");
            num--;
            animationController.trueFlase = true;
            options1.gameObject.SetActive(false);
        }
        else
        {
            options1.text = option1;
        }
        if (string.IsNullOrEmpty(option2))
        {
            Debug.Log("we are removing this");
            num--;
            animationController.trueFlase = true;
            options2.gameObject.SetActive(false);
        }
        else
        {
            options2.text = option2;
        }
        if (string.IsNullOrEmpty(option3))
        {
            Debug.Log("we are removing this");
            num--;
            animationController.trueFlase = true;
            options3.gameObject.SetActive(false);
        }
        else
        {
            options3.text = option3;

        }
        if (string.IsNullOrEmpty(option4))
        {
            Debug.Log("we are removing this");
            num--;
            animationController.trueFlase = true;
            options4.gameObject.SetActive(false);
        }
        else
        {
            options4.text = option4;
        }

        animationController.num = num;
        SetFlagRaise(highlightedPoints);
        
        
        //for (int i = 0; i < optionButtons.Length; i++)
        //{
        //    int temp = i;
        //    optionButtons[i].onClick.AddListener(() => SelectOption(optionButtons[temp], temp));
        //}


    }


    void SetFlagRaise(List<string> highlightedPoints)
    {
        refrenceManager.flagsHandler.SetFlagRaise(highlightedPoints);
    }

  


    /// <summary>
    /// options buttons listener
    /// </summary>
    /// <param name="selectedBtn"></param>
    /// <param name="optionNo"></param>
    //public void SelectOption(Button selectedBtn ,int optionNo)
    //{
    //    if (selectedButton)
    //    {
    //        selectedButton.GetComponent<Image>().sprite = unselectedOptionSprite;
    //    }
    //    selectedButton = selectedBtn;
    //    selectedButton.GetComponent<Image>().sprite = selectedOptionSprite;
    //    optionNumber = optionNo;
    //    refrenceManager.uIManager.questionSelectBtn.interactable = true;

    //}



    /// <summary>
    /// select button listner
    /// </summary>
    public void OnFlagSelectButtonListner()
    {
        
        RefrenceManager.instance.flagsHandler.UnraiseFlags();
        //List<int> imageans = new List<int>();
        //imageans.Add(optionNumber);
        refrenceManager.questionManager.NextQuestion(selectedOptionNumbers);

        refrenceManager.uIManager.SelectBtnBlocker(true);

        //refrenceManager.uIManager.questionSelectBtn.image.sprite = 
        //                                  refrenceManager.uIManager.selectBtnDisabledSprite;
        
        //refrenceManager.uIManager.selectBtnBlocker.enabled = true;
        
        EventsHandler.CallOnDisableOption();
        Destroy(this.gameObject);

    }

    private void OnDestroy()
    {
        
    }
}
