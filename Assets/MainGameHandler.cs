using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainGameHandler : MonoBehaviour
{
    public GameObject screen;
    public GameObject informatin;
    public GameObject playerControllerInput;
    public static MainGameHandler _instance;
    public Text countdownText;

    public void Awake()
    {
        if (!_instance)
            _instance = this;
    }
    void Start()
    {
        FreezeMovements();
      
    }

  public async void StartCountdown()
    {

        await Task.Delay(1000);
        countdownText.gameObject.SetActive(true);
      
        // Countdown from 3 to 1
        for (int i = 3; i > 0; i--)
        {
            Debug.Log(i);
            countdownText.text = i.ToString(); // Update the Text component with the current countdown value
            await Task.Delay(1000);
            // Wait for 1 second before decrementing
        }

        // When the countdown is finished, perform the action
        PerformAction();
    }

    void PerformAction()
    {
        countdownText.gameObject.SetActive ( false);// Update the Text component with the current countdown value

        FreeMOvement();
    }


    public void FreezeMovements()
    {

        Debug.Log(" iam seteting ");
            playerControllerInput.GetComponent<PlayerInput>().enabled = false;
    

    }
    void DisableThePopup()
    {
        informatin.SetActive(false);

    }
  public  void EnablePopup()
    {
        informatin.SetActive(true);

    }

    public void okBtnListner()
    {
        DisableThePopup();
        MainGameHandler._instance.StartCountdown();
    }

    public void FreeMOvement()
    {

        playerControllerInput.GetComponent<PlayerInput>().enabled = true;

    }
}
