using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundQuestion : SelectableQuestion
{
    #region variables
    [SerializeField] private Camera mainCamera;
    [SerializeField] private string selectableTag;
    protected float clickTimer = 0f;
    protected bool isClicking = false;
    protected Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;
    public UnityEngine.Color orange;
    public int mainButton = 0;
    private bool wait;
    RefrenceManager refrence;

    #endregion
    // Start is called before the first frame update
    void Awake()
    {


        mainCamera = RefrenceManager.instance.outlineSelection.mainCamera;

    }

    /// <summary>
    /// detect the objects using raycast
    /// </summary>
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
    /// performing tasks
    /// </summary>
    public virtual void TaskFunctionality()
    {
        if (Input.GetMouseButtonDown(0) && !wait)
        {
            wait = true;
            OnSelectButtonListner(highlight.gameObject.GetComponent<GetElements>().SelectableID);
        }

    }
    /// <summary>
    /// select button listener
    /// </summary>
    /// <param name="answer"></param>
    public override async void OnSelectButtonListner(int answer)
    {
        if (answer == 2)
        {
            SoundManager.manager.StopMusic();
            await Task.Delay(100);
            //  RefrenceManager.instance.uIManager.EnableTaskCompletedPanel();
            selectButtonListner(answer);
            return;
        }
        await Task.Delay(500);
        selectButtonListner(answer);
        //    RefrenceManager.instance.uIManager.EnableTaskCompletedPanel();
        await Task.Delay(800);
        SoundManager.manager.StopMusic();

    }

    /// <summary>
    /// select button listner override
    /// </summary>
    /// <param name="answer"></param>
    public async void selectButtonListner(int answer)
    {

        Debug.Log("answer is" + answer);
        await RefrenceManager.instance.ledRefrence.ChangeToDefaultColor();
        RefrenceManager.instance.questionManager.ledTaskQuestion = false;
        List<int> answers = new List<int>();
        answers.Add(answer);
        RefrenceManager.instance.locateTypeQuestions.ReasignDefaultTagsObjects();
        RefrenceManager.instance.locateTypeQuestions.HighlitObjectsSetToOff();
        RefrenceManager.instance.questionManager.PlayCompleteSound();
        if (answer == 2)
        {
            RefrenceManager.instance.questionManager.NextQuestion(answers, false, true, true, true);
        }
        else { RefrenceManager.instance.questionManager.NextQuestion(answers, false, true, true, false); }
        //refrenceManager.cameraController.CameraComponentHandler(true);
        EventsHandler.CallOnDisableOption();
        Destroy(this.gameObject);

    }

}
