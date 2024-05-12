using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ExitButtonHandler : MonoBehaviour
{
    public GameObject exitpanel;
    public GameObject settings;
    public GameObject endLesson;
    public GameObject exitpopup;

    public async void ExitPanelSetActive()
    {
        await Task.Delay(400);
        exitpanel.SetActive(true);
    } 

    public async void EndLessonPanelSetActive()
    {
        await Task.Delay(400);
        endLesson.SetActive(true);
        RefrenceManager.instance.taskCompleted = false;
        RefrenceManager.instance.modelPartsHandler.ResetToNormalState();
    }

    public async void SettingPanelSetActive()
    {
        await Task.Delay(400);
        settings.SetActive(true);
    }

    public async void ExitPanelSetActiveFalse()
    {
        await Task.Delay(400);
        exitpopup.SetActive(false);
    }
}
