using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EasyUI.Tabs;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using DG.Tweening;

public class LessonScreen : MonoBehaviour
{

    public List<int> totalQuestions; 
    [SerializeField] public Text NameText;
    public List<GameObject> tableContent;
    public List<GameObject> tableButton;
    public List<Image> fillBar;
    public List<Image> fillBarCopy;
    public int unlockLesson;
    public GameObject lessonUnlockPanel;
    public Text unlockLessonText;
    public GameObject lessonPrefab;
    public GameObject lessonParent, lessonNewParent;
    public GameObject contentPrefab;
    public GameObject contentParent;
    [SerializeField] NeoData neoData;
    [SerializeField] NeoDataLocal neoDataLocal;
    public bool isDataUpdated = true;
    public bool isGameEnded = true;
    public bool isUpdateTaken;

    public List<string> lessonBulletContents;

    public GameObject lessonBlocker, launchBtnBlocker, currentStatusBtnBlocker;

    UIManager uIManager;
    QuestionManager _questionManager;
    CameraController cameraController;
    Zoom zoom;
    CameraOrbit cameraOrbit;
    [HideInInspector] public bool isLessonUnlocked;
    public List<LessonViceData> resultList;

    private async void OnEnable()
    {
        RefrenceManager.instance.cameraController.camOrbitComponent.isCameraContraint = true;
        isGameEnded = true;
        RefrenceManager.instance.questionManager.isCustomQuestion = false;
        RefrenceManager.instance.gameManager.light.SetActive(false);
        //Set the camera transforms according to the transforms of the video's model
        cameraController.SetModelToVideoPosition();
        cameraOrbit.enabled = false;
        cameraController.enabled = false;
        zoom.enabled = false;

        if (isLessonUnlocked && !RefrenceManager.instance.debug.unlockAllLevels)
        {
            isLessonUnlocked = false;
            StartCoroutine(ShowUnlockAnimation());
        }

        if (isDataUpdated)
        {
            isDataUpdated = false;
            
            if (Constants.continueLastSession == 1)
            {
                Constants.lockedStatus = Constants.LessonCount;
            }
            else
            {
                Constants.lockedStatus = unlockLesson;
            }
            
            await ClearPreviousData();
            CheckLessons((bool a) =>
            {
                if (a)
                {
                    for (int i = 0; i < Constants.totalNumberofLessons; i++)
                    {
                        lessonBulletContents.Add(Constants.bulletContents[i]);
                        InstantiateLessonButtonsAndTabs(i);
                    }

                    tableContent[0].SetActive(true);

                    if (Constants.continueLastSession == 1)
                    {
                        PreviousSession_LessonsUnlocked();
                    }
                    else
                    {
                        FirstLessonUnlockTheme();
                    }

                    if (isUpdateTaken)
                    {
                        CopyTabBtnContents();
                        StartCoroutine(SlideButtonContents());
                        isUpdateTaken = false;
                    }

                    uIManager.tabsUI.Validate(TabsType.Horizontal);
                    return;
                }
                else
                {
                    //uIManager.EmptyScreenPanelEnable();
                }
            });

            Invoke(nameof(FirstLessonUnlocked), 3f);

            ResetLessonProgress();
        }

    }


    public async void LaunchButtonListener(bool isFinalLesson)
    {


        MainGameHandler._instance.FreezeMovements();
            RefrenceManager.instance.lessonScreen.currentStatusBtnBlocker.SetActive(true);

            SoundManager.manager.ButtonSound();

            await Task.Delay(400);

/*            if (idNumber < Constants.lockedStatus || RefrenceManager.instance.debug.unlockAllLevels)
            {*/
                RefrenceManager.instance.lessonScreen.currentStatusBtnBlocker.SetActive(false);

                UIManager manager = RefrenceManager.instance.uIManager;

/*                if (isFinalLesson)
                {

                    RefrenceManager.instance.uIManager.fadeEffect.VideoFadeOut(0);
                }
                else
                {*/
                    RefrenceManager.instance.gameManager.SwitchState(GameManager.GameState.Video);
                    manager.VideoScreenEnable();
          /*      }*/
                manager.LessonScreenDisable();
         

     
        await Task.Delay(1000);
    
    }




    void FirstLessonUnlocked()
    {
        RefrenceManager.instance.questionManager.currentResultScreenData[0].isUnlocked = true;
    }

    async Task ClearPreviousData()
    {
        for (int i = 0; i < tableContent.Count; i++)
        {
            Destroy(tableContent[i].gameObject);
        }
        for (int i = 0; i < tableButton.Count; i++)
        {
            Destroy(tableButton[i].gameObject);
        }
        
        tableContent.Clear();
        tableButton.Clear();
        totalQuestions.Clear();
        fillBar.Clear();
    }

    void Awake()
    {
        uIManager = RefrenceManager.instance.uIManager;
        _questionManager = RefrenceManager.instance.questionManager;
        cameraController = RefrenceManager.instance.cameraController;
        cameraOrbit = RefrenceManager.instance.cameraorbit;
        zoom = RefrenceManager.instance.zoom;

        resultList = RefrenceManager.instance.questionManager.currentResultScreenData;
    }

    private void Start()
    {
        //Set the camera transforms according to the transforms of the video's model
        cameraController.SetModelToVideoPosition();
        cameraOrbit.enabled = false;
        cameraController.enabled = false;
        zoom.enabled = false;

        if (Constants.continueLastSession == 1)
        {
            GetName();
        }

        CopyTabBtnContents();
        StartCoroutine(SlideButtonContents());
    }

    public void CheckLessons(Action<bool> callback)
    {
        if (Constants.totalNumberofLessons > 0)
        {
            callback(true);
            return;

        }
        callback(false);
    }

    void InstantiateLessonButtonsAndTabs(int i)
    {
        int index = i;
        IntantiateAndSettingTabsButtons(index);
        IntantiateAndSettingTabsContent(index);
    }

    void IntantiateAndSettingTabsButtons(int index)
    {
        GameObject lessonTabBtn = Instantiate(lessonPrefab, lessonParent.transform);


        lessonTabBtn.transform.GetChild(0).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(
                                                                        () => SetActiveButton(index));

        //lessonTabBtn.SetActive(false);

        tableButton.Add(lessonTabBtn);

        //Get reference for table button
        TabButtonUI tabUI = lessonTabBtn.transform.GetChild(0).GetComponent<TabButtonUI>();

        NeoData.Lesson selectedLesson = neoData.lesson.lessons[index];
       
        tabUI.lessonName.text = (selectedLesson.name.ToString());
        tabUI.lessonNo.text = (index + 1).ToString();
        
        fillBar.Add(lessonTabBtn.transform.GetChild(0).GetComponent<TabButtonUI>().fillBar);

        
        tabUI.DisableAllContents();
        
    }

    void CopyTabBtnContents()
    {
        for (int i = 0; i < tableButton.Count; i++)
        {
            TabButtonUI oldContent = tableButton[i].transform.GetChild(0).GetComponent<TabButtonUI>();
            TabButtonUI newContent = oldContent.contentDuplicate.GetComponent<TabButtonUI>();

            newContent.lessonName.text = oldContent.lessonName.text;
            newContent.lessonNo.text = oldContent.lessonNo.text;
            newContent.inactiveCircle.sprite = oldContent.inactiveCircle.sprite;
            newContent.fillBar.fillAmount = oldContent.fillBar.fillAmount;
            newContent.GetComponent<Image>().sprite = oldContent.GetComponent<Image>().sprite;
            newContent.lessonName.color = oldContent.lessonName.color;
            newContent.lessonNo.color = oldContent.lessonNo.color;

            if (Constants.continueLastSession == 1)
            {
                newContent.greenCheck.SetActive(oldContent.greenCheck.activeSelf ? true : false);
                newContent.orangeCheck.SetActive(oldContent.orangeCheck.activeSelf ? true : false);
            }
        }
    }

    IEnumerator SlideButtonContents()
    {
        if(lessonBlocker)
        {
            lessonBlocker.SetActive(true);
        }

        yield return new WaitForSeconds(0.6f);      //delay for the welcome content animation complete

        for(int i = 0; i < tableButton.Count; i++)
        {
            TabButtonUI tabBtn = tableButton[i].transform.GetChild(0).GetComponent<TabButtonUI>();
            yield return new WaitForSeconds(0.15f);
            tabBtn.contentDuplicate.SetActive(true);
        }

        if(lessonBlocker)
        {
            lessonBlocker.SetActive(false);
        }

        EnablePreviousContent();
    }

    void EnablePreviousContent()
    {
        for (int i = 0; i < tableButton.Count; i++)
        {
            TabButtonUI tabBtn = tableButton[i].transform.GetChild(0).GetComponent<TabButtonUI>();
            tabBtn.EnableAllContents();
        }
    }

    void IntantiateAndSettingTabsContent(int index)
    {
        //adding content
        
        GameObject lessonTabContent = Instantiate(contentPrefab, contentParent.transform);
        LessonTabs lessontab = lessonTabContent.GetComponent<LessonTabs>();

        lessontab.mainContentAnimateObject.SetActive(true);

        lessontab.idNumber = index;

        int allQuestionsLocal = 0;
        NeoData.Lesson selectedLesson = neoData.lesson.lessons[index];

        if (index < neoDataLocal.lessons.Length)
        {
            NeoDataLocal.Lesson selectedLessonLocal = neoDataLocal.lessons[index];
            allQuestionsLocal = selectedLessonLocal.questions.Length;
        }

        List<NeoData.Question> allQuestions = new List<NeoData.Question>();
        foreach (var question in selectedLesson.questions)
        {
            allQuestions.Add(question);
        }

        totalQuestions.Add(allQuestions.Count + allQuestionsLocal);

        lessontab.SetInitialStatus("0", totalQuestions[index].ToString());
        lessontab.MainTextDescription(Constants.lessonUnblockHeaderContent, lessonBulletContents[index]);
        
        tableContent.Add(lessonTabContent);
    }

    public void ResetLevel()
    {
        for (int j = Constants.lockedStatus ; j < tableContent.Count; j++)
        {
            if (j != Constants.totalNumberofLessons)
            {
                LessonTabs tabscontent = tableContent[j].GetComponent<LessonTabs>();
                int lessonNum = j + 1;

                tabscontent.MainTextDescription("", "Coming soon");
                
            }
        }
       // tableContent[0].GetComponent<LessonTabs>().SetStatus(0 + "/" + Constants.numberOfQuestionsToDisplay.ToString(), "Incomplete");
    }

    /// <summary>
    /// Resetting the status of all lessons at the start
    /// </summary>
    /// 
    public void ResetLessonProgress()
    {
        foreach (var fill in fillBar)
        {
            fill.GetComponent<Image>().fillAmount = 0;
        }
    }

    public void NextLessonUnlockTheme(int index)
    {
        TabButtonUI tabBtn = tableButton[index - 1].transform.GetChild(0).GetComponent<TabButtonUI>();

        tabBtn.Unlocked_UnselectedLesson();

        tableContent[index - 1].GetComponent<LessonTabs>().LaunchButtonUnlockedTheme();
        tableContent[index - 1].GetComponent<LessonTabs>().StatusButtonUnlockedTheme();
    }

    public void SettingStatusAfterProgress(float fillAmoutForLesson)
    {
        LessonTabs lessontabs = tableContent[Constants.currentLesson - 1].GetComponent<LessonTabs>();
        int numb = Constants.lockedStatus + 1;


        Debug.Log("Total questions: " + Constants.numberOfQuestionsToDisplay.ToString());
        
        lessontabs.SetStatus(Constants.numberOfCorrectAnswer.ToString(), Constants.numberOfQuestionsToDisplay.ToString(), "COMPLETE");

        lessontabs.currentStatusBlocker.SetActive(false);

        TabButtonUI tab = tableButton[Constants.currentLesson - 1].transform.GetChild(0).
                                                                   GetComponent<TabButtonUI>();

        tab.CheckMarkStatus(Constants.numberOfCorrectAnswer, Constants.numberOfQuestionsToDisplay);
        
        lessontabs.filler.fillAmount = fillAmoutForLesson;

        lessontabs.LaunchButtonUnlockedTheme();
        lessontabs.StatusButtonUnlockedTheme();
    }

    public void SettingProgressForLessonTabs(float fillAmoutForLesson)
    {
        int lessonIndex = Constants.currentLesson;
        fillBar[lessonIndex - 1].GetComponent<Image>().fillAmount = fillAmoutForLesson;
    }

    public void SettingTabsContent()
    {
        int lessonsNum = Constants.currentLesson;
        LessonTabs a = tableContent[lessonsNum].GetComponent<LessonTabs>();
        
        a.MainTextDescription(Constants.lessonUnblockHeaderContent, lessonBulletContents[lessonsNum]);

        //a.SetLaunchButton();
        //a.SetStatusBtn_ForCompleted_UnlockLesson();

    }

    IEnumerator ShowUnlockAnimation()
    {
        yield return new WaitForSecondsRealtime(1);
        unlockLessonText.text = "Lesson " + Constants.lockedStatus + " Unlocked";
        lessonUnlockPanel.SetActive(true);
    }

    public void SetName()
    {
        NameText.text = Constants.userName;
        //NameText.text = Constants.username;
    }

    public void GetName()
    {
        PlayerPrefs.GetString(Constants.userName);
    }

    public async void SetActiveButton(int num)
    {
        if (RefrenceManager.instance.questionManager.currentResultScreenData[num].isUnlocked)
        {

            //enable lesson blocker so that the user cannot click on the other lessons before the animations end
            lessonBlocker.SetActive(true);

            TabButtonUI tabUI = tableButton[num].transform.GetChild(0).GetComponent<TabButtonUI>();
            SoundManager.manager.ButtonSound();
            Constants.currentLesson = num + 1;
            RestAllTabs();
            LessonTabs tabs = tableContent[num].GetComponent<LessonTabs>();
            tableContent[num].gameObject.SetActive(true);
            tabs.SetHeading("Lesson " + (num + 1));

            if (tabUI.lessonName.text.IndexOf(Constants.finalLessonName, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                
                tabs.MainTextDescription("", "");
                tabs.launchBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener
               (() => tabs.LaunchButtonListener(true));
                ResultScreen.isFinalLesson = true;
            }
            else
            {
                
                tabs.MainTextDescription(Constants.lessonUnblockHeaderContent, lessonBulletContents[num]);
                tabs.launchBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener
               (() => tabs.LaunchButtonListener(false));
                ResultScreen.isFinalLesson = false;

            }

            
            NeoData.Lesson selectedLesson = neoData.lesson.lessons[num];
            tabs.heading.text = selectedLesson.name.ToString();
            tabs.mainContent.gameObject.SetActive(true);

            tabs.Launch_StatusBtn_ReEnable();

            tabs.mainContentAnimateObject.SetActive(false);
            tabs.mainContentAnimateObjectDuplicate.SetActive(true);

            tabUI.Animation();

            

            Invoke(nameof(LessonBlockerDeActivate), 0.6f);  //de-activate the blocker after the animation

            SetButtonsTheme();

            if (Constants.continueLastSession == 1)
            {
                PreviousSession_LessonsUnlocked();
            }
            else
            {
                FirstLessonUnlockTheme();
            }

            NextLessonUnlockTheme(Constants.lockedStatus);

            if (RefrenceManager.instance.questionManager.currentResultScreenData[num].isUnlocked)
            {
                tabs.StatusButtonUnlockedTheme();
                tabs.LaunchButtonUnlockedTheme();
                uIManager.EnableLaunchBtnBlocker(false);
            }
            else
            {
                uIManager.EnableLaunchBtnBlocker(true);
            }


            //check if the current lesson is completed
            if (RefrenceManager.instance.questionManager.currentResultScreenData[num].isCompleted)
            {
                Complete_SelectedLessonTheme(tabUI);
                
                tabs.currentStatusBlocker.SetActive(false);

                if (tabs.totalStatus.text == tabs.completedStatus.text)
                {
                    tabUI.greenCheck.SetActive(true);
                }
                else
                {
                    tabUI.orangeCheck.SetActive(true);
                }
            }
            else
            {
                InComplete_SelectedLessonTheme(tabUI);
                tabs.StatusButtonLockedTheme();
            }
        }

    }
    
    void LessonBlockerDeActivate()
    {
        lessonBlocker.SetActive(false);
    }

    void Complete_SelectedLessonTheme(TabButtonUI tabBtn)
    {
        tabBtn.button.GetComponent<Image>().sprite = tabBtn.orangeButton;
        tabBtn.lessonName.color = tabBtn.activeTextColor;
        tabBtn.lessonNo.color = tabBtn.activeButtonTextColor;
        tabBtn.inactiveCircle.sprite = tabBtn.fullCircleSprite;
    }

    void InComplete_SelectedLessonTheme(TabButtonUI tabUI)
    {
        //lesson is selected and button is active
        
        tabUI.greenCheck.SetActive(false);
        tabUI.orangeCheck.SetActive(false);

        //active buttons should have the orange store
        tabUI.button.GetComponent<Image>().sprite = tabUI.orangeButton;
        tabUI.inactiveCircle.sprite = tabUI.fullCircleSprite;
        tabUI.lessonName.color = tabUI.activeTextColor; //white color
        tabUI.lessonNo.color = tabUI.activeButtonTextColor; //orange color
    }

    public void RestAllTabs()
    {
        for (int i = 0; i < tableContent.Count; i++)
        {
            tableContent[i].gameObject.SetActive(false);
        }
    }

    public void SetButtonsTheme()
    {
        for (int i = 0; i < tableButton.Count; i++)
        {
            TabButtonUI tabUI = tableButton[i].transform.GetChild(0).GetComponent<TabButtonUI>();
            
            if (!RefrenceManager.instance.questionManager.currentResultScreenData[i].isCompleted)
            {
                if(RefrenceManager.instance.debug.unlockAllLevels)
                {
                    tabUI.Unlocked_UnselectedLesson();
                    tableContent[i].GetComponent<LessonTabs>().StatusButtonUnlockedTheme();
                    tableContent[i].GetComponent<LessonTabs>().LaunchButtonUnlockedTheme();
                }
                else
                {
                    tabUI.Locked_UnSelectedLesson();
                    tableContent[i].GetComponent<LessonTabs>().StatusButtonLockedTheme();
                    tableContent[i].GetComponent<LessonTabs>().LaunchButtonLockedTheme();
                }
            }
            else
            {
                tabUI.Unlocked_UnselectedLesson();
                
                if(tableContent[i].GetComponent<LessonTabs>().totalStatus.text ==
                    tableContent[i].GetComponent<LessonTabs>().completedStatus.text)
                {
                    tabUI.greenCheck.SetActive(true);
                }
                else
                {
                    tabUI.orangeCheck.SetActive(true);
                }
            }
        }
    }

    public void FirstLessonUnlockTheme()
    {
        TabButtonUI tabBtn = tableButton[0].transform.GetChild(0).GetComponent<TabButtonUI>();
        tabBtn.Unlocked_UnselectedLesson();

        tableContent[0].GetComponent<LessonTabs>().LaunchButtonUnlockedTheme();
        tableContent[0].GetComponent<LessonTabs>().StatusButtonUnlockedTheme();
    }

    public void PreviousSession_LessonsUnlocked()
    {
        for (int i = 0; i < Constants.LessonCount; i++)
        {
            TabButtonUI tabBtn = tableButton[i].transform.GetChild(0).GetComponent<TabButtonUI>();
            tabBtn.Unlocked_UnselectedLesson();

            tableContent[i].GetComponent<LessonTabs>().LaunchButtonUnlockedTheme();
            tableContent[i].GetComponent<LessonTabs>().StatusButtonUnlockedTheme();
        }
    }

    public void ActiveButtonTheme()
    {
        TabButtonUI tabBtn = tableButton[Constants.currentLesson - 1].transform.GetChild(0).
                             GetComponent<TabButtonUI>();
        
        tabBtn.ActiveButtonTheme();
    }

    public void ExitLesson()
    {
        // Check if the application is running in the Unity Editor.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
     
#endif
    }

}
