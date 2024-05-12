using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreen : MonoBehaviour
{

    public ResultQuestion question;
    public Text statusTotal;
    public Text statusCompleted;
    public GameObject resultScreenQuestionParent;
    public GameObject resultQuestionPrefab;
    public List<GameObject> results;
    public GameObject resultavailableText;
    public Color color;
    public LessonTabs lessonTabs;
    RefrenceManager refrenceManager;
    public int correctnumb = 0;
    public bool isExitLesson;
    public static bool isFinalLesson;

    private void Awake()
    {
        refrenceManager = RefrenceManager.instance;
    }

    private void OnEnable()
    {
        refrenceManager.uIManager.exitPopup_Questions.SetActive(false);
        refrenceManager.uIManager.exitPopup_Login.SetActive(false);
        correctnumb = 0;
        refrenceManager.cameraController.camOrbitComponent.isCameraContraint = false;
        //refrenceManager.cameraController.ResetPositions();
        StartCoroutine(EnablingConstraints());
        refrenceManager.zoom.ZoomActionHandler(false);
        
        if (refrenceManager.questionManager.isExitLesson || refrenceManager.questionManager.isEndLesson)
        {
            
            //we are on the main screen
            ShowDuplicateResult();
        }
        else
        {
            
            //we are in the lesson
            ShowResult();
        }
        
        //SetStatus(refrenceManager.questionManager.currentResultScreenData[lessonnumber].lesson.Count, correctnumb);
        SetInitialStatus(Constants.currentLesson - 1, correctnumb);
    }

    
    void ShowResult()
    {
        Debug.Log("Here..11");
        int lessonnumber = Constants.currentLesson - 1;
        
        if (refrenceManager.questionManager.currentResultScreenData[lessonnumber].lesson.Count > 0)
        {
            for (int i = 0; i < refrenceManager.questionManager.currentResultScreenData[lessonnumber].lesson.Count; i++)
            {
                string question = refrenceManager.questionManager.currentResultScreenData[lessonnumber].lesson[i].question;
                int index = refrenceManager.questionManager.currentResultScreenData[lessonnumber].lesson[i].index;
                bool status = refrenceManager.questionManager.currentResultScreenData[lessonnumber].lesson[i].status;

                if (status)
                {
                    correctnumb++;
                }

                AddQuestion(question, index, status);
            }
            statusCompleted.text = correctnumb.ToString();
            statusTotal.text = refrenceManager.questionManager.currentResultScreenData[lessonnumber].lesson.Count.ToString();
            return;
        }
        else
        {
            if (resultavailableText)
            {
                resultavailableText.SetActive(true);
            }
        }
    }

    void ShowDuplicateResult()
    {
        Debug.Log("Here");
        int lessonnumber = Constants.currentLesson - 1;

        if (refrenceManager.questionManager.resultList[lessonnumber].lesson.Count > 0)
        {
            for (int i = 0; i < refrenceManager.questionManager.resultList[lessonnumber].lesson.Count; i++)
            {
                string question = refrenceManager.questionManager.resultList[lessonnumber].lesson[i].question;
                int index = refrenceManager.questionManager.resultList[lessonnumber].lesson[i].index;
                bool status = refrenceManager.questionManager.resultList[lessonnumber].lesson[i].status;

                if (status)
                {
                    correctnumb++;
                }

                AddQuestion(question, index, status);
            }
            statusCompleted.text = correctnumb.ToString();
            statusTotal.text = refrenceManager.questionManager.resultList[lessonnumber].lesson.Count.ToString();
            return;
        }
        else
        {
            if (resultavailableText)
            {
                resultavailableText.SetActive(true);
            }
        }
    }

    public void OnDisable()
    {
        if (resultavailableText)
        {
            resultavailableText.SetActive(false);
        }
        ResetAll();
        //SetStatus(0, 0);
    }

    public void AddQuestion(string Question, int num, bool answer)
    {
        int questionNo = num + 1;
        GameObject ResultQustion = Instantiate(resultQuestionPrefab, resultScreenQuestionParent.transform);
        ResultQuestion questions = ResultQustion.GetComponent<ResultQuestion>();
        
        if (!answer)
        {
            questions.bgImage.sprite = questions.orangePopup;
            questions.questionText.color = color;
            questions.questionNumberCircle.color = color;
            questions.number.color = color;
            questions.check.gameObject.SetActive(false);
        }
        else
        {
            questions.check.gameObject.SetActive(true);
        }

        questions.questionText.text = Question;
        questions.number.text = questionNo.ToString();
        
        results.Add(ResultQustion);

    }

    public void GeneratePDFReport()
    {
        for(int i = 0; i < Constants.totalNumberofLessons; i++)
        {
            refrenceManager.questionManager.currentResultScreenData[i].name =
                refrenceManager.questionManager.neoData.lesson.lessons[i].name;
        }

        refrenceManager.pdfGenerator.GeneratePDFFile
                     (refrenceManager.questionManager.currentResultScreenData[Constants.currentLesson - 1]);
    }

    public void ResetAll()
    {
        for (int i = 0; i < results.Count; i++)
        {
            Destroy(results[i].gameObject);
        }
        results.Clear();
    }

    public void ResetExitPanel()
    {
        
    }

    bool abc;
    public async  void RetakeLesson()
    {
        if (!abc)
        {
            abc = true;
            refrenceManager.cameraController.SetModelToVideoPosition();
            await Task.Delay(500);
            this.gameObject.SetActive(false);
            UIManager manager = RefrenceManager.instance.uIManager;
            manager.exitPopup_Questions.SetActive(false);
            manager.exitPopup_Login.SetActive(false);
            if (isFinalLesson)
            {
                RefrenceManager.instance.uIManager.fadeEffect.VideoFadeOut(0);
            }
            else
            {
                RefrenceManager.instance.gameManager.SwitchState(GameManager.GameState.Video);
                manager.VideoScreenEnable();
            }
            Debug.Log("Running :   ");
            manager.LessonScreenDisable();
        }
        
        await Task.Delay(500);
        abc = false;
    }

    public void SetStatus(int currentAnswer)
    {
        //Status.text = currentAnswer.ToString()+ "/" +status.ToString();
        //statusTotal.text = total.ToString();
        statusCompleted.text = currentAnswer.ToString();
    }

    public void SetInitialStatus(int index, int currentAnswer)
    {
        //statusCompleted.text = "0";
        //statusTotal.text = refrenceManager.uIManager.lessonScreen.totalQuestions[index].ToString();
        SetStatus(currentAnswer);

        if (Constants.lockedStatus  > Constants.currentLesson)
        {
            Debug.Log("Current Answer : " + currentAnswer);
        }

    }

    IEnumerator EnablingConstraints()
    {
        yield return new WaitForSecondsRealtime(1);
        refrenceManager.cameraController.camOrbitComponent.isCameraContraint = true;
        refrenceManager.zoom.ZoomActionHandler(false);
    }

    public async void disbaleObjects()
    {
        await Task.Delay(400);
        this.gameObject.SetActive(false);
        refrenceManager.uIManager.questionSelectBtn.gameObject.SetActive(false);
        refrenceManager.gameManager.bgImage.SetActive(false);
      //  GeneratePDFReport();
      //  RefrenceManager.instance.lessonScreen.gameObject.SetActive(true);
        refrenceManager.questionManager.isExitLesson = true;
        //isExitLesson = true;
        refrenceManager.questionManager.CopyQuestionList();
      /*  refrenceManager.progressManager.SaveProgress();*/

    }



    
}
