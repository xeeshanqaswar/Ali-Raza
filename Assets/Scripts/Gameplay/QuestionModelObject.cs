using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class QuestionModelObject : MonoBehaviour
{
    [SerializeField] ModelSide ModelCurrentSide;
    [SerializeField] List<GameObject> modelSidePoints;
    [SerializeField] List<RotationPoints> rotatePoints;
    [SerializeField] List<GameObject> options;
    [SerializeField] List<GameObject> optionsLookat;
    QuestionType qt;

    private void OnEnable()
    {
        EventsHandler._onEnableOption += EnableOption;
        EventsHandler._onDisableOption += DisableOptions;
        Init();
    }
    private void Init()
    {
        RotateModelAccordingToSide(ModelCurrentSide);
        DisableOptions();
    }
    private void OnDisable()
    {
        EventsHandler._onEnableOption -= EnableOption;
        EventsHandler._onDisableOption -= DisableOptions;
    }
    void RotateModelAccordingToSide(ModelSide _modelSide)
    {
        switch (_modelSide)
        {
            case ModelSide.None:
                break;
            case ModelSide.Front:
                transform.rotation = Quaternion.Euler(rotatePoints[0].rotateValues.x, rotatePoints[0].rotateValues.y, rotatePoints[0].rotateValues.z);
                break;
            case ModelSide.Top:
                transform.rotation = Quaternion.Euler(rotatePoints[1].rotateValues.x, rotatePoints[1].rotateValues.y, rotatePoints[1].rotateValues.z);
                break;
            case ModelSide.Bottom:
                transform.rotation = Quaternion.Euler(rotatePoints[2].rotateValues.x, rotatePoints[2].rotateValues.y, rotatePoints[2].rotateValues.z);
                break;
            case ModelSide.Left:
                transform.rotation = Quaternion.Euler(rotatePoints[3].rotateValues.x, rotatePoints[3].rotateValues.y, rotatePoints[3].rotateValues.z);
                break;
            case ModelSide.Right:
                transform.rotation = Quaternion.Euler(rotatePoints[4].rotateValues.x, rotatePoints[4].rotateValues.y, rotatePoints[4].rotateValues.z);
                break;
            case ModelSide.Rear:
                transform.rotation = Quaternion.Euler(rotatePoints[5].rotateValues.x, rotatePoints[5].rotateValues.y, rotatePoints[5].rotateValues.z);
                break;
            default:
                break;
        }
    }
    public void SelectAnswer()
    {

        Debug.Log("Answer");
        if (qt == QuestionType.ImageBased)
        {
            RefrenceManager.instance.questionManager.newQuestion.GetComponent<ImageQuestion>().OnImageSelectButtonListner();
        }
        else if(qt == QuestionType.FlagType)
        {
            RefrenceManager.instance.questionManager.newQuestion.GetComponent<FlagQuestions>().OnFlagSelectButtonListner();
        }
        else if(qt == QuestionType.LedType)
        {

            RefrenceManager.instance.questionManager.newQuestion.GetComponent<LedBasedQuestion>().OnSelectButtonListner();

            DisableOptions();
        }
        //Transit to next question
    }
   
    public void EnableOption(List<int> list)
    {
        DisableOptions();
        for (int i = 0; i < list.Count; i++)
        {
            options[list[i]].SetActive(true);
        }
        
    }
    void DisableOptions()
    {
        for (int i = 0; i < options.Count; i++)
        {
            options[i].SetActive(false);
        }
    }
    private void LateUpdate()
    {
        LookatCamera();
    }
    void LookatCamera()
    {
        //for (int i = 0; i < optionsLookat.Count; i++)
        //{
        //    optionsLookat[i].transform.LookAt(Camera_Controller.instance._camera.transform);
        //}
    }
}

[System.Serializable]
public class RotationPoints
{
    public ModelSide ModelSide;
    public Vector3 rotateValues;
}
