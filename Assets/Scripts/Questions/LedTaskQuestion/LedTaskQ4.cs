using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static NeoDataLocal;

public class LedTaskQ4 : LedTaskQuestion
{
    [SerializeField] bool color = false;
    [SerializeField] bool secondTimer = false;
    [SerializeField] bool ThirdTimer = false;
    [SerializeField] bool fourthtime = false;
    [SerializeField] float newTimer = 0;
    [SerializeField] float newfewTimer = 0;
    [SerializeField] bool step1;
    [SerializeField] bool step2;
    [SerializeField] bool step3;
    [SerializeField] bool step4;
  
    [SerializeField] bool step6;
    public new void  Update()
    {
        // overide the update function as its the same
        base.Update();

    }

    /// <summary>
    /// Task Functionality 
    /// </summary>
    public override async void TaskFunctionality()
    {
        Debug.Log("1");
        if (Input.GetMouseButton(0) && !wait)
        {
            
            // Checking whether we are clicking the right button 
            if (highlight.gameObject.GetComponent<GetElements>().SelectableID == mainButton)
            {
               
                highlight.gameObject.transform.localScale = Constants.scaleChanger;
                isClicking = true;
                if (isClicking)
                {

                    clickTimer += Time.deltaTime;
                 
                    if (clickTimer >= 3f && !color)
                    {
                        RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(14, Color.green, true, false, Color.white, false,false);
                        RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(15, Color.green, false, false, Color.white, false,false);
                        RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(16, Color.green, false, false, Color.white, false,false);
                        color = true;
                        secondTimer = true;
                        highlight.gameObject.transform.localScale = Constants.originalScale;
                        wait = true;
                    }

                }
            }
            else
            {
                Debug.Log("this is calling 1.3");
                wait = true;
                DelayToResetButton();
                OnSelectButtonListner(highlight.gameObject.GetComponent<GetElements>().SelectableID, false);
            }

        }
        if (secondTimer)
        {
            newTimer += Time.deltaTime;

            if (newTimer < 3 && Input.GetMouseButtonUp(0) && !step1)
            {
                highlight.gameObject.transform.localScale = Constants.originalScale;
             
                step1 = true;

            }
            if (step1 && Input.GetMouseButton(0) && newTimer < 3 && !step2)
            {
                if (highlight.gameObject.GetComponent<GetElements>().SelectableID == mainButton)
                {
                    highlight.gameObject.transform.localScale = Constants.scaleChanger;
                    step2 = true;
                    RefrenceManager.instance.questionManager.ledRefrence.StopFlashing(14);
                    RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(14, Color.green, false, false, Color.white, false,false);
                    RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(15, Color.green, true, false, Color.white, false,false);
                    secondTimer = false;
                    newTimer = 0;
                    ThirdTimer = true;
                   
                }
                else
                {
                    OnSelectButtonListner(highlight.gameObject.GetComponent<GetElements>().SelectableID, false);
                    secondTimer = false;
                    newTimer = 0;
                }
            }
            if (newTimer >= 4 && (!step2 || !step1))
            {
                highlight.gameObject.transform.localScale = Constants.originalScale;
               
                OnSelectButtonListner(highlight.gameObject.GetComponent<GetElements>().SelectableID, false);
                secondTimer = false;
                newTimer = 0;
            }



        }


        if (ThirdTimer)
        {
            newTimer += Time.deltaTime;
            
            if (newTimer < 3 && Input.GetMouseButtonUp(0) && !step3)
            {
                highlight.gameObject.transform.localScale = Constants.originalScale;
                step3 = true;

            }
            if (step3 && Input.GetMouseButton(0) && newTimer < 3 && !step4)
            {
                if (highlight.gameObject.GetComponent<GetElements>().SelectableID == mainButton)
                {
                   /* highlight.gameObject.transform.localScale = Constants.scaleChanger;*/
                    step4 = true;
                    RefrenceManager.instance.questionManager.ledRefrence.StopFlashing(15);
                    RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(15, Color.green, false, false, Color.white, false,false);
                    RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(16, Color.green, true, false, Color.white, false,false);
                    ThirdTimer = false;
                    fourthtime = true;
                    newTimer = 0;
                   

                }
                else
                {
                    OnSelectButtonListner(highlight.gameObject.GetComponent<GetElements>().SelectableID, false);
                    ThirdTimer = false;
                    newTimer = 0;
                }
            }

            if (newTimer >= 4 && (!step3 || !step4))
            {
                highlight.gameObject.transform.localScale = Constants.originalScale;
               
                OnSelectButtonListner(highlight.gameObject.GetComponent<GetElements>().SelectableID, false);
                ThirdTimer = false;
            }



        }

/*        if (fourthtime)
        {
            newfewTimer += Time.deltaTime;
            Debug.Log("timer newfew" + newfewTimer);
            if (newfewTimer >= 3f)
            {

                Debug.Log("thissiidsidisudifui");
                RefrenceManager.instance.locateTypeQuestions.ReasignDefaultTagsObjects();
                RefrenceManager.instance.locateTypeQuestions.HighlitObjectsSetToOff();
                highlight.gameObject.transform.localScale = Constants.originalScale;
                step6 = true;
                fourthtime = false;
                RefrenceManager.instance.questionManager.ledRefrence.StopFlashing(15);
                RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(9, Color.green, false, false, Color.white, false, false);
                RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(13, Color.green, false, false, Color.white, false, false);
                RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(16, Color.green, false, false, Color.white, false, false);
                RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(14, Color.gray, false, false, Color.white, false, false);
                RefrenceManager.instance.questionManager.ledRefrence.SetQUadColorDefault(14);
              await Task.Delay(1000);

                highlight.gameObject.transform.localScale = Constants.originalScale;
                OnSelectButtonListner(3, true);

            }

        } */  
/*            if (newTimer >= 4 && (!step5 || !step6))
            {
                highlight.gameObject.transform.localScale = Constants.originalScale;

                OnSelectButtonListner(highlight.gameObject.GetComponent<GetElements>().SelectableID, false);
                fourthtime = false;
            }*/


        

    }

    public void LateUpdate()
    {
        if (fourthtime)
        {
            newfewTimer += Time.deltaTime;
            Debug.Log("timer newfew" + newfewTimer);
            if (newfewTimer >= 3f &&!step6)
            {

                Debug.Log("thissiidsidisudifui");
                RefrenceManager.instance.locateTypeQuestions.ReasignDefaultTagsObjects();
                RefrenceManager.instance.locateTypeQuestions.HighlitObjectsSetToOff();
                highlight.gameObject.transform.localScale = Constants.originalScale;
                step6 = true;
                fourthtime = false;
                RefrenceManager.instance.questionManager.ledRefrence.StopFlashing(15);
                RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(9, Color.green, false, false, Color.white, false, false);
                RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(13, Color.green, false, false, Color.white, false, false);
                RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(16, Color.green, false, false, Color.white, false, false);
                RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(14, Color.gray, false, false, Color.white, false, false);
                RefrenceManager.instance.questionManager.ledRefrence.SetQUadColorDefault(14);
             

                highlight.gameObject.transform.localScale = Constants.originalScale;
                OnSelectButtonListner(3, true);

            }

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
            RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(16, Color.green, false, false, Color.white, false, false);
        }
        //  RefrenceManager.instance.uIManager.EnableTaskCompletedPanel();
        RefrenceManager.instance.questionManager.PlayCompleteSound();
        await Task.Delay(3000);

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




