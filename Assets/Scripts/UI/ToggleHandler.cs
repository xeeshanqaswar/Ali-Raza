using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MiniJSON;

public class ToggleHandler : MonoBehaviour
{
    [SerializeField] Toggle toggle;
    RefrenceManager refrenceManager;
    [SerializeField] Image backgroundImage;
    [SerializeField] Color onColor;
    [SerializeField] RectTransform handleRect;
    Vector2 handlePosition;


    private void Awake()
    {
        refrenceManager = RefrenceManager.instance;
        toggle = GetComponent<Toggle>();
        handlePosition = handleRect.anchoredPosition;
        ToggleState();
        Switch();
    }

    public void Switch()
    {
        handleRect.DOAnchorPos(toggle.isOn ? handlePosition * -1f : handlePosition, 0.1f).
                                                                              SetEase(Ease.InOutBack);

        backgroundImage.DOColor(toggle.isOn ? onColor : Color.white, 0.1f);

        if (toggle.isOn)
        {
            refrenceManager.cameraController.BoundXY();
        }
        else
        {
            StartCoroutine(refrenceManager.cameraController.UnboundXY());
        }
    }

    bool ToggleState()
    {
        return !toggle.isOn;
    }

    public void OnSwitchLessonsToggle()
    {
        handleRect.DOAnchorPos(toggle.isOn ? handlePosition * -1f : handlePosition, 0.1f)
                                                                               .SetEase(Ease.InOutBack);
        
        backgroundImage.DOColor(toggle.isOn ? onColor : Color.white, 0.1f);

        if(toggle.isOn)
        {
            refrenceManager.gameManager.UnLockAllLevels();
        }
        else
        {
            refrenceManager.gameManager.LockAllLevels();
        }
    }

}
