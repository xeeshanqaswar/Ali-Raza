using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LedTaskQ3 : LedTaskQuestion
{
    public new void Update()
    {

        base.Update();

    }
 /// <summary>
 /// Task functionality
 /// </summary>
    public override void TaskFunctionality()
    {
        if (Input.GetMouseButton(0) && !wait)
        {

            highlight.gameObject.transform.localScale = Constants.scaleChanger;
            if (highlight.gameObject.GetComponent<GetElements>().SelectableID == mainButton)
            {
                wait = true;
                OnSelectButtonListner(highlight.gameObject.GetComponent<GetElements>().SelectableID, true);
                DelayToResetButton();
            }
            else
            {
                wait = true;
                OnSelectButtonListner(highlight.gameObject.GetComponent<GetElements>().SelectableID, false);
                DelayToResetButton();
            }

        }
        if (Input.GetMouseButtonUp(0) )
        {
            // Reset the timer and clicking flag when the mouse button is released

            highlight.gameObject.transform.localScale = Constants.originalScale;
        }
    }
    /// <summary>
    /// Handling the question after the selection is done
    /// </summary>
    /// <param name="answer"></param>
    /// <param name="timecomplete"></param>
    public override async void OnSelectButtonListner(int answer, bool timecomplete)
    {

        Debug.Log("THIS IS CALLING");
        RefrenceManager.instance.locateTypeQuestions.ReasignDefaultTagsObjects();
        RefrenceManager.instance.locateTypeQuestions.HighlitObjectsSetToOff();
        RefrenceManager.instance.questionManager.ledTaskQuestion = false;
        if (timecomplete)
        {
            RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(10, orange, false, false, Color.white, false, true);
            await Task.Delay(3000);
            await EnablingTheLEd(UnityEngine.Color.green, 10);


        }
        //   RefrenceManager.instance.uIManager.EnableTaskCompletedPanel();
        RefrenceManager.instance.questionManager.PlayCompleteSound();
        await Task.Delay(2000);
        await RefrenceManager.instance.ledRefrence.ChangeToDefaultColor();
        List<int> answers = new List<int>();
        answers.Add(answer);
        answers.Capacity = answers.Count;
        RefrenceManager.instance.questionManager.NextQuestion(answers, false, true, true, timecomplete);
        //refrenceManager.cameraController.CameraComponentHandler(true);
        EventsHandler.CallOnDisableOption();
        Destroy(this.gameObject);

    }
}
