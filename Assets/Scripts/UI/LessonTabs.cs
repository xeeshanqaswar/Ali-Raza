using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;

/*using UnityEditor.UIElements;*/

public class LessonTabs : MonoBehaviour
{

    #region rippleAnimation

    public GameObject ripplePrefab;
    public float rippleScale = 1f;
    public float rippleDuration = 0.5f;
    public float offsetX = 10f; //
    #endregion


    #region refrences

    public Text mainText, mainTextHeaderDuplicate;
    public TextMeshProUGUI mainTextBullets, mainTextBulletsDuplicate;
    public Text heading;
    public Text totalStatus;
    public Text completedStatus;
    public Text slashText;
    public Text completionStatus;
    public Image filler;
    public Button launchBtn;
    public Button statusBtn;
    public int idNumber;
    public GameObject mainContent, contentFooter, mainContentAnimateObject, mainContentAnimateObjectDuplicate;

    public Sprite activeLaunchButton, inactiveLaunchButton;
    public Color activeTextColor;
    public Sprite activeProgressImage, progressImageOutline, inactiveProgressImage;
    public Color activeProgressTextColor, inactiveTextColor;
    public TextMeshProUGUI launchButtonText;
    public Image progressImage;

    public GameObject transparentImage;
    public GameObject currentStatusBlocker;

    #endregion
    public void MainTextDescription(string header, string bullets)
    {
        Utilities.SetText(mainText, header);
        Utilities.SetText(mainTextBullets, bullets);
        Utilities.SetText(mainTextHeaderDuplicate, header);
        Utilities.SetText(mainTextBulletsDuplicate, bullets);
    }

    public void SetHeading(string text)
    {
        Utilities.SetText(heading, text);
    }

    public void LaunchButtonUnlockedTheme()
    {
        Utilities.SetImage(launchBtn, RefrenceManager.instance.enabledButtonImage);
        Utilities.SetTextColor(launchButtonText, RefrenceManager.instance.whiteColor);
    }

    public void LaunchButtonLockedTheme()
    {
        Utilities.SetImage(launchBtn, RefrenceManager.instance.disabledButtonImage);
        Utilities.SetTextColor(launchButtonText, RefrenceManager.instance.greyColor);
    }

    public void StatusButtonUnlockedTheme()
    {
        Utilities.SetImage(progressImage, RefrenceManager.instance.activeButtonImage);
        Utilities.SetTextColor(totalStatus, RefrenceManager.instance.whiteColor);
        Utilities.SetTextColor(slashText, RefrenceManager.instance.whiteColor);
        Utilities.SetTextColor(completedStatus, RefrenceManager.instance.orangeColor);
        Utilities.SetTextColor(completionStatus, RefrenceManager.instance.whiteColor);
    }

    public void StatusButtonLockedTheme()
    {
        currentStatusBlocker.SetActive(true);

        Utilities.SetImage(progressImage, RefrenceManager.instance.inactiveButtonImage);
        Utilities.SetTextColor(totalStatus, RefrenceManager.instance.greyColor);
        Utilities.SetTextColor(slashText, RefrenceManager.instance.greyColor);
        Utilities.SetTextColor(completedStatus, RefrenceManager.instance.greyColor);
        Utilities.SetTextColor(completionStatus, RefrenceManager.instance.greyColor);
    }

    public void OnCompletePress()
    {
        SoundManager.manager.ButtonSound();
        //Open result screen
        RefrenceManager.instance.uIManager.resultScreen.gameObject.SetActive(true);
    }

    public void SetStatus(string completed, string total, string completionText)
    {
        Utilities.SetText(totalStatus, total);
        Utilities.SetText(RefrenceManager.instance.resultScreen.statusTotal, total);
        
        Utilities.SetText(completedStatus, completed);
        Utilities.SetText(RefrenceManager.instance.resultScreen.statusCompleted, completed);

        //totalStatus.text = total;
        //RefrenceManager.instance.uIManager.resultScreen.statusTotal.text = total;
        //completedStatus.text = completed;
        //RefrenceManager.instance.uIManager.resultScreen.statusCompleted.text = completed;
        //completionStatus.text = completionText;
    }
    bool a;
    public async void LaunchButtonListener(bool isFinalLesson)
    {
        if (!a)
        {
             a = true;
            RefrenceManager.instance.lessonScreen.currentStatusBtnBlocker.SetActive(true);

            SoundManager.manager.ButtonSound();

            await Task.Delay(400);

            if (idNumber < Constants.lockedStatus || RefrenceManager.instance.debug.unlockAllLevels)
            {
                RefrenceManager.instance.lessonScreen.currentStatusBtnBlocker.SetActive(false);

                UIManager manager = RefrenceManager.instance.uIManager;

                /*if (isFinalLesson)
                {

                    //RefrenceManager.instance.uIManager.fadeEffect.VideoFadeOut(0);
                }
                else
                {
                    //RefrenceManager.instance.gameManager.SwitchState(GameManager.GameState.Video);
                    manager.VideoScreenEnable();
                }*/
                manager.LessonScreenDisable();
            }
            else
            {
                RefrenceManager.instance.uIManager.loackedPanel.SetActive(true);
            }
        }
        await Task.Delay(1000);
        a = false;
    }



    public async void CompleteButtonListener()
    {
        RefrenceManager.instance.lessonScreen.launchBtnBlocker.SetActive(true);   //so the user cannot launch the lesson until the result screen is open

        SoundManager.manager.ButtonSound();

        await Task.Delay(400);

        if (RefrenceManager.instance.questionManager.
                                    currentResultScreenData[Constants.currentLesson - 1].isCompleted)
        {
            RefrenceManager.instance.lessonScreen.launchBtnBlocker.SetActive(false);
            RefrenceManager.instance.uIManager.ResultCompleteScreenEnable();
        }
        else
        {
            Debug.Log("Current lesson is not completed");
        }

    }

    public void ResetPanel()
    {
        // Enable the panel game object
        gameObject.SetActive(false);

        // Reset the panel position to its original position


    }

    public void Launch_StatusBtn_ReEnable()
    {
        launchBtn.gameObject.SetActive(false);
        statusBtn.gameObject.SetActive(false);

        launchBtn.gameObject.SetActive(true);
        statusBtn.gameObject.SetActive(true);
    }

    IEnumerator DisableLessonScreen(UIManager manager)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        manager.LessonScreenDisable();
    }

    public void SetInitialStatus(string completed, string total)
    {
        Utilities.SetText(completedStatus, completed);
        Utilities.SetText(totalStatus, total);

        //completedStatus.text = completed;
        //totalStatus.text = total;
    }



}
