using EasyUI.Tabs;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public RefrenceManager refrenceManager;
    public bool continueLastSession;

    private void Awake()
    {

    }

    private void Start()
    {

    }


    private void SaveProgress(int index)
    {
        string filePathForLesson = GetLessonFilePath(index); // Generate a unique file path for each lesson
        string progressData = JsonUtility.ToJson(refrenceManager.questionManager.
                                                                 currentResultScreenData[index]);

        Debug.Log(progressData);
        
        File.WriteAllText(filePathForLesson, progressData);

        PlayerPrefs.SetString(Constants.userConsent, "True");
        PlayerPrefs.Save();

    }

    public void SaveProgress()
    {
        SaveProgress(Constants.currentLesson - 1);
    }

    public void LoadProgress()
    {
        
        #region new code
        
        for (int i = 0; i < Constants.totalNumberofLessons; i++)
        {
            string filePathForLesson = GetLessonFilePath(i);

            if (File.Exists(filePathForLesson))
            {
                string loadedProgressData = File.ReadAllText(filePathForLesson);

                LessonViceData loadedLessonData = JsonUtility.FromJson<LessonViceData>(loadedProgressData);

                if (loadedLessonData != null && loadedLessonData.lesson.Count > 0)
                {
                    // Update progress for each question in the lesson
                    for (int j = 0; j < loadedLessonData.lesson.Count; j++)
                    {
                        ResultScreenData questionData = loadedLessonData.lesson[j];

                        if (questionData.status)
                        {
                            questionData.correctAns++;
                        }
                    }

                    refrenceManager.questionManager.currentResultScreenData[i] = loadedLessonData;
                    refrenceManager.questionManager.resultList[i] = loadedLessonData.GetCopy();

                    //if the current lesson is unlocked on load progress, next lesson should be unlocked
                    if (refrenceManager.questionManager.currentResultScreenData[i].isCompleted)
                    {
                        if(i + 1 < Constants.totalNumberofLessons)
                            refrenceManager.questionManager.currentResultScreenData[i + 1].isUnlocked = true;
                    }

                    int totalQuestions = loadedLessonData.lesson.Count;
                    int correctAnswers = loadedLessonData.lesson.Select(q => q.correctAns).Sum();

                    // Set progress status for UI
                    TabButtonUI tabBtn = refrenceManager.lessonScreen.tableButton[i].transform.GetChild(0).
                                         GetComponent<TabButtonUI>();

                    LessonTabs tab = refrenceManager.lessonScreen.tableContent[i].GetComponent<LessonTabs>();

                    tab.totalStatus.text = totalQuestions.ToString();
                    tab.completedStatus.text = correctAnswers.ToString();


                    //check mark status
                    tabBtn.CheckMarkStatus(correctAnswers, totalQuestions);

                    //fillbar status
                    float fillAmount = totalQuestions > 0 ? (float)correctAnswers / totalQuestions : 0f;
                    tabBtn.fillBar.fillAmount = fillAmount;
                    tab.filler.fillAmount = fillAmount;
                }
            }
        }
        #endregion

    }

    private string GetLessonFilePath(int lessonIndex)
    {
        return Path.Combine(Application.persistentDataPath, $"lesson_{lessonIndex}_progress.json");
    }

    public void ContinueLastSession()
    {
        LoadProgress();
    }

    public void StartNewSession()
    {
        PlayerPrefs.DeleteKey(Constants.userConsent);
        Constants.continueLastSession = 0;
        continueLastSession = false;
        Constants.LessonCount = 1;
        DeleteAllProgress();
    }

    public void DeleteAllProgress()
    {
        for (int i = 0; i < Constants.totalNumberofLessons; i++)
        {
            string lessonFile = GetLessonFilePath(i);
            if (File.Exists(lessonFile))
            {
                File.Delete(lessonFile);
            }
        }
    }

}
