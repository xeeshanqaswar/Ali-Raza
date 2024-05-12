using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPopupHandler : MonoBehaviour
{
    RefrenceManager refrenceManager;

    private void Awake()
    {
        refrenceManager = RefrenceManager.instance;
    }

    /// <summary>
    /// onclick any where else popup should go away
    /// </summary>
    public void OnclickAnyWhereElse()
    {
        refrenceManager.uIManager.exitPopup_Questions.SetActive(false);
        refrenceManager.uIManager.exitPopup_Login.SetActive(false);
        refrenceManager.uIManager.exitPopup_MainMenu.SetActive(false);
        gameObject.SetActive(false);
    }

    public void EnableObject()
    {
        if(!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
