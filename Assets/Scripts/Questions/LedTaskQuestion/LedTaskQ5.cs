using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class LedTaskQ5 : LedTaskQuestion
{
    public new void Update()
    {

        base.Update();

    }
    /// <summary>
    /// Task functionlity
    /// </summary>
    public override void TaskFunctionality()
    {
        if (Input.GetMouseButton(0) && !wait)
        {
          
            highlight.gameObject.transform.localScale = Constants.scaleChanger;
            if (highlight.gameObject.GetComponent<GetElements>().SelectableID == mainButton)
            {
                wait = true;
                DelayToResetButton();
                OnSelectButtonListner(highlight.gameObject.GetComponent<GetElements>().SelectableID, true);


            }
            else
            {
                wait = true;
                DelayToResetButton();
                OnSelectButtonListner(highlight.gameObject.GetComponent<GetElements>().SelectableID, false);
            }

        }
        if (Input.GetMouseButtonUp(0) )
        {
            // Reset the timer and clicking flag when the mouse button is released
            Debug.Log("this is calling for resetting button ");
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


        RefrenceManager.instance.locateTypeQuestions.ReasignDefaultTagsObjects();
        RefrenceManager.instance.locateTypeQuestions.HighlitObjectsSetToOff();
        RefrenceManager.instance.questionManager.ledTaskQuestion = false;
        if (timecomplete)
        {
            RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(10, UnityEngine.Color.gray, false, false, Color.white, false, false);
            RefrenceManager.instance.questionManager.ledRefrence.SetQUadColorDefault(10);

            await Task.Delay(3000);
        }
        //     RefrenceManager.instance.uIManager.EnableTaskCompletedPanel();
        List<int> answers = new List<int>();
        answers.Add(answer);
        answers.Capacity = answers.Count;
        RefrenceManager.instance.questionManager.NextQuestion(answers, false, false, true, timecomplete);
        RefrenceManager.instance.questionManager.PlayCompleteSound();
       
        await RefrenceManager.instance.ledRefrence.ChangeToDefaultColor();
        EventsHandler.CallOnDisableOption();
        Destroy(this.gameObject);

    }
}
