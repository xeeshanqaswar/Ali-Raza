using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;


public class LedTaskQuestion : SelectableQuestion
{
   [SerializeField]  private Camera mainCamera;
    [SerializeField] private string selectableTag;
    protected float clickTimer = 0f;
   protected bool isClicking = false;
    protected Transform highlight;
   
    private RaycastHit raycastHit;
    public UnityEngine.Color orange;
    public int mainButton =0;

    RefrenceManager refrence;
   public bool wait;
    // Start is called before the first frame update
    public void Awake()
    {
        mainCamera = RefrenceManager.instance.outlineSelection.mainCamera;

    }
    // Update is called once per frame
    protected void Update()
    {
      
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
        {
            highlight = raycastHit.transform;

            if (highlight.CompareTag(Constants.tagforSelectableOption))
            {

                TaskFunctionality();
               


            }
        }

    }
    /// <summary>
    /// Task functionlity 
    /// </summary>
  public virtual void TaskFunctionality()
    {
        if (Input.GetMouseButton(0) && !wait)
        {
            highlight.gameObject.transform.localScale = Constants.scaleChanger;
         
            if (highlight.gameObject.GetComponent<GetElements>().SelectableID == mainButton)
            {
                
                isClicking = true;

                if (isClicking)
                {

                    clickTimer += Time.deltaTime;
                    
                    if (clickTimer >= 3f)
                    {

                        wait = true;
                        highlight.gameObject.transform.localScale = Constants.originalScale;
                        OnSelectButtonListner(highlight.gameObject.GetComponent<GetElements>().SelectableID, true);

                    }
                }
            }
            else
            {
                DelayToResetButton();
                wait = true;
                OnSelectButtonListner(highlight.gameObject.GetComponent<GetElements>().SelectableID, false);
            }

        }
        if (Input.GetMouseButtonUp(0) && clickTimer < 3)
        {
            highlight.gameObject.transform.localScale = Constants.originalScale;
            clickTimer = 0f;
            isClicking = false;
        }
    }
    /// <summary>
    /// adding delay to reset button
    /// </summary>
    public async void DelayToResetButton()
    {
        await Task.Delay(500);
        highlight.gameObject.transform.localScale = Constants.originalScale;

    }
    /// <summary>
    /// select button listner after user selects the options
    /// </summary>
    /// <param name="answer"></param>
    /// <param name="timecomplete"></param>
    public virtual async void OnSelectButtonListner(int answer, bool timecomplete)
    {
        RefrenceManager.instance.locateTypeQuestions.ReasignDefaultTagsObjects();
        RefrenceManager.instance.locateTypeQuestions.HighlitObjectsSetToOff();
        await RefrenceManager.instance.ledRefrence.ChangeToDefaultColor();
        RefrenceManager.instance.questionManager.ledTaskQuestion = false;
        if (timecomplete)
        {
            Debug.Log("this is calling");
            RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(9,orange, true, false, Color.white, false, true);
            // RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(9, UnityEngine.Color.red, false, false, Color.white, false, true);
            await Task.Delay(3000);
            RefrenceManager.instance.ledRefrence.StopFlashing(9);
            await EnablingTheLEd(UnityEngine.Color.red, 9,true);
            await Task.Delay(3000);
            RefrenceManager.instance.ledRefrence.StopFlashing(9);
            await EnablingTheLEd(UnityEngine.Color.gray, 9);


        }
        //   RefrenceManager.instance.uIManager.EnableTaskCompletedPanel();
        RefrenceManager.instance.questionManager.PlayCompleteSound();
        await Task.Delay(2000);
        List<int> answers = new List<int>();
        answers.Add(answer);
        answers.Capacity = answers.Count;
        RefrenceManager.instance.questionManager.NextQuestion(answers, false, false, true, timecomplete);
        //refrenceManager.cameraController.CameraComponentHandler(true);
        await RefrenceManager.instance.ledRefrence.ChangeToDefaultColor();
        EventsHandler.CallOnDisableOption();
        Destroy(this.gameObject);

    }
    /// <summary>
    /// Enable LEDS 
    /// </summary>
    /// <param name="color"></param>
    /// <param name="led"></param>
    /// <param name="isflashing"></param>
    /// <returns></returns>
    public async virtual Task EnablingTheLEd(UnityEngine.Color color ,int led, bool isflashing=false)
    {
       
         RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(led, color, isflashing, false,Color.white,false,false);
    }

}


