using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSessionPopupHandler : MonoBehaviour
{
    int lessonNumber = Constants.currentLesson - 1;
    NeoData.Lesson selectedLesson;
    List<LessonViceData> currentList;



    private void OnEnable()
    {
        currentList = RefrenceManager.instance.questionManager.currentResultScreenData;
        selectedLesson = RefrenceManager.instance.questionManager.neoData.lesson.lessons[lessonNumber];

        RefrenceManager.instance.cameraorbit.enabled = false;
        RefrenceManager.instance.zoom.enabled = false;
        RefrenceManager.instance.cameraController.enabled = false;
    }

    private void OnDisable()
    {
        for(int i = 0; i < RefrenceManager.instance.questionManager.questionsWithCorrectOptions.Count; i++)
        {
            if (RefrenceManager.instance.questionManager.questionsWithCorrectOptions[i].question == "Task: Connect the antennas ")
            {
                RefrenceManager.instance.cameraorbit.enabled = false;
                RefrenceManager.instance.zoom.enabled = false;
                RefrenceManager.instance.cameraController.enabled = false;
            }
            else
            {
                RefrenceManager.instance.cameraorbit.enabled = true;
                RefrenceManager.instance.zoom.enabled = true;
                RefrenceManager.instance.cameraController.enabled = true;
            }
        }
    }

}
