using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject light;
    public CanvasGroup canvasGroup;
    public GameObject bgImage;
    public enum GameState
    {
        MainScreen,
        Video,
        Model
    }

    private GameState currentState;

    public GameObject modelObject;

    QuestionManager questionManager;
    UIManager uIManager;

    public bool isFirstQuestion;

    private void Start()
    {
        // Initialize the game with the Main Screen state
        modelObject.SetActive(false);
        
        SwitchState(GameState.MainScreen);
        questionManager = RefrenceManager.instance.questionManager;
        uIManager = RefrenceManager.instance.uIManager;
    }

    public void SwitchState(GameState newState)
    {
        CleanupState();

        currentState = newState;

        InitializeState();
    }

    public void FirstQuestionFalse()
    {
        isFirstQuestion = false;
    }

    private async void InitializeState()
    {
        switch (currentState)
        {
            case GameState.MainScreen:
                
                break;
            case GameState.Video:
                light.SetActive(false);
                modelObject.SetActive(false);
                RefrenceManager.instance.cameraController.SetModelToVideoPosition();
                VideoStateHandling();

                
                break;
            case GameState.Model:
                light.SetActive(false);
                bgImage.SetActive(true);
                RefrenceManager.instance.uIManager.MenuIconDisable();
                canvasGroup.alpha = 1f;
                RefrenceManager.instance.uIManager.VideoScreenDisable();
            //    Invoke(nameof(EnableIcons), 2f);
                EnableModelObject();
                Invoke(nameof(ModelStateHandling), .1f);
                
                break;
        }
    }

    void EnableIcons()
    {
        uIManager.MenuIconEnable();
    }

    public void VideoStateHandling()
    {
        Invoke(nameof(EnableModelObject), .1f);
        RefrenceManager.instance.uIManager.MenuIconDisable();

    }

    void EnableModelObject()
    {
        modelObject.SetActive(false);
    }

    void EnableLight()
    {
        light.SetActive(true);
    }


    void ResetModelToDefault()
    {
        RefrenceManager.instance.cameraController.ResetPositions();
    }


    public void ModelStateHandling()
    {
        
        RefrenceManager.instance.questionManager.currentResultScreenData[Constants.currentLesson - 1].lesson.Clear();

        RefrenceManager.instance.questionManager.LoadQuestions(Constants.currentLesson, (bool questionavailabe) =>
        {
            if (questionavailabe)
            {
                isFirstQuestion = true;

                RefrenceManager.instance.questionManager.DisplayCurrentQuestion(RefrenceManager.instance.questionManager.ShouldDisplayLocalQuestion());

                Invoke(nameof(EnableLight), 2f);

                RefrenceManager.instance.questionManager.isEndLesson = false;
                RefrenceManager.instance.questionManager.isExitLesson = false;
                //RefrenceManager.instance.resultScreen.isExitLesson = false;

               // Invoke(nameof(ReEnableCameraThings), 3f);


                //if (!RefrenceManager.instance.questionManager.isCustomQuestion)
                //{
                //    Invoke(nameof(ResetModelToDefault), 2f);
                //}
            }
            else
            {
                uIManager.emptyQuestionScreen.SetTitle(RefrenceManager.instance.questionManager.lessonName);
                uIManager.EmptyQuestionScreenEnable();
            }

        });

    }

    void ReEnableCameraThings()
    {
        RefrenceManager.instance.cameraController.enabled = true;
        RefrenceManager.instance.cameraorbit.enabled = true;
        RefrenceManager.instance.zoom.enabled = true;
    }

    private void CleanupState()
    {
        // Add cleanup code for the current state
        switch (currentState)
        {
            case GameState.MainScreen:
                // Cleanup Main Screen state
                break;
            case GameState.Video:
                // Cleanup Video state
                

                break;
            case GameState.Model:
                // Cleanup Model state
                break;
        }
    }

    public void UnLockAllLevels()
    {
        RefrenceManager.instance.debug.unlockAllLevels = true;
        RefrenceManager.instance.lessonScreen.SetButtonsTheme();
        RefrenceManager.instance.uIManager.EnableLaunchBtnBlocker(true);
        for (int i = 0; i < RefrenceManager.instance.questionManager.currentResultScreenData.Count; i++) 
        {
            
            RefrenceManager.instance.questionManager.currentResultScreenData[i].isUnlocked = true;
        }
    }

    public void LockAllLevels()
    {
        RefrenceManager.instance.debug.unlockAllLevels = false;
        RefrenceManager.instance.lessonScreen.SetButtonsTheme();
        RefrenceManager.instance.lessonScreen.FirstLessonUnlockTheme();
        RefrenceManager.instance.lessonScreen.NextLessonUnlockTheme(Constants.lockedStatus);
        RefrenceManager.instance.uIManager.EnableLaunchBtnBlocker(true);
        RefrenceManager.instance.questionManager.InitialiseResultScreenList();
      RefrenceManager.instance.progressManager.LoadProgress();
        RefrenceManager.instance.questionManager.currentResultScreenData[0].isUnlocked = true;
        /*      for (int i = 0; i < RefrenceManager.instance.questionManager.currentResultScreenData.Count; i++) { }
              {
                  RefrenceManager.instance.questionManager.currentResultScreenData[i].isUnlocked = false;
              }*/
        //RefrenceManager.instance.lessonScreen.ActiveButtonTheme();
    }

    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    private void OnApplicationQuit()
    {
        ///PlayerPrefs.DeleteAll();
    }
}
