using DG.Tweening;
using Drag_Drop_Question;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class ModelPartsHandler : MonoBehaviour
{
    public List<GameObject> dragableObjects;
    public List<Vector3> dragablesPosition;
    public CameraOrbit orbit;
    public bool antena1;
    public bool antena1CorrectPosition;
    public bool antena2;
    public bool antena2CorrectPosition;
    public bool antena3;
    public bool antena3CorrectPosition;
    public Camera defaultCamera;
    public Camera dragCamera;
    public bool slot1;
    public bool slot2;
    public bool slot3;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetDraggableState() {

        AssignDiffrentTagsObjects();
        SetDraggableObjectPostion();
        SetCamera();
        SetupTheSetting();

    }

    public void ResetToNormalState()
    {

        ResetSetting();
        ReAssignDiffrentTagsObjects();
        ResetToOrignalPosition();
        ResetCamera();
    }


    public void SetupTheSetting()
    {

        orbit.enabled = false;  

    }

    public void ResetSetting()
    {

        orbit.enabled = true;

    }

    
    public void AssignDiffrentTagsObjects()
    {

        for (int i = 0; i < dragableObjects.Count; i++) {

         
         /*   dragableObjects[i].gameObject.layer = LayerMask.NameToLayer( Constants.tagforDrageOption);*/
            dragableObjects[i].gameObject.GetComponent<DragObject>().enabled = true;
            dragableObjects[i].gameObject.AddComponent<Rigidbody>().useGravity = false;
            dragableObjects[i].gameObject.GetComponent<BoxCollider>().enabled = true;
        }

    }
    public void ReAssignDiffrentTagsObjects()
    {
        for (int i = 0; i < dragableObjects.Count; i++)
        {
           /* dragableObjects[i].gameObject.layer = LayerMask.NameToLayer(Constants.tagforDrageDefault);*/
            dragableObjects[i].gameObject.GetComponent<DragObject>().enabled = false;
            Destroy( dragableObjects[i].gameObject.GetComponent<Rigidbody>());
            dragableObjects[i].gameObject.GetComponent<BoxCollider>().enabled = false;

        }
    }

    
    public void SetDraggableObjectPostion()
    {
        for (int i = 0; i < dragableObjects.Count; i++) {
           Transform currentTransform = dragableObjects[i].gameObject.transform;
            Vector3 targetPosition = dragablesPosition[i];

            // Use DoTween to smoothly move the object to the target position
            currentTransform.DOMove(targetPosition, .5f).SetEase(Ease.Linear);

          //  dragableObjects[i].gameObject.transform.position = dragablesPosition[i];

        }

    }
    
    public void ResetToOrignalPosition()
    {
        for (int i = 0; i < dragableObjects.Count; i++)
        {

            dragableObjects[i].gameObject.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    public void SetCamera()
    {
        defaultCamera.gameObject.SetActive(false);
        dragCamera.gameObject.SetActive(true);
    }

    public void ResetCamera()
    {
        defaultCamera.gameObject.SetActive(true);
        dragCamera.gameObject.SetActive(false);

    }
   

    #region Select Button States

    public void CheckAndEnableSelectButton()
    {

        if (slot1&& slot2 && slot3)
        {

          
            RefrenceManager.instance.uIManager.SelectBtnBlocker(false);

        }
        else {
           

            RefrenceManager.instance.uIManager.SelectBtnBlocker(true); ;
        }

    
    
    }

    public void AssignASlot(int number)
    {
        if (number == 1)
        {
            slot1 = true;
        }
        if (number == 2)
        {
            slot2 = true;
        }
        if (number == 3)
        {
            slot3 = true;
        }
        CheckAndEnableSelectButton();

    }

    public void SettingSlotfree(int number)
    {
        if (number == 1 && slot1)
            slot1 = false;

        if (number == 2 && slot2)
            slot2 = false;

        if (number == 3 && slot3)
            slot3 = false;

    }

    public bool CheckSlot(int number)
    {
        if (number == 1 && !slot1)
            return true;

        if (number == 2 && !slot2)
            return true;

        if (number == 3 && !slot3)
            return true;

        return false;

    }

    public void AddSelection(int number, bool iscorrect = false)
    {

        if (number == 1)
        {
            if (!antena1)
            {
                antena1 = true;
            }
            antena1CorrectPosition = iscorrect;
        }
        if (number == 2)
        {
            if (!antena2)
            {

                antena2 = true;
            }
            antena2CorrectPosition = iscorrect;
        }
        if (number == 3)
        {
            if (!antena3)
            {
                antena3 = true;
            }
            antena3CorrectPosition = iscorrect;
        }

        CheckAndEnableSelectButton();
    }

    public void RemoveSelection(int number)
    {

        if (number == 1) { if (antena1) antena1 = false; antena1CorrectPosition = false; ; }
        if (number == 2) { if (antena2) antena2 = false; antena2CorrectPosition = false; }
        if (number == 3) { if (antena3) antena3 = false; antena3CorrectPosition = false; }
        CheckAndEnableSelectButton();
    }

    public bool VerifyAnswers()
    {
        return antena1CorrectPosition && antena2CorrectPosition && antena3CorrectPosition;
    }

    #endregion


}
