using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using TMPro;
using DG.Tweening;
using JetBrains.Annotations;
using System.Threading.Tasks;

public class ImageQuestion : MonoBehaviour
{

    #region rippleAnimation

    public GameObject ripplePrefab;
    public float rippleScale = 1f;
    public float rippleDuration = 0.5f;
    public float offsetX = 10f; //
    #endregion


    [SerializeField] Image imageOption1, imageOption2, imageOption3, imageOption4;
    [SerializeField] Button[] imageOptionButtons;
    [SerializeField] Button[] imageButtons;
    [SerializeField] GameObject[] imageOptions;
    [SerializeField] Button exitButton;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject questionPanel;
    private Button imageSelectedButton;
    [SerializeField] Button imageConfirmButton;
    [SerializeField] public TextMeshProUGUI question; // question text to be replaced
    public TextMeshProUGUI questionType;
    [SerializeField] TextMeshProUGUI lessonName; // question text to be replaced
    [SerializeField] Image backgroundImage;
    [SerializeField] Color SelectedColor; // selected color for oprion selection
    public string questionType2; // question type for image based questions
    int imageOptionButton;
    QuestionTester _questionDatabase;
    [SerializeField] Color correctOptionColor;
    public float targetOpacity = 0.95f;
    public float duration = 2.0f;
    RefrenceManager refrenceManager;
    [SerializeField] private GameObject popupImage;


    public GameObject option1, option2, option3, option4;
    public Sprite selectedOptionSprite, unselectedOptionSprite, selectedOptionCircle, unselectedOptionCircle;
    public Color selectedOptionTextColor, unselectedOptionColor;


    private void Awake()
    {
        refrenceManager = RefrenceManager.instance;
        _questionDatabase = refrenceManager.questionsDatabase;
    }

    private void OnEnable()
    {
        refrenceManager.cameraController.zoom.ZoomActionHandler(false);
        refrenceManager.cameraController.CameraComponentHandler(false);
        //CameraController.CameraTransition?.Invoke();
        //exitButton.onClick.AddListener(() => refrenceManager.uIManager.resultScreen.ResetExitPanel());
        //Invoke(nameof(ChangeOpacityValue), Constants.imageCorotuneDelay);
        ChangeOpacityValue();
    }
    void Start()
    {
        
        // deselection for 

        //if (imageConfirmButton)
        //{
        //    imageConfirmButton.interactable = false;
        //}

        

        // assigning the callback method to every option button in image
        foreach (var imageButton in imageButtons)
        {
            imageButton.onClick.AddListener(() => ImageOptionSelected(imageButton));
        }

        
    }

    int imageOptionOldIndex = 0;
    
    // Method will be called on any option button selection

    /// <summary>
    /// Selection of User options
    /// </summary>
    /// <param name="imageOptionButtonClicked"></param>
    public void ImageOptionSelected(Button imageOptionButtonClicked)
    {

        SoundManager.manager.ButtonSound();

        imageOptionButton = Array.IndexOf(imageButtons, imageOptionButtonClicked);
        Debug.Log("Image Index on button Click" + imageOptionButton);

        imageSelectedButton = imageOptionButtonClicked;

        if (questionType2 == QuestionType.ImageBased.ToString())
        {
            EventsHandler.CallOnEnableOption(new List<int>() { imageOptionButton });
        }

        //enable the outline to remember the image option selected 

        imageOptions[imageOptionOldIndex].transform.GetChild(2).gameObject.SetActive(false);
        imageOptions[imageOptionButton].transform.GetChild(2).gameObject.SetActive(true);

        imageOptionOldIndex = imageOptionButton;
        imageIndex = imageOptionButton ;

        //SetButtonOnClick(true, imageSelectedButton);        //commented by ahmad

        //commented by Ahmad.
        //imageConfirmButton.GetComponent<Image>().color = SelectedColor;

        //imageConfirmButton.interactable = true;
        //refrenceManager.uIManager.questionSelectBtn.interactable = true;
        refrenceManager.uIManager.SelectBtnBlocker(false);
    }

    void SetButtonOnClick(bool state, Button option)
    {
       
        if (state)
        {
            option.GetComponent<Image>().sprite = selectedOptionSprite;
            option.transform.GetChild(1).GetComponent<Image>().sprite = selectedOptionCircle;
            option.transform.GetChild(1).transform.GetChild(0).
                                                      GetComponent<TextMeshProUGUI>().color = selectedOptionTextColor;
        }
        else
        {
            option.GetComponent<Image>().sprite = unselectedOptionSprite;
            option.transform.GetChild(1).GetComponent<Image>().sprite = unselectedOptionCircle;
            option.transform.GetChild(1).transform.GetChild(0).
                                                      GetComponent<TextMeshProUGUI>().color = unselectedOptionColor;
        }
    }

  /// <summary>
  /// User Confirmation Button when user selects an option 
  /// </summary>
    public void ConfirmSelectionForImage()
    {
        if (imageSelectedButton)
        {
            imageSelectedButton.GetComponent<Image>().color = Color.white;
            imageSelectedButton = null;
            //imageConfirmButton.interactable = false;
            //refrenceManager.uIManager.questionSelectBtn.interactable = false;
            refrenceManager.uIManager.SelectBtnBlocker(true);
        }

    }
   /// <summary>
   /// calls when user presses confirmation button
   /// </summary>
    public async void OnImageSelectButtonListner()
    {
        
        await Task.Delay(400);
        List<int> imageans = new List<int>();
        imageans.Add(imageIndex);
        //imageans.Add(imageOptionButton);
        /* RefrenceManager.instance.zoom.ZoomActionHandler(true);*/
        refrenceManager.questionManager.NextQuestion(imageans);
        //refrenceManager.uIManager.questionSelectBtn.interactable = false;
        refrenceManager.uIManager.SelectBtnBlocker(true);
        EventsHandler.CallOnDisableOption();
        Destroy(this.gameObject);
    }

    public void Animation()
    {
        RectTransform rt = Instantiate(ripplePrefab, transform).GetComponent<RectTransform>();
        rt.localScale = Vector3.zero;
        // Set the initial position to the center of the button
        rt.anchoredPosition = Vector2.zero;
        // Use DOTween to animate the ripple effect
        rt.DOScale(Vector3.one * 2.5f, 0.5f).SetEase(Ease.Linear).OnComplete(() => Destroy(rt.gameObject));
    }

    public void CorrectOptionHighliter(List<int> index)
    {
/*        for (int i = 0; i < index.Count; i++)
        {
            imageOptionButtons[index[i]].GetComponent<Image>().color = correctOptionColor;
        }*/
    }

    /// <summary>
    /// Setting Images options and quesiton on the UI
    /// </summary>
    /// <param name="Quesiton"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="d"></param>
    /// <param name="_questionType"></param>
    public void SetQuestionImage(string lesson,string Quesiton, string a, string b, string c, string d, string _questionType, List<int> correctOption)
    {
        CorrectOptionHighliter(correctOption);
        questionType2 = _questionType;
        string qType = refrenceManager.uIManager.AddSpaceBeforeCapitalLetters(_questionType.ToString(), false);
        questionType.text = qType;
        question.text = Quesiton;
        lessonName.text = lesson;
        imageOption1.sprite = _questionDatabase.LoadImage(a);
        imageOption2.sprite = _questionDatabase.LoadImage(b);
        imageOption3.sprite = _questionDatabase.LoadImage(c);
        imageOption4.sprite = _questionDatabase.LoadImage(d);
     
    }

    /// <summary>
    /// setting up a delay for options images
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>

    void ImageOptionsEffect()
    {
        optionsPanel.SetActive(true);
    }

    void ChangeOpacityValue()
    {
        Color currentColor = backgroundImage.color;
        Color targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, targetOpacity);
        // Use DoTween to animate the color's alpha value over time
        backgroundImage.DOColor(targetColor, duration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => {
                questionPanel.SetActive(true);
                Invoke(nameof(ImageOptionsEffect), 1);
                
            } ); 
    }

    int imageIndex; //Index to disable the image popup
    Vector3 popupInitialPosition;
    public void ImagePoputOnClick(int index)
    {
        SoundManager.manager.ButtonSound();

        foreach (var imageButton in imageOptionButtons)
        {
            imageButton.enabled = false;

            //added by Ahmad
            SetButtonOnClick(false, imageButton);
            
            //imageButton.GetComponent<Image>().color = Color.white;
        }


        SetButtonOnClick(true, imageOptionButtons[index]);

        imageOptions[index].gameObject.SetActive(false);
        //imageOptionButtons[index].gameObject.SetActive(false);

        //remove outline of all the images
        foreach(var image in imageOptions)
        {
            image.transform.GetChild(2).gameObject.SetActive(false);
        }

        popupImage.GetComponent<Image>().sprite = imageOptions[index].GetComponent<Image>().sprite;
        popupImage.SetActive(true);

        //imageConfirmButton.interactable = false;
        //refrenceManager.uIManager.questionSelectBtn.interactable = true;
        refrenceManager.uIManager.SelectBtnBlocker(false);

        imageIndex = index;
        PopupAnimation();

        Debug.Log("Image Index on Image Click" + imageIndex);
        //imageConfirmButton.GetComponent<Image>().color = Color.white;
        //refrenceManager.uIManager.questionSelectBtn.GetComponent<Image>().color = Color.white;

    }

    void PopupAnimation()
    {
        popupInitialPosition = popupImage.transform.position;
        
        popupImage.transform.position = imageOptions[imageIndex].transform.position;
        //popupImage.transform.position = imageOptionButtons[imageIndex].transform.position;
        
        popupImage.transform.DOMove(popupInitialPosition, 0.5f);
    }

    

    public void DisablePopup()
    {
        imageOptions[imageOptionOldIndex].transform.GetChild(2).gameObject.SetActive(false);
        imageOptions[imageIndex].gameObject.SetActive(true);
        imageOptions[imageIndex].transform.GetChild(2).gameObject.SetActive(true);
        imageOptionOldIndex = imageIndex;
        popupImage.SetActive(false);
        foreach (var item in imageOptionButtons)
        {
            item.enabled = true;
            SetButtonOnClick(false, item);
        }
        
    }


    private void OnDestroy()
    {
        /* refrenceManager.cameraController.zoom.ZoomActionHandler(true);*/
        refrenceManager.cameraController.zoom.ZoomActionHandler(true);
        refrenceManager.cameraController.CameraComponentHandler(true);
    }

}
