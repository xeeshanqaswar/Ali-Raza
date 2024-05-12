using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsentManager : MonoBehaviour
{
    UIManager uiManager;
    private void Awake()
    {
        uiManager = RefrenceManager.instance.uIManager;
    }

    public void ContinuePress()
    {
        Constants.continueLastSession = 1;
        RefrenceManager.instance.progressManager.continueLastSession = true;
        uiManager.UserConsentPopupDisable();
        RefrenceManager.instance.loginScreen.LaunchGame();
        RefrenceManager.instance.progressManager.ContinueLastSession();
    }

    public void CancelPress()
    {
        RefrenceManager.instance.progressManager.StartNewSession();
        uiManager.UserConsentPopupDisable();
        uiManager.LoginScreenEnable();
        //RefrenceManager.instance.loginScreen.LaunchGame();
    }

    void Start()
    {
        
    }

}
