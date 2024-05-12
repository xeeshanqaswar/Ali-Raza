using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    [SerializeField] Camera ModelCamera;


    Vector3 offset;
    public string destinationTag = "DropArea";


    void OnMouseDown()
    {
        Debug.Log("Mouse btutton Down: ");

        offset = transform.position - MouseWorldPosition();
        transform.GetComponent<Collider>().enabled = false;
    }

    void OnMouseDrag()
    {
        Debug.Log("Changing Positions: ");
        transform.position = MouseWorldPosition() + offset;
    }

    void OnMouseUp()
    {
        var rayOrigin = transform.position;
        var rayDirection = -Vector3.up;
        RaycastHit hitInfo;
        Debug.DrawRay(transform.position, rayDirection * 10f, Color.blue, 10000);
        if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo))
        {
            if (hitInfo.transform.tag == destinationTag)
            {
                Debug.Log("Hitting: ");
                transform.position = hitInfo.transform.position;
            }
        }


        transform.GetComponent<Collider>().enabled = true;
    }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = ModelCamera.WorldToScreenPoint(transform.position).z;
        return ModelCamera.ScreenToWorldPoint(mouseScreenPos);
    }
}