using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class LedTaskQ2 : LedTaskQuestion
{
    bool color=false;
    public new void  Update()
    {
       
        base.Update();

    }
    

    /// <summary>
    /// Task Functionality
    /// </summary>
    public override void TaskFunctionality()
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
                    Debug.Log(clickTimer);
                    /// enable leds after the 
                    if (clickTimer >= 3f &&!color)
                    {
                        RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(9, orange, true, false, Color.white, false, true);
                        color = true;   

                    }
                    // question passed scenrio
                    if(clickTimer >= 8f)
                    {
                        wait = true;
                        highlight.gameObject.transform.localScale = Constants.originalScale;
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
        // check the failed question scenenrio
        if (Input.GetMouseButtonUp(0) && clickTimer < 8)
        {
            // Reset the timer and clicking flag when the mouse button is released
            highlight.gameObject.transform.localScale = Constants.originalScale;
            OnSelectButtonListner(highlight.gameObject.GetComponent<GetElements>().SelectableID, false);
            clickTimer = 0f;
            isClicking = false;
        }
    }
    /// <summary>
    /// Handling the question after the selection is done
    /// </summary>
    /// <param name="answer"></param>
    /// <param name="timecomplete"></param>
    public override async void OnSelectButtonListner(int answer, bool timecomplete)
    {
        await RefrenceManager.instance.ledRefrence.ChangeToDefaultColor();
        RefrenceManager.instance.ledRefrence.StopFlashing(9);
        RefrenceManager.instance.locateTypeQuestions.ReasignDefaultTagsObjects();
        RefrenceManager.instance.locateTypeQuestions.HighlitObjectsSetToOff();
        RefrenceManager.instance.questionManager.ledTaskQuestion = false;
        if (timecomplete)
        {
            RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(9, Color.gray, false, false, Color.white, false, false);
            RefrenceManager.instance.questionManager.ledRefrence.SetQUadColorDefault(9);
        }
        //   RefrenceManager.instance.uIManager.EnableTaskCompletedPanel();
        RefrenceManager.instance.questionManager.PlayCompleteSound();
        await Task.Delay(2000);
        RefrenceManager.instance.ledRefrence.StopFlashing(9);
        await RefrenceManager.instance.ledRefrence.ChangeToDefaultColor();
        List<int> answers = new List<int>();
        answers.Add(answer);
        answers.Capacity = answers.Count;
        RefrenceManager.instance.questionManager.NextQuestion(answers, false, false, true, timecomplete);
        answers.Clear();
        EventsHandler.CallOnDisableOption();
        Destroy(this.gameObject);

    }
}
