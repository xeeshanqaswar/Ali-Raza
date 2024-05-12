using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static NeoDataLocal;

public class LedRefrence : MonoBehaviour
{

    public List<Renderer> leds;
    public NeoDataLocal neoDataLocal;
    public bool isFlashingLocal = true;
    static UnityEngine.Color defaultColor = UnityEngine.Color.grey;
 [HideInInspector]  public  UnityEngine.Color transparentColor = new UnityEngine.Color(defaultColor.r, defaultColor.g, defaultColor.b, 0f);

    public UnityEngine.Color orangeEmission;

    [System.Serializable]
    public class BandLeds
    {
        public GameObject red;
        public GameObject yellow;
        public GameObject green;
    }

    private void Start()
    {
        //ChangeToDefaultColor();
    }

    public void ChangeLedColor(int index, UnityEngine.Color color, bool isFlashing, bool isChangingColor, UnityEngine.Color changingColor, bool isBackAndForth, bool changingEmission)
    {
        isFlashingLocal = true;

        #region debug code



/*        Transform ledTransform = leds[index].gameObject.transform;

        if (ledTransform != null && ledTransform.childCount > 0)
        {
            Debug.LogError("lED TRANSFORM IS NOT NULL."+index);
            // The Transform is valid, and there is at least one child
            Renderer childRenderer = ledTransform.GetChild(0).GetComponent<Renderer>();

            if (childRenderer != null)
            {
                // The Renderer component is valid, proceed with the code
                childRenderer.material.color = color;
            }
            else
            {
                Debug.LogError("Child GameObject does not have a Renderer component.");
            }
        }
        else
        {
            Debug.LogError("Invalid Transform or no child GameObjects."+index );
        }*/
#endregion



        leds[index].material.color = color;
        SetQuadMaterialColor(color, index);
        if (changingEmission)
        {
  
            leds[index].material.SetColor("_EmissionColor", orangeEmission);
            SetQuadEmmision(orangeEmission, index);
        }
        else
        {
            leds[index].material.SetColor("_EmissionColor", color);
            SetQuadEmmision(orangeEmission, index);
        }


        if (isFlashing)
        {
            if (isBackAndForth)
            {
                backAndForthDelay += 0.2f;

                StartCoroutine(FlashingBackAndForth(index, color));

                return;
            }
           
            StartCoroutine(FlashingLeds(index, color, 0.5f,changingColor, isChangingColor));
            
        }

    }
    public void StopAllFlashing()
    {
        foreach (var led in leds)
        {
            led.material.color = transparentColor;
            led.material.SetColor("_EmissionColor", transparentColor);
            led.transform.GetChild(0).GetComponent<Renderer>().material.color = transparentColor;
            led.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", transparentColor);
        }
        isFlashingLocal = false;
        StopAllCoroutines();
    }
    public void StopFlashing(int index)
    {
 
        isFlashingLocal = false;
        backAndForthDelay = 0f;
        leds[index].material.color = transparentColor;
        SetQuadEmmision(transparentColor, index);
        SetQuadMaterialColor(transparentColor, index);
        leds[index].material.SetColor("_EmissionColor", transparentColor);
        StopAllCoroutines();
    }

    public async Task ChangeToDefaultColor()
    {
        isFlashingLocal = false;
        backAndForthDelay = 0f;
        foreach (var led in leds)
        {
            led.material.color = transparentColor;
            led.transform.GetChild(0).GetComponent<Renderer>().material.color = transparentColor;
            led.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", transparentColor);
            led.material.SetColor("_EmissionColor", transparentColor);
        }
    }


    IEnumerator FlashingLeds(int index, UnityEngine.Color color, float delay, UnityEngine.Color changingColor, bool isChangingColor)
    {
        UnityEngine.Color newChangingColor = new UnityEngine.Color(changingColor.r, changingColor.g, changingColor.b, 0.05f);

        if (isFlashingLocal)
        {
            Debug.Log("falshing is on ");
            if (isChangingColor)
            {
                SetQuadMaterialColor(newChangingColor, index);
                leds[index].material.color = newChangingColor;
                leds[index].material.SetColor("_EmissionColor", orangeEmission);
                SetQuadEmmision(orangeEmission, index);
            }
            else
            {
                leds[index].material.color = UnityEngine.Color.grey;
                SetQuadMaterialColor(transparentColor, index);
                leds[index].material.SetColor("_EmissionColor", transparentColor);
                SetQuadEmmision(transparentColor, index);
            }

            yield return new WaitForSecondsRealtime(delay);
            leds[index].material.color = color;
            SetQuadMaterialColor(color, index);
            SetQuadEmmision(color, index);
            leds[index].material.SetColor("_EmissionColor", color);
            yield return new WaitForSecondsRealtime(delay);

            StartCoroutine(FlashingLeds(index, color, delay, newChangingColor, isChangingColor));
        }

        else
        {
            leds[index].material.color = transparentColor;
            leds[index].material.SetColor("_EmissionColor", transparentColor);
            SetQuadMaterialColor(transparentColor, index);
            SetQuadEmmision(transparentColor, index);
        }

    }

    float backAndForthDelay = 0f;
    IEnumerator FlashingBackAndForth(int index, UnityEngine.Color color)
    {
        
        if (isFlashingLocal)
        {
            leds[index].material.color = transparentColor;
            SetQuadMaterialColor(transparentColor, index);
            SetQuadEmmision(transparentColor, index); 
            leds[index].material.SetColor("_EmissionColor", transparentColor);
            yield return new WaitForSecondsRealtime(backAndForthDelay);
            leds[index].material.color = color;
            leds[index].transform.GetChild(0).GetComponent<Renderer>().material.color = color;
            SetQuadEmmision(color, index);
            SetQuadMaterialColor(color, index);
            leds[index].material.SetColor("_EmissionColor", color);
            yield return new WaitForSecondsRealtime(backAndForthDelay);
            StartCoroutine(FlashingBackAndForth(index, color));

        }
        else
        {
            leds[index].material.color = transparentColor;
            SetQuadMaterialColor(transparentColor, index);
            SetQuadEmmision(transparentColor, index);
            leds[index].material.SetColor("_EmissionColor", transparentColor);
           
        }

    }


    public void SetQuadEmmision(UnityEngine.Color color,int index)
    {
        leds[index].transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
    }

    public void SetQuadMaterialColor(UnityEngine.Color color, int index)
    {

       /* Debug.Log("Changing Quad colors: ");*/
        UnityEngine.Color modifiedColor = new UnityEngine.Color(color.r, color.g, color.b, 0.05f);
        leds[index].transform.GetChild(0).GetComponent<Renderer>().material.color = modifiedColor;


    }

    public void SetQUadColorDefault(int index) {

        leds[index].transform.GetChild(0).GetComponent<Renderer>().material.color = transparentColor;
        leds[index].transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", transparentColor);
    }



}
