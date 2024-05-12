using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class LedTaskQ7 : LedTaskQuestion
{
    private bool color;

    private void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    public new void Update()
    {
        // overide the update function as its the same
        base.Update();

    }

    /// <summary>
    /// Task Functionlty
    /// </summary>
    public override async void TaskFunctionality()
    {
        if (Input.GetMouseButton(0) && !wait)
        {
                highlight.gameObject.transform.localScale = Constants.scaleChanger;
            // Checking whether we are clicking the right button 
            if (highlight.gameObject.GetComponent<GetElements>().SelectableID == mainButton)
            {
                isClicking = true;
                if (isClicking)
                {

                    clickTimer += Time.deltaTime;
                    
                    if (clickTimer >= 3f && !color)
                    {
                        Debug.Log("this is working");
                        RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(14, UnityEngine.Color.green, false, false, UnityEngine.Color.white, false,false);
                        RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(15, UnityEngine.Color.green, true, false, UnityEngine.Color.white, false,false);
                        RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(16, UnityEngine.Color.green, false, false, UnityEngine.Color.white, false,false);
                        color = true;
                        highlight.gameObject.transform.localScale = Constants.originalScale;
                        wait = true;
                        ColorChnageToOrignal();

                    }

                }
            }
            else
            {
                
                DelayToResetButton();
               
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            highlight.gameObject.transform.localScale = Constants.originalScale;
        }

    }
    public async void ColorChnageToOrignal()
    {
        await Task.Delay(6000);
        if (RefrenceManager.instance.taskCompleted) { return; }
        RefrenceManager.instance.questionManager.ledRefrence.StopFlashing(15);
        RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(14, UnityEngine.Color.gray, false, false, UnityEngine.Color.white, false,false);
        RefrenceManager.instance.questionManager.ledRefrence.SetQUadColorDefault(14);
        RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(15, UnityEngine.Color.green, false, false, UnityEngine.Color.white, false, false);
        RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(16, UnityEngine.Color.gray, false, false, UnityEngine.Color.white, false,false);
        RefrenceManager.instance.questionManager.ledRefrence.SetQUadColorDefault(16);

        wait = false;
        Debug.Log("this is calling");
        color = false;
        clickTimer = 0;

    }
    public void OnDisable()
    {
        RefrenceManager.instance.questionManager.ledRefrence.StopFlashing(15);
        RefrenceManager.instance.questionManager.ledRefrence.ChangeLedColor(15, UnityEngine.Color.gray, false, false, UnityEngine.Color.white, false, false);
      
        Debug.Log("asddddddddddddddddddddddddddd");

    }
   
}
