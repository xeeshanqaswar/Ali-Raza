using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LedTaskQ6 : LedTaskQuestion
{   // Start is called before the first frame update
    public new void Update()
    {
        // using inherited update
        base.Update();

    }
   /// <summary>
   /// Task Functionality Handling 
   /// </summary>
    public override void TaskFunctionality()
    {
        if (Input.GetMouseButton(0) && !wait)
        {
            //button pressing visual
            highlight.gameObject.transform.localScale = Constants.scaleChanger;
            // Checking if we click the Right button
            if (highlight.gameObject.GetComponent<GetElements>().SelectableID == mainButton)
            {
                
                isClicking = true;

                if (isClicking)
                {

                    clickTimer += Time.deltaTime;
                 
                    if (clickTimer >= 3f)
                    {
                        highlight.gameObject.transform.localScale = Constants.originalScale;
                        wait = true;
                        OnSelectButtonListner(highlight.gameObject.GetComponent<GetElements>().SelectableID, true);

                    }
                }
            }
            else
            {
                wait = true;
                DelayToResetButton();
                OnSelectButtonListner(highlight.gameObject.GetComponent<GetElements>().SelectableID, false);
            }

        }
        // when user realse the button 
        if (Input.GetMouseButtonUp(0))
        {
            // Reset the timer and clicking flag when the mouse button is released
            highlight.gameObject.transform.localScale = Constants.originalScale;

        }
    }
    /// <summary>
    /// Selection of select button
    /// </summary>
    /// <param name="answer"></param>
    /// <param name="timecomplete"></param>
    public override async void OnSelectButtonListner(int answer, bool timecomplete)
    {

        // setting to default color of the lights
        await RefrenceManager.instance.ledRefrence.ChangeToDefaultColor();
        // setting the tags back to orignal one 
        RefrenceManager.instance.locateTypeQuestions.ReasignDefaultTagsObjects();
        // Highliting the object set to off
        RefrenceManager.instance.locateTypeQuestions.HighlitObjectsSetToOff();
        RefrenceManager.instance.questionManager.ledTaskQuestion = false;
        if (timecomplete)
        {
            RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(9, UnityEngine.Color.green, true, false, Color.white, false, false);
            // await Task.Delay(3000);

        }
        //  RefrenceManager.instance.uIManager.EnableTaskCompletedPanel();
        RefrenceManager.instance.questionManager.PlayCompleteSound();
        await Task.Delay(3000);
        await RefrenceManager.instance.ledRefrence.ChangeToDefaultColor();
        List<int> answers = new List<int>();
        answers.Add(answer);
        RefrenceManager.instance.questionManager.NextQuestion(answers, false, true, true, timecomplete);
        answers.Clear();
        EventsHandler.CallOnDisableOption();
        Destroy(this.gameObject);

    }
}
