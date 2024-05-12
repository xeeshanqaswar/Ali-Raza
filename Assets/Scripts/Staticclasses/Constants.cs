using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class Constants
{
    public static int LessonCount
    {
        get
        {
            return PlayerPrefs.GetInt("LessonCount", 1);
        }
        set
        {
            PlayerPrefs.SetInt("LessonCount", value);
        }
    }

    public static int continueLastSession
    {
        get
        {
            return PlayerPrefs.GetInt("continueLastSession", 0);
        }
        set
        {
            PlayerPrefs.SetInt("continueLastSession", value);
        }
    }

    public static string userName
    {
        get
        {
            return PlayerPrefs.GetString("userName", "");
        }
        set
        {
            PlayerPrefs.SetString("userName", value);
        }
    }

    public static string username;

    public static int lockedStatus;
    public static string userConsent = "UserConsentGiven";
    public static string dataLoded = "DataLoaded";
    public static string newDataLoaded = "NewDataLoaded";
    public static string apiUri = "https://us-central1-kirintec-training.cloudfunctions.net/getLessons";
    public static string apiVersionUri = "https://us-central1-kirintec-training.cloudfunctions.net/getVersion";

    public static string fileName = "/gameData.json";
    public const string PROGRESS_FILE_NAME = "progressData.json";
    public static string fileName1 = "/gameData1.json";
    public static string fileNameForCameraPostions = "/ cameraData.json";
    public static int currentLesson = 1;
    public static int numberOfQuestionsToDisplay = 4;
    public static int minNumberOfQuestion = 4;
    public static int maxNumberOfQuestion = 10;
    public static int numberOfCorrectAnswer;
    public static int totalNumberofLessons;
    public static float imageCorotuneDelay = 1.5f;
    public static float defaultLightIntensity = 4f;
    public static float targetLightIntensity = 2.5f;
    public static string lessonUnblockHeaderContent = "Each lesson consists of a video tutorial and a confirmation test. After watching the video, you will be required to answer questions about what you have just learnt.\r\n\r\nIn this lesson you will learn about:\r\n";
    
    public static string lessonUnlockBulletContent = "1. How to perform a hard shutdown\n" +
                                                     "2. Warning light indicators\n3. Common error messages\n" +
                                                     "4. How to fix common errors";
    
    public static string tagforSelectableOption = "Selectableoption";
    public static string tagforDrageOption = "drag";
    public static string tagforDrageDefault = "3DModel";
    public static string tagforSelectable = "Selectable";
    public static string flagTypeQuestoin = "Flag Select";
    public static string imageType = "Standard Image Select";
    public static string customType = "Custom Text Select";
    public static string versionKey = "Version";
    public static Vector3 scaleChanger = new Vector3(.9f,.9f,.9f);
    public static Vector3 originalScale = new Vector3(1, 1, 1);
    public static string finalLessonName = "Final";


    public static string[] bulletContents = { 
                                              //first bullet starts here
                                              "表t" + "\n" +
                                              "表t" +
                                              "\n表t" +
                                              "\n表t" +
                                              "\n表t",
                                              // ends here

                                              //second bullet starts here
                                              "表tTurning the System on and off\r\n" +
                                                "表tSystem health checks\r\n" +
                                                "表tStarting and stopping Jamming\r\n" +
                                                "表tSelecting Mission Fills\r\n" +
                                                "表tAlarms\r\n" +
                                                "表tHow to Zeroize the NEO\r\n" +
                                                "表tGNSS\r\n" +
                                                "表tAnd Health and Safety considerations\r\n",
                                              //ends here

                                              // third bullet starts here

                                              "表tThe various states and alarms\r\n" +
                                                "表tIdentifying faults that can be rectified by the Operator\r\n" +
                                                "表tIdentifying faults that require a Maintainer to rectify\r\n"

                                              ,
                                              //ends here

                                              // fourth bullet starts here

                                              "表tSetting up the CTS\r\n" +
                                                "表tHow to conduct a test using the CTS\r\n" +
                                                "表tHow to identify if a test has been successful\r\n" +
                                                "表tHow to Zeroise the CTS\r\n"

                                              ,
                                              //ends here

                                              // fifth bullet starts here
                                              "表tConnecting via RMM Ninja\r\n" +
                                                "表tFunctions of RMM Ninja\r\n"
                                              ,
                                              // ends here

                                              // 6th bullet starts here

                                              ""
                                              ,
                                              //ends here

                                              // 7th bullet starts here
                                              "表tLoading CTS Mission Fills\r\n" +
                                                "表tSetting up the CTS to record log files\r\n" +
                                                "表tExtracting log files\r\n",
                                              // ends here

                                              // 8th bullet starts here
                                              "表tConnecting the NEO to a laptop or Tablet\r\n" +
                                                "表tOpening the tool\r\n" +
                                                "表tDiscovering modules\r\n" +
                                                "表tZeroise software and Fills\r\n" +
                                                "表tUpdating software\r\n" +
                                                "表tChecking Boot size\r\n" +
                                                "表tRecovering Log Files\r\n" +
                                                "表tReboot\r\n" +
                                                "表tInstall test scripts\r\n" +
                                                "表tUpdating Bootloader\r\n" +
                                                "表tFunctions that are not used with the NEO\r\n" ,
                                              // ends here
                                              
                                              // Ninth Bullet Starts here
                                              
                                                "表tMaintenance tasks that are require to ensure that the NEO continues to\r\n" +
                                                "\toperate effectively",
                                              
                                              // ends here

                                                " ", "", "", ""};

    public static Action Disable;
    public static Action NewDisable;

}

[Serializable]
public static class Utilities
{
    public static void SetTextColor(Text text, Color color)
    {
        text.color = color;
    }

    public static void SetTextColor(TextMeshProUGUI textGUI, Color color)
    {
        textGUI.color = color;
    }

    public static void SetImage(Button button, Sprite image)
    {
        button.image.sprite = image;
    }

    public static void SetImage(Image img, Sprite image)
    {
        img.sprite = image;
    }

    public static void SetText(TextMeshProUGUI text, string content)
    {
        text.text = content;
    }

    public static void SetText(Text text, string content)
    {
        text.text = content;
    }

}


public enum QuestionType
{
    StandardTextSelect,
    FlagType,
    TrueFalse,
    ImageBased,
    LocateType,
    LedType,
    SoundType,
    LedTaskBased,
    ConstraintType,
    FlagTypeLocal,
    LedTaskBasedWithOptions,
    Draggable
}
