using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EmptyQuestionScreen : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    UIManager uimanager;
    private void Awake()
    {
        uimanager = RefrenceManager.instance.uIManager;
    }

    public void SetTitle(string title)
    {
        titleText.text = title;
    }

}
