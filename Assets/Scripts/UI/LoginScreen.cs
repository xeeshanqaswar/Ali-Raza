using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;

public class LoginScreen : MonoBehaviour
{

    UIManager uiManager;
    [SerializeField] Button launchButton;
    [SerializeField] TMP_InputField studentID;
    [SerializeField] TextMeshProUGUI placeHolderText;
    [SerializeField] Sprite launchBtnOrangeStroke, launchBtnInactive;

  

    private void Awake()
    {
        uiManager = RefrenceManager.instance.uIManager;
    }

    private void OnEnable()
    {
        Constants.Disable += LaunchGame;
        uiManager.exitPopup_Questions.SetActive(false);
        uiManager.exitPopup_Login.SetActive(false);
    }

    private void OnDisable()
    {
        Constants.Disable -= LaunchGame;
    }

    public void FixedUpdate()
    {
        
        if(studentID.text.ToString() == string.Empty)
        {
            launchButton.image.sprite = launchBtnInactive;
            launchButton.interactable = false;
        }
        else
        {
            launchButton.image.sprite = launchBtnOrangeStroke;
            launchButton.interactable = true;
        }

    }
    

    public void LaunchGame()
    {
        if (studentID.text.ToString() != string.Empty)
        {
            Constants.userName = studentID.text;
            Constants.username = studentID.text;
        }

        uiManager.LoginScreenDisable();
    /*    uiManager.LessonScreenEnable();*/
        uiManager.lessonScreen.SetName();
        MainGameHandler._instance.EnablePopup();
    

    }

    public void OpenConsentPopup()
    {
        uiManager.UserConsentPopupEnable();
    }

    public void OnPlaceHolderSelect()
    {
        placeHolderText.text = "";
    }

    public void OnPlaceHolderDeSelect()
    {
        placeHolderText.text = "Student ID";
    }
    
}
