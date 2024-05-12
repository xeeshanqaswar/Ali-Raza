using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotationAndZoom : MonoBehaviour
{
    public Transform target; // The target object to rotate around
    public float rotationSpeed = 2.0f;
    public float zoomSpeed = 2.0f;
    public float minZoomDistance = 2.0f;
    public float maxZoomDistance = 10.0f;

    private Transform cameraTransform;
    private Vector3 offset;

    private bool isRotating = false;
    private Vector3 lastMousePosition;

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        cameraTransform = transform;
        offset = cameraTransform.position - target.position;
        lastMousePosition = Input.mousePosition;
    }

    void Update()
    {
        HandleMouseInput();
        RotateCamera();
        Zoom();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isRotating = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isRotating = false;
        }
    }

    private void RotateCamera()
    {
        if (isRotating)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 mouseDelta = currentMousePosition - lastMousePosition;

            rotationX += mouseDelta.x * rotationSpeed;
            rotationY -= mouseDelta.y * rotationSpeed;

            rotationY = Mathf.Clamp(rotationY, -90, 90);

            Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);
            Vector3 desiredPosition = target.position + rotation * offset;

            cameraTransform.position = desiredPosition;
            cameraTransform.LookAt(target);
        }

        lastMousePosition = Input.mousePosition;
    }

    private void Zoom()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDelta != 0)
        {
            float zoomAmount = scrollDelta * zoomSpeed;
            Vector3 zoomDirection = cameraTransform.position - target.position;
            float newDistance = Mathf.Clamp(zoomDirection.magnitude - zoomAmount, minZoomDistance, maxZoomDistance);
            cameraTransform.position = target.position + zoomDirection.normalized * newDistance;
        }
    }


}
