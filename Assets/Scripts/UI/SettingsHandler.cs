using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsHandler : MonoBehaviour
{
    RefrenceManager _referenceManager;

    private void Awake()
    {
        _referenceManager = RefrenceManager.instance;
    }

    private void OnEnable()
    {
        _referenceManager.uIManager.ChangeCursorToDefault();
        _referenceManager.cameraorbit.enabled = false;
        _referenceManager.zoom.enabled = false;
        _referenceManager.cameraController.enabled = false;
    }

    private void OnDisable()
    {
        if (_referenceManager.questionManager.isMagnifyingCursorEnabled)
        {
            _referenceManager.uIManager.ChangeCursorToMagnifyingGlass();
        }
        else if (_referenceManager.questionManager.isFingerCursorEnable)
        {
            _referenceManager.uIManager.ChangeCursorToFinger();
        }
        else
        {
            _referenceManager.uIManager.ChangeCursorToDefault();
        }


        for (int i = 0; i < RefrenceManager.instance.questionManager.questionsWithCorrectOptions.Count; i++)
        {
            if (RefrenceManager.instance.questionManager.questionsWithCorrectOptions[i].question == "Task: Connect the antennas ")
            {
                RefrenceManager.instance.cameraorbit.enabled = false;
                RefrenceManager.instance.zoom.enabled = false;
                RefrenceManager.instance.cameraController.enabled = false;
            }
            else
            {
                RefrenceManager.instance.cameraorbit.enabled = true;
                RefrenceManager.instance.zoom.enabled = true;
                RefrenceManager.instance.cameraController.enabled = true;
            }
        }
    }
}
