using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Orbit : MonoBehaviour
{
    [SerializeField] float sensitivity = 2.0f;
   
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    [SerializeField] bool rotationConstraint = false;
    public float maxRotation = 45.0f; // Maximum rotation angle
    public float minRotation = -45.0f; // Minimum rotation 


    //public Camera mainCamera;
    public float zoomSpeed = 10.0f;
    public float zoomTime = 10.0f;
    public float minZoom = 1.0f;
    public float maxZoom = 10.0f;

    [SerializeField] bool isZooming = false;
    [SerializeField] bool zoomin = false;
    void Update()
    {
        
        if (Input.GetMouseButton(0))
            OrbitAroundObject();

        if (Input.GetMouseButton(1))
        {
             Zoom();
        }
      
    }
    void OrbitAroundObject()
    {
        // Capture mouse input for rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calculate rotation angles
        rotationX += mouseY * sensitivity;
        rotationY -= mouseX * sensitivity;

        RotationConstraint();
        // Apply rotation to the GameObject
        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
    }
    void RotationConstraint()
    {
        if (rotationConstraint)
        {
            rotationX = Mathf.Clamp(rotationX, minRotation, maxRotation);
            rotationY = Mathf.Clamp(rotationY, minRotation, maxRotation);
        }
    }

    private void Zoom()
    {
        if (!zoomin)
        {
            if (!isZooming)
            {
                StartCoroutine(ZoomIn());
            }
        }
        else
        {
            if (!isZooming)
            {
                StartCoroutine(ZoomOut());
            }
        }
    }

    IEnumerator ZoomIn()
    {
        isZooming = true;
        
        float elapsedTime = 0f;

        while (elapsedTime < zoomTime)
        {
            Camera_Controller.instance._camera.fieldOfView = Mathf.Lerp(Camera_Controller.instance._camera.fieldOfView, maxZoom, elapsedTime / zoomSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
      

        zoomin = true;
        isZooming = false;
    }
    IEnumerator ZoomOut()
    {
        isZooming = true;

        float elapsedTime = 0f;

        while (elapsedTime < zoomTime)
        {
            Camera_Controller.instance._camera.fieldOfView = Mathf.Lerp(Camera_Controller.instance._camera.fieldOfView, minZoom, elapsedTime / zoomSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        zoomin = false;
        isZooming = false;
    }

    private void OnDisable()
    {
        ResetPositions();
    }

    public void ResetPositions()
    {

        rotationX = 0.0f;
        rotationY = 0.0f;

       transform.rotation = new Quaternion(0, 0, 0, 0);
        OrbitAroundObject();
        if (zoomin)
        {
            Camera_Controller.instance._camera.fieldOfView = minZoom;
            zoomin = false;
        }
    }
}
