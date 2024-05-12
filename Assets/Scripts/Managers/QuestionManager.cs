using DG.Tweening;
using JetBrains.Annotations;
using Syncfusion.Compression.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.Windows;
using static NeoData;

/// <summary>
/// question data manager 
/// </summary>
public class QuestionManager : MonoBehaviour
{
    #region UI refrences
    public ResultScreen resultScreen;
    public bool ledTaskQuestion;
    public bool shouldResetPositions = false;
    int correctanswer;
    public GameObject questionPrefab;
    public GameObject dragablePrefab;
    public GameObject soundQuestionPrefab;
    public List<GameObject> taskLedQuestionPrefab;
    public GameObject questionPrefabForImage;
    public GameObject flagQuestionPrefab;
    public GameObject questionParent;
    public GameObject ledQuestionPrefab;
    public NeoData neoData;
    public NeoDataLocal neoDataLocalDataBase;
    public int currentQuestionIndex = 0;
    public List<LessonViceData> currentResultScreenData = new List<LessonViceData>();
    public List<LessonViceData> resultList = new List<LessonViceData>();
    public List<NeoData.Question> selectedQuestions = new List<NeoData.Question>();
    public List<NeoDataLocal.Question> selectedLocalDatabaseQuestions = new List<NeoDataLocal.Question>();
    public GameObject newQuestion;
    private UIManager _uiManager;
    private GameManager _gameManager;
    private Orbit cameraOrbit;
    public string lessonName;
    public List<QuestionWithCorrectOption> questionsWithCorrectOptions = new List<QuestionWithCorrectOption>();
    public GameObject selectableQuestionPrefav;
    public LedRefrence ledRefrence;
    #endregion
    
    RefrenceManager refrenceManager;
    [HideInInspector]
    public SelectableQuestion question;
    [HideInInspector]
    public LedTaskQuestion ledquestion;
    [HideInInspector]
    public FlagQuestions currentflag;
    Light targetLight;
    public GameObject dropDown, resetPos, speedSlider;
    public List<int> questionIndexes = new List<int>();
    public List<int> taskbasedIndexes = new List<int>();
    public bool isCompleted;
    public bool isFingerCursorEnable, isMagnifyingCursorEnabled;
    public bool isEndLesson, isExitLesson;
    public bool isCustomQuestion;

    public List<GameObject> questions = new List<GameObject>();

    private void Awake()
    {
        refrenceManager = RefrenceManager.instance;
        _uiManager = RefrenceManager.instance.uIManager;
        _gameManager = RefrenceManager.instance.gameManager;

        CheckForFirstUser();

    }

    public void CheckForFirstUser()
    {

/*        if (!PlayerPrefs.HasKey(Constants.userConsent))
        {*/
            _uiManager.LoginScreenEnable();
      /*  }*/
/*        else
        {
            _uiManager.UserConsentPopupEnable();
        }*/

    }

    private void Start()
    {
        isCompleted = false;
        refrenceManager.lessonScreen.isGameEnded = false;
        targetLight = _uiManager.directionalLight;

    }

    /// <summary>
    /// Initialize the Result screen list to store the results
    /// </summary>
    public void InitialiseResultScreenList()
    {
        currentResultScreenData.Clear();
        resultList.Clear();

        for (int i = 0; i < Constants.totalNumberofLessons; i++)
        {
            currentResultScreenData.Add(new LessonViceData());
            
        }

        for(int i = 0; i < currentResultScreenData.Count; i++)
        {
            LessonViceData currentLessonData = RefrenceManager.instance.questionManager.currentResultScreenData[i];
            resultList.Add(currentLessonData.GetCopy());
        }

    }
    
    /// <summary>
    /// Load question based on each lesson 
    /// </summary>
    /// <param name="lessonNumber"></param>
    /// <param name="callback"></param>
    /// 
    public void LoadQuestions(int lessonNumber, Action<bool> callback)
    {
        int totalQuestionLocal = 0;
        
        lessonNumber = lessonNumber - 1;

        if (neoData == null || neoData.lesson == null || neoDataLocalDataBase == null || neoDataLocalDataBase.lessons == null)
        {
            return;
        }

        NeoData.Lesson selectedLesson = neoData.lesson.lessons[lessonNumber];
        lessonName = selectedLesson.name;
        _uiManager.lessonHeader.text = lessonName;
        List<NeoData.Question> allQuestions = new List<NeoData.Question>();


        if ((neoDataLocalDataBase.lessons.Length > lessonNumber))
        {
            NeoDataLocal.Lesson LocalDatabase = neoDataLocalDataBase.lessons[lessonNumber];
            List<NeoDataLocal.Question> allQlocalDatabase = new List<NeoDataLocal.Question>();
            foreach (var question in LocalDatabase.questions)
            {
                allQlocalDatabase.Add(question);
            }
            // Commment for shuffling off
            if (lessonNumber != 1)
            {
                List<NeoDataLocal.Question> shuffledLocalQuestions = ShuffleLocalQuestions(allQlocalDatabase);
                selectedLocalDatabaseQuestions = shuffledLocalQuestions;
            }
            else
            {
                selectedLocalDatabaseQuestions = allQlocalDatabase;
            }
            // upto there also see the code behind shuffle start region
            // uncommrnt for shuffle off

            selectedLocalDatabaseQuestions = allQlocalDatabase;

            totalQuestionLocal = allQlocalDatabase.Count;
        }
        foreach (var question in selectedLesson.questions)
        {
            allQuestions.Add(question);
        }

        // check wheather the lessons have any questions

      
        List<NeoData.Question> shuffledQuestions = ShuffleQuestions(allQuestions);
        Constants.numberOfQuestionsToDisplay = shuffledQuestions.Count + totalQuestionLocal;
        for (int i = 0; i < Constants.numberOfQuestionsToDisplay; i++)
        {
            questionIndexes.Add(i);
        }
        _uiManager.totalQuestions.text = Constants.numberOfQuestionsToDisplay.ToString();
        // Ensure that there are no two consecutive task-based questions

        selectedQuestions = shuffledQuestions;

        // turn off shufffle start
      /*  Shuffle(questionIndexes);*/
        // adding logic to set draggable question at the last
/*        if (lessonNumber == 1)
        {
            int highestValueIndex = questionIndexes.IndexOf(questionIndexes.Max());
            int lastIndex = questionIndexes.Count - 1;
            int temp = questionIndexes[lastIndex];
            questionIndexes[lastIndex] = questionIndexes[highestValueIndex];
            questionIndexes[highestValueIndex] = temp;
        }*/
        // turn off shufffle end
        callback(true);
    }
    
    /// <summary>
    /// Shuffling Question  
    /// </summary>
    /// <param name="questions"></param>
    /// <returns></returns>
    List<NeoData.Question> ShuffleQuestions(List<NeoData.Question> questions)
    {
        List<NeoData.Question> shuffledList = new List<NeoData.Question>(questions);

        // Shuffle the options within each question
        foreach (var question in shuffledList)
        {
            if (question.type != Constants.customType)
            {
                question.options = ShuffleOptions(question.options.ToList()).ToArray();
            }
        }
        return shuffledList;
    }
    
    /// <summary>
    /// Shuffle the local data base questions
    /// </summary>
    /// <param name="questions"></param>
    /// <returns></returns>
    List<NeoDataLocal.Question> ShuffleLocalQuestions(List<NeoDataLocal.Question> questions)
    {
        List<NeoDataLocal.Question> shuffledList = new List<NeoDataLocal.Question>(questions);
        Shuffle(shuffledList);

        foreach (var question in shuffledList)
        {
                question.options = ShuffleOptionslocal(question.options.ToList()).ToArray();
          
        }


        return shuffledList;
    }
    
    /// <summary>
    /// Shuffles options 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    private void Shuffle<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

/// <summary>
/// Shuffles online options 
/// </summary>
/// <param name="options"></param>
/// <returns></returns>
    List<NeoData.Option> ShuffleOptions(List<NeoData.Option> options)
    {

        List<Option> nonEmptyOptions = options.Where(option => !string.IsNullOrEmpty(option.option)).ToList();
        List<Option> emptyOptions = options.Except(nonEmptyOptions).ToList();

        // Shuffle the non-empty options
        for (int i = nonEmptyOptions.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            Option temp = nonEmptyOptions[i];
            nonEmptyOptions[i] = nonEmptyOptions[randomIndex];
            nonEmptyOptions[randomIndex] = temp;
        }

        // Combine shuffled non-empty options with empty options
        List<Option> shuffledOptions = new List<Option>(nonEmptyOptions.Concat(emptyOptions));

        return shuffledOptions;
    }


    List<NeoDataLocal.Options> ShuffleOptionslocal(List<NeoDataLocal.Options> options)
    {

        List<NeoDataLocal.Options> nonEmptyOptions = options.Where(option => !string.IsNullOrEmpty(option.optionText)).ToList();
        List<NeoDataLocal.Options> emptyOptions = options.Except(nonEmptyOptions).ToList();

        // Shuffle the non-empty options
        for (int i = nonEmptyOptions.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            NeoDataLocal.Options temp = nonEmptyOptions[i];
            nonEmptyOptions[i] = nonEmptyOptions[randomIndex];
            nonEmptyOptions[randomIndex] = temp;
        }

        // Combine shuffled non-empty options with empty options
        List<NeoDataLocal.Options> shuffledOptions = new List<NeoDataLocal.Options>(nonEmptyOptions.Concat(emptyOptions));

        return shuffledOptions;
    }

    void QuestionCompleteSound()
    {
        SoundManager.manager.QuestionCompleteSound();
    }

    public void PlayCompleteSound()
    {
        Invoke(nameof(QuestionCompleteSound), 0.3f);
    }

    void DisplayNextQuestion()
    {
        
        refrenceManager.cameraController.camOrbitComponent.isCameraContraint = true;
        refrenceManager.uIManager.questionLine.SetActive(false);
        DisplayCurrentQuestion(ShouldDisplayLocalQuestion());
        refrenceManager.uIManager.questionLine.SetActive(true);
    }

    /// <summary>
    /// Display Next Question
    /// </summary>
    public async void NextQuestion(List<int> answer, bool selectable = false, bool iswaiting = false, bool taskbased = false, bool timecomplete = false, bool Dragable = false, bool dragAns = false)
    {
        if (!taskbased && !Dragable)
        {
            Debug.Log("this is anwering 1");
            DisplayQuestionOnResultScreen(answer, selectable);

        }
        else if (Dragable)
        {
            Debug.Log("this is anwering 2");
            DragBasedQuestionAdditionToResultScreen(dragAns);

        }
        else
        {
            Debug.Log("this is anwering 3");
            TaskBasedQuestionAdditionToResultScreen(timecomplete, answer);

        }

        if (iswaiting)
            await Task.Delay(500);

        currentQuestionIndex++;

        if (currentQuestionIndex < Constants.numberOfQuestionsToDisplay)
        {
            isCompleted = false;
            refrenceManager.lessonScreen.isGameEnded = false;
            Invoke(nameof(DisplayNextQuestion), 0.5f);
        }
        else
        {
            isCompleted = true;
            refrenceManager.lessonScreen.isGameEnded = true;
            Constants.numberOfCorrectAnswer = correctanswer;
            selectedQuestions.Clear();
            questionIndexes.Clear();
            await ledRefrence.ChangeToDefaultColor();
            RefrenceManager.instance.taskCompleted = false;
            RefrenceManager.instance.locateTypeQuestions.ReasignDefaultTagsObjects();
            RefrenceManager.instance.locateTypeQuestions.HighlitObjectsSetToOff();
            RefrenceManager.instance.ledRefrence.StopAllFlashing();
            SoundManager.manager.StopMusic();
            SoundManager.manager.ButtonSound();
            if (RefrenceManager.instance.questionManager.ledTaskQuestion)
                RefrenceManager.instance.questionManager.ledTaskQuestion = false;

            SetCursorToDefault();
            correctanswer = 0;
            currentQuestionIndex = 0;
            questionParent.SetActive(false);
            EnableButtons(false);
            _uiManager.ResultScreenEnable();
            GetQuestionsWithCorrectOptions().Clear();
        }
    }

    void ShowResultScreen()
    {
        _uiManager.ResultScreenEnable();
    }

    public void CopyQuestionList()
    {

        int currentLesson = Constants.currentLesson - 1;

        resultList[currentLesson].lesson.Clear();

        for (int i = 0; i < currentResultScreenData[currentLesson].lesson.Count; i++)
        {
            resultList[currentLesson].lesson.Add(currentResultScreenData[currentLesson].lesson[i]);
        }
    }

    public void UnlockNextLesson()
    {
        currentResultScreenData[Constants.currentLesson - 1].isCompleted = true;
        currentResultScreenData[Constants.currentLesson - 1].isUnlocked = true;

        if (Constants.currentLesson < Constants.totalNumberofLessons)
        {
            currentResultScreenData[Constants.currentLesson].isUnlocked = true;
            refrenceManager.lessonScreen.SettingTabsContent();
        }

        // updating status after progress
        float fillAmoutForLesson = ((float)Constants.numberOfCorrectAnswer / (float)Constants.numberOfQuestionsToDisplay);

        refrenceManager.lessonScreen.SettingStatusAfterProgress(fillAmoutForLesson);
        refrenceManager.lessonScreen.SettingProgressForLessonTabs(fillAmoutForLesson);


        //Updating Progress for lesson tabs
        if (Constants.currentLesson >= Constants.lockedStatus && Constants.currentLesson < Constants.totalNumberofLessons)
        {
            Constants.lockedStatus++;
            Constants.LessonCount++;

            refrenceManager.lessonScreen.NextLessonUnlockTheme(Constants.lockedStatus);

            refrenceManager.lessonScreen.isLessonUnlocked = true;

            NeoData.Lesson selectedLesson = neoData.lesson.lessons[Constants.currentLesson];
            selectedLesson.isUnlocked = true;
        }

    }


    public async void EndCurrentLesson()
    {
        refrenceManager.uIManager.questionSelectBtn.gameObject.SetActive(false);
        isEndLesson = true;
        Destroy(newQuestion.gameObject);
        selectedQuestions.Clear();
        questionIndexes.Clear();
        await ledRefrence.ChangeToDefaultColor();

        RefrenceManager.instance.locateTypeQuestions.ReasignDefaultTagsObjects();
        RefrenceManager.instance.locateTypeQuestions.HighlitObjectsSetToOff();
        RefrenceManager.instance.ledRefrence.StopAllFlashing();
        
        SoundManager.manager.StopMusic();

        if (RefrenceManager.instance.questionManager.ledTaskQuestion)
        RefrenceManager.instance.questionManager.ledTaskQuestion = false;

        SetCursorToDefault();
        correctanswer = 0;
        currentQuestionIndex = 0;
        questionParent.SetActive(false);
        refrenceManager.uIManager.LessonScreenEnable();
        EnableButtons(false);
        GetQuestionsWithCorrectOptions().Clear();
    }

    /// <summary>
    /// Enable the screen buttons
    /// </summary>
    /// <param name="state"></param>
    public void EnableButtons(bool state)
    {
        speedSlider.SetActive(state);
        resetPos.SetActive(state);
    }

    IEnumerator SmoothChangeIntensity(float targetIntensity)
    {
        
        float startIntensity = targetLight.intensity;
        float elapsedTime = 0f;

        while (elapsedTime < 2)
        {
            targetLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / 2);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the light reaches the exact target intensity
        targetLight.intensity = targetIntensity;
    }

    /// <summary>
    /// Display local question
    /// </summary>
    /// <returns></returns>
    public bool ShouldDisplayLocalQuestion()
    {
        if (questionIndexes[currentQuestionIndex]<selectedQuestions.Count)
        {
            return false;
        }
        return true;
    }

    #region  Questions implementations


    /// <summary>
    /// Display Question  
    /// </summary>
    public void DisplayCurrentQuestion( bool localQuestion=false)
    {
        _uiManager.totalQuestionsDone.text = (currentQuestionIndex + 1).ToString();

        if (localQuestion)
        {
            QuestionType type = selectedLocalDatabaseQuestions[questionIndexes[currentQuestionIndex] - selectedQuestions.Count].type;
            FactoryQuestionLocalDb(type);
        }
        else
        {
            // string type = selectedQuestions[currentQuestionIndex].type; // backup code 
            
            string type = selectedQuestions[questionIndexes[currentQuestionIndex]].type;
            RefrenceManager.instance.cameraController.camOrbitComponent.ResetConstraints();
            FactoryOnlineQuestion(type); 
        }
    }

    /// <summary>
    /// Display the online question
    /// </summary>
    /// <param name="type"></param>
    void FactoryOnlineQuestion(string type)
    {

        if (type.Equals(Constants.imageType))
        {
            DisplayImageQuestion();
        }

        else if (type.Equals(Constants.customType))
        {
            DisplayFlagQuestions();
        }

        else if (type.Equals(Constants.flagTypeQuestoin))
        {
            DisplayFlagQuestions();
        }

        else
        {
            SimpleQuestionDisplay();
        }
    }

    /// <summary>
    /// Display the local database question 
    /// </summary>
    /// <param name="type"></param>
    void FactoryQuestionLocalDb(QuestionType type)
    {

        if (type == QuestionType.LocateType)
        {
            DisplayLocateQuestion();

        }
        if (type == QuestionType.StandardTextSelect)
        {

            SimpleQuestionDisplay();
        }

        else if (type == QuestionType.LedTaskBasedWithOptions)
        {
            DisplayLedTaskWithOptionQuestion();
        }

        else if (type == QuestionType.LedType)
        {
            LedQuestionDisplay();
        }

        else if (type == QuestionType.SoundType)
        {
            DisplaySoundsBasedQuestion();
        }

        else if (type == QuestionType.LedTaskBased)
        {
            DisplayLedTaskQuestion();
        }

        else if (type == QuestionType.ConstraintType)
        {
            DisplayCustomConstraintQuestion();
        }

        else if (type == QuestionType.FlagTypeLocal)
        {

            DisplayFlagQuestionsLocal();
        }

        else if (type == QuestionType.Draggable)
        {
            DisplayDraggableQuestion();
        }

    }

    /// <summary>
    /// flag local question
    /// </summary>
    async void DisplayFlagQuestionsLocal()
    {
        RefrenceManager.instance.questionManager.isCustomQuestion = true;

        StartCoroutine(SmoothChangeIntensity(Constants.defaultLightIntensity));
        SetCursorToDefault();
        
        refrenceManager.uIManager.questionSelectBtn.gameObject.SetActive(true);

        refrenceManager.uIManager.SelectBtnBlocker(true);

        shouldResetPositions = true;
        refrenceManager.zoom.ZoomActionHandler(false);
        await Task.Delay(500);
        refrenceManager.zoom.ZoomActionHandler(true);
        int index = questionIndexes[currentQuestionIndex] - selectedQuestions.Count;
        int cameraposition = selectedLocalDatabaseQuestions[index].cameraAngle;

        await refrenceManager.outlineSelection.InterpolateToTarget(cameraposition, async () =>
        {
            await Task.Delay(2500);
            ConstraintQuestion(index);
            refrenceManager.cameraController.CameraComponentHandler(true);
        });

        if (selectedLocalDatabaseQuestions[index] != null)
        {
            questionParent.SetActive(true);
            EnableButtons(true);
            newQuestion = Instantiate(flagQuestionPrefab, questionParent.transform);
            currentflag = newQuestion.GetComponent<FlagQuestions>();
            string l = lessonName;
            string q = selectedLocalDatabaseQuestions[index].questionText;
            List<int> correctOption = CheckCorrectAnswerLocal(index);
            
            correctOption.Capacity = correctOption.Count;
            QuestionWithCorrectOption questionWithCorrectOption = new QuestionWithCorrectOption
            {
                question = q, // Assuming you want the first variation's question
                correctOption = correctOption
            };
            questionsWithCorrectOptions.Add(questionWithCorrectOption);
            _uiManager.questionSelectBtn.onClick.AddListener(currentflag.OnFlagSelectButtonListner);

            currentflag.SetQuestionText(l, q, ChangeQuestionType(QuestionType.FlagTypeLocal.ToString()), 
                LDbOptionData(0, index),LDbOptionData(1, index), LDbOptionData(2, index), 
                LDbOptionData(3, index),  HighlitedDataLocal(index), correctOption, false);


            refrenceManager.uIManager.AnimateQuestion_Type(currentflag.question, currentflag.questionTypeText);
        }

    }

    /// <summary>
    /// Led question handling 
    /// </summary>
    async void LedQuestionDisplay()
    {
        RefrenceManager.instance.questionManager.isCustomQuestion = true;

        StartCoroutine(SmoothChangeIntensity(Constants.targetLightIntensity));
        SetCursorToDefault();
        
        refrenceManager.uIManager.questionSelectBtn.gameObject.SetActive(true);
        refrenceManager.uIManager.SelectBtnBlocker(true);
        await ledRefrence.ChangeToDefaultColor();
        await Task.Delay(1000);
        shouldResetPositions = true;
        refrenceManager.zoom.ZoomActionHandler(false);
        int index = questionIndexes[currentQuestionIndex] - selectedQuestions.Count;
        int cameraposition = selectedLocalDatabaseQuestions[index].cameraAngle;

        await refrenceManager.outlineSelection.InterpolateToTarget(cameraposition, async () =>
        {
            await Task.Delay(2500);
            ConstraintQuestion(index);
            refrenceManager.cameraController.CameraComponentHandler(true);

        });
        if (selectedLocalDatabaseQuestions[index] != null)
        {
            questionParent.SetActive(true);
            EnableButtons(true);
            newQuestion = Instantiate(ledQuestionPrefab, questionParent.transform);
            string q = selectedLocalDatabaseQuestions[index].questionText;
            List<int> answer = new List<int>();
            answer=CheckCorrectAnswerLocal(index);
            answer.Capacity = answer.Count;
            int correctOption = selectedLocalDatabaseQuestions[index].correctAnswerIndex;
           
            
            newQuestion.GetComponent<LedBasedQuestion>().SetQuestionLed(lessonName, q, LDbOptionData(0,index)
                , LDbOptionData(1,index), LDbOptionData(2, index), LDbOptionData(3, index), 
                ChangeQuestionType(QuestionType.LedType.ToString()) , answer[0]);

            refrenceManager.uIManager.AnimateQuestion_Type(
                newQuestion.GetComponent<LedBasedQuestion>().questionText,
                newQuestion.GetComponent<LedBasedQuestion>().questionTypeText);

            _uiManager.questionSelectBtn.onClick.AddListener(newQuestion.GetComponent<LedBasedQuestion>().OnSelectButtonListner);
            
            QuestionWithCorrectOption questionWithCorrectOption = new QuestionWithCorrectOption
            {
                question = q, // Assuming you want the first variation's question
                correctOption = answer
            };
            EnableAllLEDs(index);
            questionsWithCorrectOptions.Add(questionWithCorrectOption);
        }
    }

    string ChangeQuestionType(string inputType)
    {
        // Switch statement for input type changes
        switch (inputType)
        {
            case "StandardTextSelect":
                inputType = "Multiple Choice";
                break;
            case "Draggable":
                inputType = "Perform The Task";
                break;

            case "TrueFalse":
                inputType = "True Or False";
                break;
            case "Flag Select":
                inputType = "Flag Select";
                break;
            case "FlagType":
                inputType = "Flag Select";
                break;
            case "LedTaskBasedWithOptions":
                inputType = "Perform The Task";
                break;

            case "LedTaskBased":
                inputType = "Perform The Task";
                break;

            case "Standard Image Select":
                inputType = "Select Image";
                break;


            case "LedType":
                inputType = "Multiple Choice";
                break;

            case "SoundType":
                inputType = "Perform The Task";
                break;

            case "FlagTypeLocal":
                inputType = "Multiple Choice";
                break;

            case "LocateType":
                inputType = "Locate";
                break;

            case "ConstraintType":
                inputType = "Multiple Choice";
                break;

            case "True/False":
                inputType = "True Or False";
                break;

            case "Standard Text Select":
                inputType = "Multiple Choice";
                break;
            case "Multiple Option Selection":
                inputType = "Select All That Apply";
                break;
            default:
                // Default case if none of the above conditions match
                Debug.LogError("Input type not recognized" + inputType);
                break;

        }
        return inputType;
    }
    
    /// <summary>
    /// Image Question Implmentation
    /// </summary>
    async void DisplayImageQuestion()
    {
        StartCoroutine(SmoothChangeIntensity(Constants.defaultLightIntensity));
        SetCursorToDefault();
        await ledRefrence.ChangeToDefaultColor();
        refrenceManager.uIManager.questionSelectBtn.gameObject.SetActive(true);
        refrenceManager.uIManager.SelectBtnBlocker(true);


        if (selectedQuestions[questionIndexes[currentQuestionIndex]] != null)
        {
            refrenceManager.zoom.ZoomActionHandler(false);
            refrenceManager.cameraController.camOrbitComponent.isCameraContraint = true;
            await Task.Delay(200);
            questionParent.SetActive(true);
            EnableButtons(true);
            newQuestion = Instantiate(questionPrefabForImage, questionParent.transform);
            ImageQuestion imgquestion = newQuestion.GetComponent<ImageQuestion>();
            List<int> correctOption = CheckForCorrectAnswer();
            correctOption.Capacity = correctOption.Count;
            string q = selectedQuestions[questionIndexes[currentQuestionIndex]].question;
            QuestionWithCorrectOption questionWithCorrectOption = new QuestionWithCorrectOption
            {
                question = q, // Assuming you want the first variation's question
                correctOption = correctOption
            };
            questionsWithCorrectOptions.Add(questionWithCorrectOption);
            imgquestion.SetQuestionImage(lessonName, q, OptionsImage(0), OptionsImage(1), OptionsImage(2), 
                OptionsImage(3), ChangeQuestionType(QuestionType.ImageBased.ToString()), correctOption);

            refrenceManager.uIManager.AnimateQuestion_Type(imgquestion.question, imgquestion.questionType);

            _uiManager.questionSelectBtn.onClick.AddListener(imgquestion.OnImageSelectButtonListner);
        }
    }
    
    /// <summary>
    /// Custom Question Implmentation
    /// </summary>
    async void DisplayLocateQuestion()
    {
        RefrenceManager.instance.questionManager.isCustomQuestion = true;

        StartCoroutine(SmoothChangeIntensity(Constants.defaultLightIntensity));
        SetCursorToMaginfyingGlass();
        await ledRefrence.ChangeToDefaultColor();
        refrenceManager.uIManager.questionSelectBtn.gameObject.SetActive(true);
        refrenceManager.uIManager.SelectBtnBlocker(true);


        int index = questionIndexes[currentQuestionIndex] - selectedQuestions.Count;
        refrenceManager.zoom.ZoomActionHandler(false);
        shouldResetPositions = true;
        //await refrenceManager.cameraController.ResetPositionsCopy();
        questionParent.SetActive(true);
        EnableButtons(true);
        newQuestion = Instantiate(selectableQuestionPrefav, questionParent.transform);
        question = newQuestion.GetComponent<SelectableQuestion>();
        string q = selectedLocalDatabaseQuestions[index].questionText;
        int cameraposition = selectedLocalDatabaseQuestions[index].cameraAngle;
       

        await refrenceManager.outlineSelection.InterpolateToTarget(cameraposition, async () =>
        {
            await Task.Delay(2500);
            ConstraintQuestion(index);
            refrenceManager.cameraController.CameraComponentHandler(true);
        });

        List<int> answers= new List<int>();
        answers = CheckCorrectAnswerLocal(index);
        answers.Capacity = answers.Count;
        QuestionWithCorrectOption questionWithCorrectOption = new QuestionWithCorrectOption
        {
            question = q,
            correctOption = answers
        };
        question.SetQuestionText(lessonName, q, ChangeQuestionType(QuestionType.LocateType.ToString()), false);

        refrenceManager.uIManager.AnimateQuestion_Type(question.questionText, question.questionTypeText);

        _uiManager.questionSelectBtn.onClick.AddListener(question.OnSelectButtonListner);
        questionsWithCorrectOptions.Add(questionWithCorrectOption);
        await Task.Delay(1500);
        EnableHighlitedPoints(index);
    }
    
    /// <summary>
    /// Custom contraint question handling 
    /// </summary>
    async void DisplayCustomConstraintQuestion()
    {
        RefrenceManager.instance.questionManager.isCustomQuestion = true;

       /* StartCoroutine(SmoothChangeIntensity(Constants.defaultLightIntensity));*/
        SetCursorToDefault();
        
        refrenceManager.uIManager.questionSelectBtn.gameObject.SetActive(true);
        refrenceManager.uIManager.SelectBtnBlocker(true);
       /* await ledRefrence.ChangeToDefaultColor();*/
       /* shouldResetPositions = true;*/
        //int index = currentQuestionIndex - selectedQuestions.Count; back up
        int index = questionIndexes[currentQuestionIndex] - selectedQuestions.Count;
        /*refrenceManager.zoom.ZoomActionHandler(false);*/
        //await refrenceManager.cameraController.ResetPositionsCopy();
        questionParent.SetActive(true);
        EnableButtons(true);
        newQuestion = Instantiate(questionPrefab, questionParent.transform);
        Questoin questions = newQuestion.GetComponent<Questoin>();
        string q = selectedLocalDatabaseQuestions[index].questionText;
        int cameraposition = selectedLocalDatabaseQuestions[index].cameraAngle;

       /* await refrenceManager.outlineSelection.InterpolateToTarget(cameraposition, async () =>
        {
            await Task.Delay(2500);
            ConstraintQuestion(index);
            refrenceManager.cameraController.CameraComponentHandler(true);

        });*/
        
        List<int> answers = new List<int>();
        answers = CheckCorrectAnswerLocal(index);
        answers.Capacity = answers.Count;
        QuestionWithCorrectOption questionWithCorrectOption = new QuestionWithCorrectOption
        {
            question = q,
            correctOption = answers
        };

        questions.SetQuestionText(lessonName, q, OptionsDataConstraints(0,index), 
           OptionsDataConstraints(1,index), OptionsDataConstraints(2,index), OptionsDataConstraints(3,index),
           ChangeQuestionType(selectedLocalDatabaseQuestions[index].type.ToString()), answers, false);

        refrenceManager.uIManager.AnimateQuestion_Type(questions.question, questions.questionTypeText);

        _uiManager.questionSelectBtn.onClick.AddListener(questions.OnSelectButtonListner);
        questionsWithCorrectOptions.Add(questionWithCorrectOption);
    }

    async void DisplayDraggableQuestion()
    {
        RefrenceManager.instance.questionManager.isCustomQuestion = true;

        StartCoroutine(SmoothChangeIntensity(Constants.defaultLightIntensity));
        SetCursorToDefault();
        
        refrenceManager.uIManager.questionSelectBtn.gameObject.SetActive(true);
        refrenceManager.uIManager.SelectBtnBlocker(true);
       
        shouldResetPositions = true;
        //int index = currentQuestionIndex - selectedQuestions.Count; back up
        int index = questionIndexes[currentQuestionIndex] - selectedQuestions.Count;
        //refrenceManager.zoom.ZoomActionHandler(false);
        //await refrenceManager.cameraController.ResetPositionsCopy();
        questionParent.SetActive(true);
        EnableButtons(true);
        newQuestion = Instantiate(dragablePrefab, questionParent.transform);
        DragableQuestion questions = newQuestion.GetComponent<DragableQuestion>();
        string l = lessonName;
        string q = selectedLocalDatabaseQuestions[index].questionText;
        int cameraposition = selectedLocalDatabaseQuestions[index].cameraAngle;

        await refrenceManager.outlineSelection.InterpolateToTarget(cameraposition, async () =>
        {
            await Task.Delay(1500);
            ConstraintQuestion(index);
            refrenceManager.cameraController.CameraComponentHandler(true);
            refrenceManager.modelPartsHandler.SetDraggableState();
        });
      //  refrenceManager.modelPartsHandler.SetDraggableState();
        List<int> answers = new List<int>();
        answers = CheckCorrectAnswerLocal(index);
        answers.Capacity = answers.Count;
        QuestionWithCorrectOption questionWithCorrectOption = new QuestionWithCorrectOption
        {
            question = q,
            correctOption = answers
        };
        questions.SetQuestionText(l, q, ChangeQuestionType(QuestionType.Draggable.ToString()), false);
        refrenceManager.uIManager.AnimateQuestion_Type(questions.questionText, questions.questionTypeText);
        _uiManager.questionSelectBtn.onClick.AddListener(questions.OnSelectButtonListner);
        questionsWithCorrectOptions.Add(questionWithCorrectOption);
       
     
    }

    /// <summary>
    /// Sounds Question Implmentation
    /// </summary>
    async void DisplaySoundsBasedQuestion()
    {
        RefrenceManager.instance.questionManager.isCustomQuestion = true;

        StartCoroutine(SmoothChangeIntensity(Constants.targetLightIntensity));
        SetCursorToFinger();
        refrenceManager.uIManager.questionSelectBtn.gameObject.SetActive(false);
        await Task.Delay(1000);
        shouldResetPositions = true;
        int index = questionIndexes[currentQuestionIndex] - selectedQuestions.Count;
        ledTaskQuestion = true;
        refrenceManager.zoom.ZoomActionHandler(false);
       
        SoundManager.manager.PlaySound();
        questionParent.SetActive(true);
        EnableButtons(true);
        newQuestion = Instantiate(soundQuestionPrefab, questionParent.transform);
        question = newQuestion.GetComponent<SoundQuestion>();
        string l = lessonName;
        string q = selectedLocalDatabaseQuestions[index].questionText;
        int cameraposition = selectedLocalDatabaseQuestions[index].cameraAngle;

        await refrenceManager.outlineSelection.InterpolateToTarget(cameraposition, async () =>
        {
            await Task.Delay(2500);
            ConstraintQuestion(index);
            refrenceManager.cameraController.CameraComponentHandler(true);
        });
        

        
        List<int> answers = new List<int>();
        answers.Add

        (selectedLocalDatabaseQuestions[index].correctAnswerIndex);
        answers.Capacity = answers.Count;
        QuestionWithCorrectOption questionWithCorrectOption = new QuestionWithCorrectOption
        {
            question = q,
            correctOption = answers
        };
        await ledRefrence.ChangeToDefaultColor();
        EnableAllLEDs(index);
        question.SetQuestionText(l, q, ChangeQuestionType(QuestionType.SoundType.ToString()), false);

        refrenceManager.uIManager.AnimateQuestion_Type(question.questionText, question.questionTypeText);

        _uiManager.questionSelectBtn.onClick.AddListener(question.OnSelectButtonListner);
        questionsWithCorrectOptions.Add(questionWithCorrectOption);
        await Task.Delay(1500);
        EnableHighlitedPoints(index);
    }

    /// <summary>
    /// Led based question 
    /// </summary>
    async void DisplayLedTaskQuestion()
    {
        RefrenceManager.instance.questionManager.isCustomQuestion = true;

        StartCoroutine(SmoothChangeIntensity(Constants.targetLightIntensity));
        SetCursorToFinger();
        refrenceManager.uIManager.questionSelectBtn.gameObject.SetActive(false);
        await Task.Delay(1000);
        shouldResetPositions = true;
        int index = questionIndexes[currentQuestionIndex] - selectedQuestions.Count;
        ledTaskQuestion = true;
        refrenceManager.zoom.ZoomActionHandler(false);
        questionParent.SetActive(true);
        EnableButtons(true);
        newQuestion = Instantiate(taskLedQuestionPrefab[selectedLocalDatabaseQuestions[index].questionNumber], questionParent.transform);
        ledquestion = newQuestion.GetComponent<LedTaskQuestion>();
        string q = selectedLocalDatabaseQuestions[index].questionText;
        int cameraposition = selectedLocalDatabaseQuestions[index].cameraAngle;

        await refrenceManager.outlineSelection.InterpolateToTarget(cameraposition, async () =>
        {
            await Task.Delay(2500);
            ConstraintQuestion(index);
            refrenceManager.cameraController.CameraComponentHandler(true);
        });
        

        await ledRefrence.ChangeToDefaultColor();
        EnableAllLEDs(index);
        List<int> answers = new List<int>();
        answers.Add
            
        (selectedLocalDatabaseQuestions[index].correctAnswerIndex);
        answers.Capacity = answers.Count;
        QuestionWithCorrectOption questionWithCorrectOption = new QuestionWithCorrectOption
        {
            question = q,
            correctOption = answers
        };
        ledquestion.SetQuestionText(lessonName, q, ChangeQuestionType(QuestionType.LedTaskBased.ToString()), 
            false);

        refrenceManager.uIManager.AnimateQuestion_Type(ledquestion.questionText, ledquestion.questionTypeText);

        _uiManager.questionSelectBtn.onClick.AddListener(ledquestion.OnSelectButtonListner);
        questionsWithCorrectOptions.Add(questionWithCorrectOption);
        await Task.Delay(1500);
        EnableHighlitedPoints(index);
    }
    
    /// <summary>
    /// Led task based question 
    /// </summary>
    async void DisplayLedTaskWithOptionQuestion()
    {
        RefrenceManager.instance.questionManager.isCustomQuestion = true;

        StartCoroutine(SmoothChangeIntensity(Constants.targetLightIntensity));
        SetCursorToFinger();
        await ledRefrence.ChangeToDefaultColor();
        refrenceManager.uIManager.questionSelectBtn.gameObject.SetActive(true);
        refrenceManager.uIManager.SelectBtnBlocker(true);

        await Task.Delay(1000);
        shouldResetPositions = true;
        int index = questionIndexes[currentQuestionIndex] - selectedQuestions.Count;
        ledTaskQuestion = true;
        refrenceManager.zoom.ZoomActionHandler(false);
        questionParent.SetActive(true);
        EnableButtons(true);
        newQuestion = Instantiate(taskLedQuestionPrefab[selectedLocalDatabaseQuestions[index].questionNumber], questionParent.transform);
     
        
        int cameraposition = selectedLocalDatabaseQuestions[index].cameraAngle;

        await refrenceManager.outlineSelection.InterpolateToTarget(cameraposition, async () =>
        {
            await Task.Delay(2500);
            ConstraintQuestion(index);
            refrenceManager.cameraController.CameraComponentHandler(true);
        });
        await ledRefrence.ChangeToDefaultColor();
        EnableAllLEDs(index);
        List<int> answers = new List<int>();
        answers = CheckCorrectAnswerLocal(index);
        string q = selectedLocalDatabaseQuestions[index].questionText;
        
        newQuestion.GetComponent<LedBasedQuestion>().SetQuestionLed(lessonName, q, LDbOptionData(0, index), 
            LDbOptionData(1, index), LDbOptionData(2, index), LDbOptionData(3, index), 
            ChangeQuestionType(QuestionType.LedTaskBasedWithOptions.ToString()), answers[0]);

        refrenceManager.uIManager.AnimateQuestion_Type(newQuestion.GetComponent<LedBasedQuestion>().questionText, 
            newQuestion.GetComponent<LedBasedQuestion>().questionTypeText);
        _uiManager.questionSelectBtn.onClick.AddListener(newQuestion.GetComponent<LedBasedQuestion>().OnSelectButtonListner);
        answers.Capacity = answers.Count;

        QuestionWithCorrectOption questionWithCorrectOption = new QuestionWithCorrectOption
        {
            question = q,
            correctOption = answers
        };

        questionsWithCorrectOptions.Add(questionWithCorrectOption);
        await Task.Delay(1500);
        EnableHighlitedPoints(index);
    }

    /// <summary>
    /// Flag Question Implmentation
    /// </summary>
    async void DisplayFlagQuestions()
    {
        SetCursorToDefault();
        StartCoroutine(SmoothChangeIntensity(Constants.defaultLightIntensity));
        await ledRefrence.ChangeToDefaultColor();
        refrenceManager.uIManager.questionSelectBtn.gameObject.SetActive(true);
        refrenceManager.uIManager.SelectBtnBlocker(true);

        refrenceManager.zoom.ZoomActionHandler(false);
        if (shouldResetPositions)
        {
            refrenceManager.cameraController.ResetPositions();
            shouldResetPositions = false;
        }
        await Task.Delay(500);
        refrenceManager.zoom.ZoomActionHandler(true);
        if (selectedQuestions[questionIndexes[currentQuestionIndex]] != null)
        {
            questionParent.SetActive(true);
            EnableButtons(true);
            newQuestion = Instantiate(flagQuestionPrefab, questionParent.transform);
            currentflag = newQuestion.GetComponent<FlagQuestions>();
            string l = lessonName;
            List<int> correctOption = CheckForCorrectAnswer();
            correctOption.Capacity = correctOption.Count;
            QuestionWithCorrectOption questionWithCorrectOption = new QuestionWithCorrectOption
            {
                question = QuestionText(), // Assuming you want the first variation's question
                correctOption = correctOption
            };
            questionsWithCorrectOptions.Add(questionWithCorrectOption);
            _uiManager.questionSelectBtn.onClick.AddListener(currentflag.OnFlagSelectButtonListner);
            
            currentflag.SetQuestionText(l,QuestionText(), ChangeQuestionType(QuestionType.FlagType.ToString())
                ,  OptionsData(0), OptionsData(1), OptionsData(2), OptionsData(3), HighlitedData(), 
                   correctOption);

            refrenceManager.uIManager.AnimateQuestion_Type(currentflag.question, currentflag.questionTypeText);

        }
        
    }
  
     /// <summary>
     /// Simple Question implmentation
     /// </summary>
    async void SimpleQuestionDisplay()
    {
        StartCoroutine(SmoothChangeIntensity(Constants.defaultLightIntensity));
        SetCursorToDefault();

        await ledRefrence.ChangeToDefaultColor();
        refrenceManager.uIManager.questionSelectBtn.gameObject.SetActive(true);
        refrenceManager.uIManager.SelectBtnBlocker(true);

        refrenceManager.zoom.ZoomActionHandler(false);


        if (shouldResetPositions)
        {
            refrenceManager.cameraController.ResetPositions();
            shouldResetPositions = false;
        }
        await Task.Delay(500);
        refrenceManager.zoom.ZoomActionHandler(true);
        if (selectedQuestions[questionIndexes[currentQuestionIndex]] != null)
        {
            questionParent.SetActive(true);
            EnableButtons(true);
            newQuestion = Instantiate(questionPrefab, questionParent.transform);
            Questoin questions = newQuestion.GetComponent<Questoin>();
            string l = lessonName;
            List< int> correctOption = CheckForCorrectAnswer();
            QuestionWithCorrectOption questionWithCorrectOption = new QuestionWithCorrectOption
            {
                question = QuestionText(), // Assuming you want the first variation's question
                correctOption = correctOption
            };
            questionsWithCorrectOptions.Add(questionWithCorrectOption);
            _uiManager.questionSelectBtn.onClick.AddListener(questions.OnSelectButtonListner); 
            
            questions.SetQuestionText(l, QuestionText(), OptionsData(0), OptionsData(1), OptionsData(2), 
               OptionsData(3), ChangeQuestionType(selectedQuestions[questionIndexes[currentQuestionIndex]].
                                                                  type), correctOption);

            refrenceManager.uIManager.AnimateQuestion_Type(questions.question, questions.questionTypeText);

        }
    }


    async void SimpleLocalQuestionDisplay()
    {
        StartCoroutine(SmoothChangeIntensity(Constants.defaultLightIntensity));
        SetCursorToDefault();

        await ledRefrence.ChangeToDefaultColor();
        refrenceManager.uIManager.questionSelectBtn.gameObject.SetActive(true);
        refrenceManager.uIManager.SelectBtnBlocker(true);

        refrenceManager.zoom.ZoomActionHandler(false);


        if (shouldResetPositions)
        {
            refrenceManager.cameraController.ResetPositions();
            shouldResetPositions = false;
        }
        await Task.Delay(500);
        refrenceManager.zoom.ZoomActionHandler(true);

            questionParent.SetActive(true);
            EnableButtons(true);
            newQuestion = Instantiate(questionPrefab, questionParent.transform);
            Questoin questions = newQuestion.GetComponent<Questoin>();
            string l = lessonName;
            List<int> correctOption = CheckForCorrectAnswer();
            QuestionWithCorrectOption questionWithCorrectOption = new QuestionWithCorrectOption
            {
                question = QuestionText(), // Assuming you want the first variation's question
                correctOption = correctOption
            };
            questionsWithCorrectOptions.Add(questionWithCorrectOption);
            _uiManager.questionSelectBtn.onClick.AddListener(questions.OnSelectButtonListner);

            questions.SetQuestionText(l, QuestionText(), OptionsData(0), OptionsData(1), OptionsData(2),
               OptionsData(3), ChangeQuestionType(selectedQuestions[questionIndexes[currentQuestionIndex]].
                                                                  type), correctOption);

            refrenceManager.uIManager.AnimateQuestion_Type(questions.question, questions.questionTypeText);

       
    }



    #region Questions Data


    void ConstraintQuestion(int index)
    {
        if (selectedLocalDatabaseQuestions[index].isConstraint)
        {
            refrenceManager.cameraController.camOrbitComponent.isCameraContraint = true;
            refrenceManager.zoom.ZoomActionHandler(false);
            refrenceManager.outlineSelection.ResetButtonHandler(false);

        }
        else if (selectedLocalDatabaseQuestions[index].someConstraint)
        {
            refrenceManager.cameraController.camOrbitComponent.SetPositionAndRotation();
            refrenceManager.cameraController.camOrbitComponent.SettingSomeConstraints();
            refrenceManager.outlineSelection.ResetButtonHandler(false);
            Invoke(nameof(UnsetContstraint),1f);
            
        }
        else
        {
            refrenceManager.cameraController.camOrbitComponent.SetPositionAndRotation();
            refrenceManager.zoom.ZoomActionHandler(true);
            Invoke(nameof(ResettingConstraints),0.5f);
        }

        if (selectedLocalDatabaseQuestions[index].enableZoom)
        {
            refrenceManager.zoom.ZoomActionHandler(true);
        }
    }

    /// <summary>
    /// Providing the question Text
    /// </summary>
    /// <returns></returns>
    public string QuestionText()
    {
        return selectedQuestions[questionIndexes[currentQuestionIndex]].question;
        
    }


    public void UnsetContstraint()
    {
        refrenceManager.cameraController.camOrbitComponent.isCameraContraint = false;
    }

    public void ResettingConstraints()
    {
        refrenceManager.cameraController.camOrbitComponent.ResetConstraints();
    }
    
    /// <summary>
    /// Providing the options data 
    /// </summary>
    /// <param name="OptionIndex"></param>
    /// <returns></returns>
    public string OptionsData(int OptionIndex)
    {
        return selectedQuestions[questionIndexes[currentQuestionIndex]].options[OptionIndex].option;
    }
    
    /// <summary>
    /// option data for the contraint question
    /// </summary>
    /// <param name="OptionIndex"></param>
    /// <param name="questionIndex"></param>
    /// <returns></returns>
    public string OptionsDataConstraints(int OptionIndex, int questionIndex)
    {
        return selectedLocalDatabaseQuestions[questionIndex].options[OptionIndex].optionText;
    }
    
    /// <summary>
    /// Local database option data 
    /// </summary>
    /// <param name="Optionindex"></param>
    /// <param name="QuestionIndex"></param>
    /// <returns></returns>
    public string LDbOptionData(int Optionindex,int QuestionIndex)
    {
        return  selectedLocalDatabaseQuestions[QuestionIndex].options[Optionindex].optionText;
    }
    
    /// <summary>
    /// Image against option
    /// </summary>
    /// <param name="optionIndex"></param>
    /// <returns></returns>
    public string OptionsImage(int optionIndex)
    {
        return selectedQuestions[questionIndexes[currentQuestionIndex]].options[optionIndex].image;
    }

    /// <summary>
    /// Highlited points data 
    /// </summary>
    /// <returns></returns>
    public List<string> HighlitedData()
    {
        List<string> highlitedPoints = new List<string>();
        for (int i = 0; i < selectedQuestions[questionIndexes[currentQuestionIndex]].options.Length; i++)
        {
            highlitedPoints.Add(selectedQuestions[questionIndexes[currentQuestionIndex]].options[i].highlightedPoint);
        }
        return highlitedPoints;
    }

    /// <summary>
    /// Highlited points data local
    /// </summary>
    /// <returns></returns>
    public List<string> HighlitedDataLocal(int questionIndex)
    {
        List<string> highlitedPoints = new List<string>();
        for (int i = 0; i < selectedLocalDatabaseQuestions[questionIndex].options.Length; i++)
        {
            highlitedPoints.Add(selectedLocalDatabaseQuestions[questionIndex].options[i].highlitedPoints.ToString());
        }
        return highlitedPoints;
    }

    /// <summary>
    /// Enable the LEDs based on question
    /// </summary>
    /// <param name="index"></param>
    void EnableAllLEDs(int index)
    {
        foreach (var led in selectedLocalDatabaseQuestions[index].led)
        {
            ledRefrence.ChangeLedColor(led.index, led.color, led.isFlashing, led.isChangingColor, led.changingColor, led.flashingBackForth,led.changeEmission);
        }
    }
    /// <summary>
    /// Enable the highlited points
    /// </summary>
    /// <param name="index"></param>
    async void EnableHighlitedPoints(int index)
    {
        
        for (int i = 0; i < selectedLocalDatabaseQuestions[index].highlitedPoints.Length; i++)
        {
            await refrenceManager.locateTypeQuestions.HighlitObjects(selectedLocalDatabaseQuestions[index].highlitedPoints[i]);
            refrenceManager.locateTypeQuestions.AssignDiffrentTagsObjects(selectedLocalDatabaseQuestions[index].highlitedPoints[i]);
        }
    }

    /// <summary>
    /// changing cursor icon to magnifying Glass
    /// </summary>
    public void SetCursorToMaginfyingGlass()
    {
        _uiManager.ChangeCursorToMagnifyingGlass();
        isFingerCursorEnable = false;
        isMagnifyingCursorEnabled = true;
    }

    /// <summary>
    /// changing cursor icon to finger
    /// </summary>
    public void SetCursorToFinger()
    {
        _uiManager.ChangeCursorToFinger();
        isFingerCursorEnable = true;
        isMagnifyingCursorEnabled = false;
    }

    /// <summary>
    /// changing cursor icon to finger
    /// </summary>
    public void SetCursorToDefault()
    {
        _uiManager.ChangeCursorToDefault();
        isFingerCursorEnable = false;
        isMagnifyingCursorEnabled = false;
    }

    #endregion


    #endregion

    #region Correct answer
    /// <summary>
    /// Get list of correct answers
    /// </summary>
    /// <returns></returns>
    public List<int> CheckForCorrectAnswer()
    {
        List<int> answer = new List<int>();
        for (int i = 0; i < selectedQuestions[questionIndexes[currentQuestionIndex]].options.Length; i++)
        {
            if (selectedQuestions[questionIndexes[currentQuestionIndex]].options[i].correct)
            {
                answer.Add(i);
            }
        }
        answer.Capacity = answer.Count;
        return answer;
       
    }
    /// <summary>
    /// Check the answer of the local question
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private List<int> CheckCorrectAnswerLocal(int index)
    {
        List<int> answer=new List<int>();
        for (int i = 0; i < selectedLocalDatabaseQuestions[index].options.Length; i++)
        {
            if (selectedLocalDatabaseQuestions[index].options[i].correct)
            {
                answer.Add(i);
            }
        }
        answer.Capacity = answer.Count;
        return answer;
    }
    /// <summary>
    /// Enable the EXit popup panel
    /// </summary>
    public void EnableExitPopupPanel()
    {
        if (_uiManager.loginScreen.gameObject.activeInHierarchy)
        {
            _uiManager.ExitPopup_LoginActivation();
        }
        else if(_uiManager.lessonScreen.gameObject.activeInHierarchy)
        {
            _uiManager.ExitPopup_MainMenuActivation();
        }
        else
        {
            _uiManager.ExitPopup_QuestionActivation();
        }
    }

    #endregion
    /// <summary>
    ///  Show Questoin on Result Screen 
    /// </summary>

    #region ResultScreen
    
    /// adding the question to result screen
    public void DisplayQuestionOnResultScreen(List<int> answer, bool selectable=false)
    {
        int lessonnum = Constants.currentLesson - 1;
        List<int> correctOptions = questionsWithCorrectOptions[currentQuestionIndex].correctOption;
        bool correct = correctOptions.OrderBy(option => option)
                                .SequenceEqual(answer.OrderBy(option => option));

        if (questionsWithCorrectOptions[currentQuestionIndex].correctOption.Count == answer.Count && correct && !selectable)
        {
            correctanswer++;
            ResultScreenData data = new ResultScreenData
            {

                question = questionsWithCorrectOptions[currentQuestionIndex].question,
                index = currentQuestionIndex,
                status = true
            };
            AddDataToResultDataBase(data, lessonnum);

            //AddDataToResultDataList(data, lessonnum);


        }
        else if (questionsWithCorrectOptions[currentQuestionIndex].correctOption[0] == answer[0] && selectable)
        {

            ResultScreenData data = new ResultScreenData
            {

                question = questionsWithCorrectOptions[currentQuestionIndex].question,
                index = currentQuestionIndex,
                status = true
            };
            correctanswer++;
            AddDataToResultDataBase(data, lessonnum);

            //AddDataToResultDataList(data, lessonnum);
        }
        else
        {

            ResultScreenData data = new ResultScreenData
            {

                question = questionsWithCorrectOptions[currentQuestionIndex].question,
                index = currentQuestionIndex,
                status = false
            };
            AddDataToResultDataBase(data, lessonnum);

            //AddDataToResultDataList(data, lessonnum);

        }
        resultScreen.SetStatus(correctanswer);
    }

    /// <summary>
    /// Task based question to add to result screen
    /// </summary>
    /// <param name="TimeComplete"></param>
    /// <param name="answer"></param>
    public void TaskBasedQuestionAdditionToResultScreen( bool TimeComplete, List<int> answer)
    {
      
        int lessonnum = Constants.currentLesson - 1;
        if (TimeComplete)
        {
            Debug.Log("TimeComplete");
            foreach (int i in answer)
            {
                Debug.Log(i + " ");
            }
            Debug.Log("we are outside " + questionsWithCorrectOptions[currentQuestionIndex].correctOption[0] + "and" + answer[0]);

            if (questionsWithCorrectOptions[currentQuestionIndex].correctOption[0] == answer[0])
            {
                Debug.Log("we are inside ");
                ResultScreenData datas = new ResultScreenData
                {
                    question = questionsWithCorrectOptions[currentQuestionIndex].question,
                    index = currentQuestionIndex,
                    status = true
                };
                correctanswer++;
                AddDataToResultDataBase(datas, lessonnum);

                //AddDataToResultDataList(datas, lessonnum);

            }
            else
            {

                ResultScreenData data = new ResultScreenData
                {
                    question = questionsWithCorrectOptions[currentQuestionIndex].question,
                    index = currentQuestionIndex,
                    status = false
                };
                AddDataToResultDataBase(data, lessonnum);

                //AddDataToResultDataList(data, lessonnum);
            }
        }

        else
        {
            Debug.Log("!TimeComplete");
            ResultScreenData data = new ResultScreenData
            {
                question = questionsWithCorrectOptions[currentQuestionIndex].question,
                index = currentQuestionIndex,
                status = false
            };
            AddDataToResultDataBase(data, lessonnum);

            //AddDataToResultDataList(data, lessonnum);

            /*resultScreen.SetStatus(Constants.numberOfQuestionsToDisplay, correctanswer);*/
        }
    }
    
    /// <summary>
    /// Drag based Question
    /// </summary>
    /// <param name="DragAnswer"></param>
    /// <param name="answer"></param>
    public void DragBasedQuestionAdditionToResultScreen(bool DragAnswer)
    {
        Debug.Log("drag answer is" + DragAnswer);
        int lessonnum = Constants.currentLesson - 1;
        if (DragAnswer)
        {
            ResultScreenData datas = new ResultScreenData
            {
                question = questionsWithCorrectOptions[currentQuestionIndex].question,
                index = currentQuestionIndex,
                status = true
            };
            correctanswer++;
            AddDataToResultDataBase(datas, lessonnum);

            //AddDataToResultDataList(datas, lessonnum);

        }
        else
        {
            ResultScreenData data = new ResultScreenData
            {
                question = questionsWithCorrectOptions[currentQuestionIndex].question,
                index = currentQuestionIndex,
                status = false
            };
            AddDataToResultDataBase(data, lessonnum);

            //AddDataToResultDataList(data, lessonnum);

            /*resultScreen.SetStatus(Constants.numberOfQuestionsToDisplay, correctanswer);*/
        }
    }

    /// <summary>
    /// update the database to store and view the result from the main screen
    /// </summary>
    /// <param name="data"></param>
    /// <param name="lessonnum"></param>
    public void AddDataToResultDataBase(ResultScreenData data, int lessonnum)
    {
        if (currentResultScreenData[lessonnum].lesson.Count > currentQuestionIndex)
        {
            currentResultScreenData[lessonnum].lesson[currentQuestionIndex] = data;
        }
        else
        {
            currentResultScreenData[lessonnum].lesson.Add(data);
        }

    }

    public void AddDataToResultDataList(ResultScreenData data, int lessonnum)
    {
        if (resultList[lessonnum].lesson.Count > currentQuestionIndex)
        {
            resultList[lessonnum].lesson[currentQuestionIndex] = data;
        }
        else
        {
            resultList[lessonnum].lesson.Add(data);
        }

    }

    /// <summary>
    /// Get the list and made them clear
    /// </summary>
    /// <returns></returns>
    public List<QuestionWithCorrectOption> GetQuestionsWithCorrectOptions()
    {
        return questionsWithCorrectOptions;
    }

    #endregion


}


#region Helper classes to handle the database
/// <summary>
/// Store the question and its answer
/// </summary>
[System.Serializable]
public class QuestionWithCorrectOption
{
    public string question;
    public List<int> correctOption;
}

/// <summary>
/// Resiult screen
/// </summary>
[System.Serializable]
public class ResultScreenData
{
    public string question;
    public int index;
    public bool status;
    public int correctAns;
    public int lockedStatus;
    public float fillAmount;

    public ResultScreenData GetCopy()
    {
        return new ResultScreenData()
        { 
            question = this.question,
            index = this.index,
            status = this.status,  
            correctAns = this.correctAns,   
            lockedStatus = this.lockedStatus,   
            fillAmount = this.fillAmount,   
        }; 
    }
}

/// <summary>
/// Lesson vice data
/// </summary>
[System.Serializable]
public class LessonViceData
{
    public List<ResultScreenData> lesson = new List<ResultScreenData>();
    public bool isCompleted;
    public bool isUnlocked;
    public string name;

    public LessonViceData GetCopy()
    {
        var temp = new LessonViceData()
        {
            lesson = new List<ResultScreenData>(),
            isCompleted = this.isCompleted,
            name = this.name,
            isUnlocked = this.isUnlocked,
        };
        foreach (var item in lesson) 
        { 
            temp.lesson.Add(item);  
        }
        return temp;
    }
}
#endregion