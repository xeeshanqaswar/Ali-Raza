using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefrenceManager : MonoBehaviour
{
    public static RefrenceManager instance;
    public UIManager uIManager;
    public GameManager gameManager;
    public QuestionManager questionManager;
    public ModelManager manager;
    public QuestionTester questionsDatabase;
    public CheckInternetConnection internetConnection;
    public CameraController cameraController;
    public LocateTypeQuestions locateTypeQuestions;
    public OutlineSelection outlineSelection;
    public FlagsHandler flagsHandler;
    public ModelPartsHandler modelPartsHandler;
    public Zoom zoom;
    public LedRefrence ledRefrence;
    public DebugEditor debug;
    public LessonScreen lessonScreen;
    public ResultScreen resultScreen; 
    public ResultScreen resultScreenNew; 
    public ProgressManager progressManager;
    public LoginScreen loginScreen;
    public PDFGenerator pdfGenerator;
    public TableButtonsAnimation tableButtonAnimator;
    public CameraOrbit cameraorbit;
    public MyVideoPlayer myVideoPlayer;

    #region UI References

    [Header("UI references")]

    [Header("Colors")]
    public Color whiteColor;
    public Color orangeColor;
    public Color greyColor;

    [Header("Images")]

    [Header("Button Images")]
    public Sprite activeButtonImage;
    public Sprite inactiveButtonImage;
    public Sprite disabledButtonImage;
    public Sprite enabledButtonImage;
    public Sprite orangeButtonImage;
    public Sprite lessonOrangeButtonImage;

    [Header("Circle Images")]
    public Sprite activeCircleImage;
    public Sprite inactiveCircleImage;
    public Sprite fullCircleImage;

    [Header("Sounds")]
    public AudioClip buttonClickSound;
    public AudioClip alarmSound;
    public AudioClip attachSound;
    public AudioClip questionCompleteSound;


    public bool taskCompleted;  
    #endregion


    private void Awake()
    {

        if (!instance)
        {
            instance = this;

        }

    }




}
