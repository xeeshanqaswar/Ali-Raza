using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataUploadFirebase : MonoBehaviour
{

    [SerializeField] private TMP_InputField lessonNumIF;
    [SerializeField] private TMP_InputField questionNumIF;
    [SerializeField] private Button saveBtn;
    [SerializeField] private GameObject errorMessage;
    [SerializeField] private GameObject successPanel;
    [SerializeField] private GameObject AddDataPanel;
    [SerializeField] private GameObject LoadingPanel;
    [SerializeField] private Transform objectCameraTransform;
    CameraController cameraController;
    public int minValue = 1;
    public int maxValue = 10;

    private void Awake()
    {
        cameraController = objectCameraTransform.gameObject.GetComponent<CameraController>();
    }
    private void Start()
    {
        saveBtn.onClick.AddListener(()=> SaveDataButtonListner());
        lessonNumIF.onValueChanged.AddListener(ValidateInputForLesson);
        questionNumIF.onValueChanged.AddListener(ValidateInputForQuestion);
    }

    public async void SaveDataButtonListner()
    {
        if (string.IsNullOrEmpty(lessonNumIF.text) || string.IsNullOrEmpty(questionNumIF.text))
        {

            //StartCoroutine(ErrorMessageDisable(2f));
            //Debug.Log("Data is empty...");
            ErrorMessageDisabled(2f);
            return;
        }
        AddDataPanel.SetActive(false);
        LoadingPanel.SetActive(true);
        CheckAlreadyLesson(lessonNumIF.text, questionNumIF.text, CheckResult);
    }
    void CheckResult(bool a, string id)
    {
        if (a)
        {
            AddDataToFirebase(id);

        }
        else
        {
            AddDataToFirebase(id);
        }
    }

    void AddDataToFirebase(string id)
    {
        string lessonNumber = lessonNumIF.text;
        string questionNumber = questionNumIF.text;
        var Data = new Points(new Lessonss(lessonNumber, questionNumber), objectCameraTransform.position, objectCameraTransform.rotation, cameraController.distance);

        FirebaseController.PostUser(Data, id, () =>
        {
            SuccessMessageDisable();
        });
        
    }

    IEnumerator ErrorMessageDisable(float delay)
    {
        errorMessage.GetComponent<TextMeshProUGUI>().text = "Please Enter all Details!";
        errorMessage.SetActive(true);
        yield return new WaitForSecondsRealtime(delay);
        errorMessage.SetActive(false);
    }


    async void ErrorMessageDisabled(float delay)
    {
  
        errorMessage.GetComponent<TextMeshProUGUI>().text = "Please Enter all Details!";
        errorMessage.SetActive(true);
        await Task.Delay((int)(delay*1000));
        errorMessage.SetActive(false);
    }

    void SuccessMessageDisable()
    {
        LoadingPanel.SetActive(false);
        successPanel.SetActive(true);
        lessonNumIF.text = "";
        questionNumIF.text = "";
        cameraController.ResetPositions();
    }

    public void CheckAlreadyLesson(string lesson, string quesitonnumber,Action<bool,string> callback)
    {
            FirebaseController.GetUsers(users =>
            {
                if (users != null)
                {
                    foreach (var user in users)
                    {
                        if (user.Value.lessons.lessonNumber == lesson && user.Value.lessons.QuesitonNumber == quesitonnumber)
                        {
                            callback(true, user.Key);
                            return;

                        }
                    }
                }

                DateTime now = DateTime.Now;
                string unique_number = now.ToString("yyyyMMddHHmmssfff");
                callback(false, unique_number);
            });
        

    }
    
    private void ValidateInputForLesson(string input)
    {
        if (!string.IsNullOrEmpty(input))
        {


            if (input.Contains("-"))
            {

                input = input.Replace("-", "");


            }
            else if (input.StartsWith("-"))
            {
                input = input.Substring(1);
            }
            
            lessonNumIF.text = input.ToString();
        }
    }
    private void ValidateInputForQuestion(string input)
    {
        if (!string.IsNullOrEmpty(input))
        {

            if (input.Contains("-"))
            {
     
                input = input.Replace("-","");
                

            }else if (input.StartsWith("-"))
            {
                input = input.Substring(1);
            }

            questionNumIF.text = input.ToString();
           
        }
    }


}
