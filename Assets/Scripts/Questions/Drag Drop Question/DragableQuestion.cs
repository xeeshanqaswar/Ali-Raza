using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DragableQuestion : MonoBehaviour
{
    public float distance;
    public int Optionnumber1;
    public int Optionnumber2;
    public int Optionnumber3;
    public int Optionnumber4;
    public int currentAnswer;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI questionTypeText;
    public RefrenceManager refrenceManager;
    int option;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void SetQuestionText(string lesson, string Quesiton, string type, bool isChange = false)
    {
        questionText.text = Quesiton;
        string qType = RefrenceManager.instance.uIManager.AddSpaceBeforeCapitalLetters(type, isChange);
        questionTypeText.text = qType;
    }

    public void OnSelectButtonListner()
    {

      //  refrenceManager.questionManager.NextQuestion();
        //refrenceManager.uIManager.questionSelectBtn.interactable = false;
     
        // Resting the Setting 
        bool a = RefrenceManager.instance.modelPartsHandler.VerifyAnswers();
        Debug.Log("Answer are"+a);
        List<int> answers = new List<int>();
        answers.Add(0);
        answers.Capacity = answers.Count;
        RefrenceManager.instance.questionManager.PlayCompleteSound();
        RefrenceManager.instance.questionManager.NextQuestion(answers, false, false, false, false,true,a);
        RefrenceManager.instance.modelPartsHandler.ResetToNormalState();
        ///CameraController.instance.ResetPositions();
        EventsHandler.CallOnDisableOption();
        Destroy(this.gameObject);


    }
}
