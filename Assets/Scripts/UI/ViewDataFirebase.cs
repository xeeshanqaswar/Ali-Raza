using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewDataFirebase : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI lessonNumber;
    [SerializeField] TextMeshProUGUI questionNumber;
    [SerializeField] TextMeshProUGUI camerapPosition;
    [SerializeField] TextMeshProUGUI cameraRotation;
    Vector3 localPosition;
    Vector3 localRotation;
    public string id;


    private void Start()
    {
        
    }

    public void SetData(string lessonNo, string questionNo, Vector3 position, Vector3 rotation,string id)
    {
        this.id = id;
        lessonNumber.text = lessonNo;
        questionNumber.text = questionNo;
        camerapPosition.text = position.ToString();
        cameraRotation.text = rotation.ToString();
        localPosition = position;
        localRotation = rotation;
    }

    public void DeleteUser()
    {
        FirebaseController.DeleteUser(id, () =>
        {

            Debug.Log("Data deleted Succfully");
            FirebaseFetchPanel.RefreshPanelAction.Invoke();
        });

    }

    public void ViewPerspective()
    {
        /*RefrenceManager.instance.cameraController.SetPositionsOfCamera(lessonNumber.text,questionNumber.text,localPosition, localRotation);*/

    }


}
