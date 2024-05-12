using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using System.Text;
using static QuestionManager;
using static EventsHandler;
using Unity.VisualScripting;
using System.Threading.Tasks;
using DG.Tweening;

public class Questoin : MonoBehaviour
{
    public YaxisAnimation animationController;
    public bool isFlagQuestion;
    public TextMeshProUGUI lessonName;
    public Image[] optionsImage;
    public TextMeshProUGUI question;
    public TextMeshProUGUI options1;
    public TextMeshProUGUI options2;
    public TextMeshProUGUI options3;
    public TextMeshProUGUI options4;
    public Button[] optionButtons;
    public string questionType1;
    public TextMeshProUGUI questionTypeText;
    private Button selectedButton; // Reference to the currently selected button
    public Color correctAnswerColor;
    protected RefrenceManager refrenceManager;


    public Sprite selectedOptionSprite, selectedOptionCircle, unselectedOptionCircle, unselectedOptionSprite;
    public Color selectedoptionTextColor, unselectedOptionTextColor;


    protected void Awake()
    {
        refrenceManager = RefrenceManager.instance;
    }

    private void Start()
    {
        // Disable the confirmation button initially
        //refrenceManager.uIManager.questionSelectBtn.interactable = false;
        //confirmButton.interactable = false;
        
/*        foreach (var button in optionButtons)
        {
            button.onClick.AddListener(() => OnOptionButtonClick(button));
        }*/
       // ResetPositionButton();
    }

    public List<int> selectedOptionNumbers;
    protected bool allowMultipleSelection;
    int count = 0;

    // Called when any option button is clicked
    public void OnOptionButtonClick(Button clickedButton)
    {

        SoundManager.manager.ButtonSound();

        int optionIndex = System.Array.IndexOf(optionButtons, clickedButton);
        
        //bool isSelected = clickedButton.GetComponent<Image>().color == selectedColor;
        
        bool isSelected = clickedButton.GetComponent<Image>().sprite == selectedOptionSprite;


        // If multiple selections are not allowed, clear previous selections
        if (!allowMultipleSelection && !isSelected)
        {
            DeselectAllOptions();
        }

        // Toggle the selection state
        SetSelectedButton(!isSelected, clickedButton, optionIndex);

        // Enable the confirmation button if at least one option is selected
        //refrenceManager.uIManager.questionSelectBtn.interactable = AnyOptionSelected();
        if (AnyOptionSelected())
        {
         
            refrenceManager.uIManager.SelectBtnBlocker(false);
        }
        else
        {
           
            refrenceManager.uIManager.SelectBtnBlocker(true);
        }
    }
    bool AnyOptionSelected()
    {
        return selectedOptionNumbers.Count > 0;
    }

    void SetSelectedButton(bool isSelected, Button button, int optionIndex)
    {
        //button.GetComponent<Image>().color = isSelected ? selectedColor : Color.white;
        
        
        button.GetComponent<Image>().sprite = isSelected ? selectedOptionSprite : unselectedOptionSprite;
        
        if (!isFlagQuestion)
        {
            button.transform.GetChild(1).GetComponent<Image>().sprite = 
                                                  isSelected ? selectedOptionCircle : unselectedOptionCircle;
        
            button.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = 
                                    isSelected ? selectedoptionTextColor : unselectedOptionTextColor;

        }

        if (isSelected)
        {
            // Add the selected option index to the list
            selectedOptionNumbers.Add(optionIndex);
        }
        else
        {
            // Remove the deselected option index from the list
            selectedOptionNumbers.Remove(optionIndex);
        }
    }

    void DeselectAllOptions()
    {
        foreach (var button in optionButtons)
        {
            SetSelectedButton(false, button, System.Array.IndexOf(optionButtons, button));
        }
    }
  
    public void OnSelectButtonListner()
    {
        //Debug.Log(selectedOptionNumbers.Count);
        refrenceManager.questionManager.NextQuestion(selectedOptionNumbers);
        //refrenceManager.uIManager.questionSelectBtn.interactable = false;
        refrenceManager.uIManager.SelectBtnBlocker(true);
        ///CameraController.instance.ResetPositions();
        EventsHandler.CallOnDisableOption();
        Destroy(this.gameObject);

    }

    public void CurrentOptionHighlighter(List<int> number)
    {

        /*for (int i = 0; i < number.Count; i++)
        {

            optionsImage[number[i]].color = correctAnswerColor;
        }*/
    }

    public void ResetPositionButton()
    {
         CameraController.ResettingCamera?.Invoke();
    }

    public virtual void  SetQuestionText(string lesson, string Quesiton, string a, string b , string c, string d, 
                                string questionType, List<int> correctanswer, bool isChange = false)
    {
        allowMultipleSelection = correctanswer.Count > 1;
        CurrentOptionHighlighter(correctanswer);
        lessonName.text = lesson;
        questionType1 = questionType;
        string qTYpe = refrenceManager.uIManager.AddSpaceBeforeCapitalLetters(questionType.ToString(), isChange);
        questionTypeText.text = qTYpe;  //adding question type field in the question popup
        question.text = Quesiton;
        string[] optionsArray = new string[] { a, b, c, d };
        TextMeshProUGUI[] optionsTextArray = new TextMeshProUGUI[] { options1, options2, options3, options4 };
        int num = 4;
        for (int i = 0; i < optionsArray.Length; i++)
        {
            
            if (string.IsNullOrEmpty(optionsArray[i]))
            {
                num--;
                animationController.trueFlase = true;
                optionButtons[i].gameObject.SetActive(false);
            }
            else
            {
                optionsTextArray[i].text = optionsArray[i];
            }
        }
        animationController.num=num;

    }

    private void OnDestroy()
    {
        
    }

}
