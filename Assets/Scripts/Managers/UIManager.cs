using DG.Tweening;
using EasyUI.Tabs;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Screen 
    public GameObject emptyDataPanel;
    public GameObject backgroundPanel;
    public GameObject emptyLessonPanel;
    public LoginScreen loginScreen;
    public LessonScreen lessonScreen;
    public EmptyQuestionScreen emptyQuestionScreen;
    public GameObject videoPlayer;
    public ResultScreen resultScreen;
    public ResultScreen completeResultScreen;
    public FadeinFadeout fadeEffect;
    public GameObject loadingScreen, loadingScreenForVersionCheck;
    public GameObject InternetScreen;
    public TabsUI tabsUI;
    public GameObject loackedPanel;
    //public GameObject exitPopupLesson;
    //public GameObject exitPopupresult;
    public GameObject exitPopup_Questions;
    public GameObject exitPopup_Login;
    public GameObject exitPopup_MainMenu;
    public GameObject taskCompleted;
    public Button resetPosButton;

    public GameObject checkUpdateButton;
    public GameObject updatePanel, noUpdatePanel, noInternetPanel;

    public Button questionSelectBtn;
    public Image selectBtnBlocker;
    public Sprite selectBtnSprite, selectBtnDisabledSprite;
    public TextMeshProUGUI lessonHeader;
    public TMP_Dropdown dropdown;
    public GameObject menuIcon, transparentImage, blocker;
    public Light directionalLight;

    public Button unlockBtn;
    public GameObject unlockCheckMark;

    Texture2D MagnifyingGlass;
    Texture2D Finger;
    public TextMeshProUGUI totalQuestionsDone;
    public TextMeshProUGUI totalQuestions;

    public GameObject userConsentPopup;
    public GameObject questionLine;
    public GameObject settingsScreen, endLessonPopup, endSessionPopup;

    public GameObject currentStatusBlocker;

    #endregion

    public void SelectBtnBlocker(bool enabled)
    {
        selectBtnBlocker.gameObject.SetActive(enabled);
        
        if (enabled)
        {
            questionSelectBtn.image.sprite = selectBtnDisabledSprite;
        }
        else
        {
            questionSelectBtn.image.sprite = selectBtnSprite;
        }
    }


    public void AnimateQuestion_Type(TextMeshProUGUI question, TextMeshProUGUI qType)
    {
        StartCoroutine(AnimateQuestions(question, qType));
    }


    IEnumerator AnimateQuestions(TextMeshProUGUI question, TextMeshProUGUI qType)
    {
        question.gameObject.SetActive(false);
        qType.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(1f);
        if (qType)
            qType.gameObject.SetActive(true);
        if (question)
            question.gameObject.SetActive(true);

    }

    private void Awake()
    {
        LoadTextures();
    }

    #region panel on Off 


    public void EnableBgPanel()
    {

        if (!backgroundPanel.activeInHierarchy)
        {
            backgroundPanel.SetActive(true);
        }
    }

    public void DisbaleBgPanel()
    {

        if (backgroundPanel.activeInHierarchy)
        {
            backgroundPanel.SetActive(false);
        }
    }

    public void EnableTaskCompletedPanel()
    {

        if(!taskCompleted.activeInHierarchy)
        {
            taskCompleted.SetActive(true);
        }
    }

    public void DisbaleTaskCompletedPanel()
    {

        if (taskCompleted.activeInHierarchy)
        {
            taskCompleted.SetActive(false);
        }
    }

    public void EnableTransparentImage(bool state)
    {
        transparentImage.SetActive(state);
    }

    public void EnableLaunchBtnBlocker(bool state)
    {
        blocker.SetActive(state);
    }

    void LoadTextures()
    {
        MagnifyingGlass = Resources.Load<Texture2D>("search");
        Finger = Resources.Load<Texture2D>("select");
    }

    public void ChangeCursorToMagnifyingGlass()
    {
        if (Finger != null)
        {
            // Ensure the texture is marked as readable
            Finger.filterMode = FilterMode.Bilinear;
            Finger.anisoLevel = 0;
            Vector2 customHotspot = new Vector2(MagnifyingGlass.width * 0.5f, MagnifyingGlass.height * 0.5f);
            // Set the cursor to the custom texture
            Cursor.SetCursor(MagnifyingGlass, customHotspot, CursorMode.Auto);
        }
        else
        {
            Debug.LogError("Failed to load custom cursor texture.");
        }
    }

    public void ChangeCursorToFinger()
    {
        if (Finger != null)
        {
            // Ensure the texture is marked as readable
            Finger.filterMode = FilterMode.Bilinear;
            Finger.anisoLevel = 0;

            // Set the cursor to the custom texture
            Cursor.SetCursor(Finger, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Debug.LogError("Failed to load custom cursor texture.");
        }
        
    }

    public void ChangeCursorToDefault()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void LoginScreenEnable()
    {
        if (!loginScreen.gameObject.activeInHierarchy)
        {
            loginScreen.gameObject.SetActive(true);
        }
    }

    public void LoginScreenDisable()
    {

        if (loginScreen.gameObject.activeInHierarchy)
        {

            loginScreen.gameObject.SetActive(false);

        }

    }

    public void UserConsentPopupEnable()
    {
        userConsentPopup.SetActive(true);
    }
    
    public void UserConsentPopupDisable()
    {
        userConsentPopup.SetActive(false);
    }

    public void EmptyScreenPanelEnable()
    {
        if (!emptyLessonPanel.gameObject.activeInHierarchy)
        {
            emptyLessonPanel.gameObject.SetActive(true);
        }
    }

    public void EmptyScreenPanelDisable()
    {
        if (emptyLessonPanel.gameObject.activeInHierarchy)
        {
            emptyLessonPanel.gameObject.SetActive(false);
        }
    }

    public void InternetScreenEnable()
    {
        if (!InternetScreen.gameObject.activeInHierarchy)
        {

            InternetScreen.gameObject.SetActive(true);

        }

    }

    public void InternetScreenDisable()
    {

        if (InternetScreen.gameObject.activeInHierarchy)
        {

            InternetScreen.gameObject.SetActive(false);

        }

    }

    public void LoadingScreenEnable()
    {
        if (!loadingScreen.gameObject.activeInHierarchy)
        {
            loadingScreen.gameObject.SetActive(true);
        }

    }

    public void LoadingScreenDisable()
    {

        if (loadingScreen.gameObject.activeInHierarchy)
        {

            loadingScreen.gameObject.SetActive(false);

        }

    }

    public void LoadingScreenForVersionCheck(bool state)
    {
        loadingScreenForVersionCheck.SetActive(state);
    }

    public void LessonScreenEnable()
    {
        if (!lessonScreen.gameObject.activeInHierarchy)
        {
           /* fadeEffect.fadeinout(1f);*/
            lessonScreen.gameObject.SetActive(true);

        }

    }

    public void LessonScreenDisable()
    {

        if (lessonScreen.gameObject.activeInHierarchy)
        {

            lessonScreen.gameObject.SetActive(false);

        }

    }

    public void VideoScreenEnable()
    {
        if (!videoPlayer.gameObject.activeInHierarchy)
        {
         //   fadeEffect.fadeinout(1f);
            videoPlayer.gameObject.SetActive(true);

        }

    }

    public void VideoScreenDisable()
    {

        if (videoPlayer.gameObject.activeInHierarchy)
        {
            videoPlayer.gameObject.SetActive(false);

        }

    }

    #region testing code
    bool a;
    public void OnOff()
    {
        a = !a;
        videoPlayer.SetActive(a);

    }
    #endregion

    public void ResultScreenEnable()
    {
        if (!resultScreen.gameObject.activeInHierarchy)
        {
            fadeEffect.fadeinout(.1f);
            resultScreen.gameObject.SetActive(true);

        }

    }

    public void ResultScreenDisbale()
    {

        if (resultScreen.gameObject.activeInHierarchy)
        {

            resultScreen.gameObject.SetActive(false);

        }

    }

    public void ResultCompleteScreenEnable()
    {
        if (!completeResultScreen.gameObject.activeInHierarchy)
        {
            //fadeEffect.fadeinout();
            completeResultScreen.gameObject.SetActive(true);

        }

    }

    public void UpdatePanelScreenEnable()
    {
        updatePanel.SetActive(true);
        EnableTransparentImage(true);
    }

    public void UpdatePanelScreenDisable()
    {
        updatePanel.SetActive(false);
    }

    public void NoUpdatePanelDisable()
    {
        noUpdatePanel.SetActive(false);
    }

    public void NoUpdatePanelEnable()
    {
        noUpdatePanel.SetActive(true);
        EnableTransparentImage(true);
    }

    public void NoInternetPanelEnable()
    {
        Debug.Log("this is working");
        noInternetPanel.gameObject.SetActive(true);
        EnableTransparentImage(true);
    }

    public void NoInternetPanelDisable()
    {
        noInternetPanel.gameObject.SetActive(false);
    }

    public void ResultCompleteScreenDisbale()
    {

        if (completeResultScreen.gameObject.activeInHierarchy)
        {

            completeResultScreen.gameObject.SetActive(false);

        }

    }

    public void EmptyQuestionScreenEnable()
    {

        if (!emptyQuestionScreen.gameObject.activeInHierarchy)
        {

            emptyQuestionScreen.gameObject.SetActive(true);

        }

    }

    public void EmptyQuestionScreenDisable()
    {

        if (emptyQuestionScreen.gameObject.activeInHierarchy)
        {

            emptyQuestionScreen.gameObject.SetActive(false);

        }

    }

    public void ExitPopup_QuestionActivation()
    {
        if (exitPopup_Questions.gameObject.activeInHierarchy)
        {

            exitPopup_Questions.gameObject.SetActive(false);

        }
        else
        {
            exitPopup_Questions.gameObject.SetActive(true);
        }
    }

    public void ExitPopup_MainMenuActivation()
    {
        if (exitPopup_MainMenu.gameObject.activeInHierarchy)
        {

            exitPopup_MainMenu.gameObject.SetActive(false);

        }
        else
        {
            exitPopup_MainMenu.gameObject.SetActive(true);
        }
    }

    public void ExitPopup_LoginActivation()
    {
        if (exitPopup_Login.gameObject.activeInHierarchy)
        {

            exitPopup_Login.gameObject.SetActive(false);

        }
        else
        {
            exitPopup_Login.gameObject.SetActive(true);
        }
    }

    public void DisableExitPopup_NoLessonActivation()
    {
        exitPopup_Login.gameObject.SetActive(false);
    }

    public void MenuIconEnable()
    {
        menuIcon.SetActive(true);
    }

    public void MenuIconDisable()
    {
        menuIcon.SetActive(false);
    }


    #endregion

    #region ButtonListners
    public void LoginButtonListner()
    {




    }



    #endregion
    /// <summary>
    /// Add spaces before capital letter string
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public string AddSpaceBeforeCapitalLetters(string input, bool isChange = false)
    {
        if (isChange)
        {

            System.Text.StringBuilder result = new System.Text.StringBuilder();

            foreach (char character in input)
            {
                if (char.IsUpper(character) || character == '/')
                {
                    result.Append(" ");
                }
                result.Append(character);
            }

            return result.ToString();
        }

        return input;
    }
}
