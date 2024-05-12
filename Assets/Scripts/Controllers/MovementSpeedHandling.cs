using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MovementSpeedHandling : MonoBehaviour
{
    #region

    public Slider speedSlider;
    public TextMeshProUGUI speedText;
    int speedValue;

    #endregion

    private void Start()
    {
        //Setting the last saved speed value

        if (!PlayerPrefs.HasKey("SpeedValue"))
        {
            
            speedSlider.value = 0.45f;
            speedValue = (int)(speedSlider.value * 100);
            speedText.text = speedValue.ToString();
            PlayerPrefs.SetFloat("SpeedValue", speedValue);
        }
        else
        {
            
            speedSlider.value = PlayerPrefs.GetFloat("SpeedValue")/100;
            speedValue = (int)(speedSlider.value);
            speedText.text = speedValue.ToString();
        }

        ChanedValue();


    }

    /// <summary>
    /// On speed slider value changed
    /// </summary>
    public void ChanedValue()
    {
        int newSpeed = (int)(speedSlider.value * 100);
        speedText.text = newSpeed.ToString();
        RefrenceManager.instance.cameraController.camOrbitComponent.SetSpeedOnChange(newSpeed);
        PlayerPrefs.SetFloat("SpeedValue", newSpeed);
    }


}
