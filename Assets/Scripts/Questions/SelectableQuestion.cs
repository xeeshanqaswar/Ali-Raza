using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectableQuestion : MonoBehaviour
{
    #region variables

    public float distance;
    public int Optionnumber1;
    public int Optionnumber2;
    public int Optionnumber3;
    public int Optionnumber4;
    public int currentAnswer;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI questionTypeText;
    public  RefrenceManager refrenceManager;
    int option;

    #endregion

    private void Awake()
    {
        refrenceManager = RefrenceManager.instance;
    }
   
    /// <summary>
    /// set questions and lesson name
    /// </summary>
    /// <param name="lesson"></param>
    /// <param name="Quesiton"></param>
    public void SetQuestionText(string lesson, string Quesiton, string type, bool isChange = false)
    {
        questionText.text = Quesiton;
        string qType = RefrenceManager.instance.uIManager.AddSpaceBeforeCapitalLetters(type, isChange);
        questionTypeText.text = qType;
    }

    [HideInInspector] public bool isSelected = false;

    /// <summary>
    /// option button listener
    /// </summary>
    /// <param name="answer"></param>
    public void OnOptionSelected(int answer)
    {
        SoundManager.manager.ButtonSound();
        refrenceManager.uIManager.SelectBtnBlocker(false);
        //refrenceManager.uIManager.questionSelectBtn.interactable = true;
        option = answer;
    }
  
    /// <summary>
    /// reset position button listener
    /// </summary>
    public void ResetPosition()
    {
        CameraController.ResettingCamera?.Invoke();
    }

    /// <summary>
    /// select button listner
    /// </summary>
    public void OnSelectButtonListner()
    {
        isSelected = true;
        List<int> answers = new List<int>();
        answers.Add(option);
        refrenceManager.locateTypeQuestions.ReasignDefaultTagsObjects();
        refrenceManager.locateTypeQuestions.HighlitObjectsSetToOff();
        refrenceManager.questionManager.PlayCompleteSound();
        refrenceManager.questionManager.NextQuestion(answers, true);
        //refrenceManager.cameraController.CameraComponentHandler(true);
        
        //refrenceManager.uIManager.questionSelectBtn.interactable = false;
        refrenceManager.uIManager.SelectBtnBlocker(true);

        EventsHandler.CallOnDisableOption();
        Destroy(this.gameObject);

    }
    /// <summary>
    /// override for select button
    /// </summary>
    /// <param name="answer"></param>
    public virtual void OnSelectButtonListner(int answer)
    {
        
        List<int> answers = new List<int>();
        answers.Add(answer);
        refrenceManager.locateTypeQuestions.ReasignDefaultTagsObjects();
        refrenceManager.locateTypeQuestions.HighlitObjectsSetToOff();
        refrenceManager.questionManager.PlayCompleteSound();
        refrenceManager.questionManager.NextQuestion(answers,true);
        //refrenceManager.cameraController.CameraComponentHandler(true);
        EventsHandler.CallOnDisableOption();
        Destroy(this.gameObject);

    }


    private void OnDestroy()
    {
        //ResetPosition();
        //Destroy(refrenceManager);
    }

}