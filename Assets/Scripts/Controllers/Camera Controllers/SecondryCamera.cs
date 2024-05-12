using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class SecondryCamera : MonoBehaviour
{

    public Transform target; // Assign the target object in the Unity Editor
    public float rotationSpeed = 5f;

    float horizontalInput;
    float verticalInput;

    float horizontalRotation;
    float verticalRotation;

    private bool isXMovement = false;
    private bool isYMovement = false;
    private bool isMouseDown = false;

    Vector2 angleForMouseDetection;

    public float zoomSpeed = 5f;
    public float minZoomDistance = 1f;
    public float maxZoomDistance = 10f;
    public float inertiaFactor = 0.95f;

    [SerializeField] CameraController cameraController;
    //SecondryCamera cameraTesting;
    public static Action<bool> CameraComponentHandler;
    

    private void Awake()
    {
        //cameraTesting = GetComponent<SecondryCamera>();
    }
    private void OnEnable()
    {
        CameraComponentHandler += CameraComponentHandle;
   
    }

    void Update()
    {
        // Get input for left-right and up-down movement
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            isXMovement = false;
            isYMovement = false;
            isMouseDown = true;
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            angleForMouseDetection.x = 0;
            angleForMouseDetection.y = 0;
            isMouseDown = false;
            
            
        }

        if (isMouseDown && !isXMovement && !isYMovement)
        {
            StartCoroutine(MouseDetection());
        }

        // Check if the left mouse button is released
      

        if (Input.GetMouseButton(0))
        {
            // Get input for left-right and up-down movement
            HandleRotation();
        }

        //MouseWheeling();
    }

    void HandleRotation()
    {
        horizontalInput = Input.GetAxis("Mouse X");
        verticalInput = Input.GetAxis("Mouse Y");


        // Calculate rotation angles based on input
        horizontalRotation = horizontalInput * rotationSpeed*inertiaFactor;
        verticalRotation = verticalInput * rotationSpeed*inertiaFactor;

        // Rotate the camera around the target object horizontally
        if (isXMovement)
        {
            transform.RotateAround(target.position, Vector3.up, horizontalRotation);
        }

        // Rotate the camera around the target object vertically
        if (isYMovement)
        {
            transform.RotateAround(target.position, transform.right, -verticalRotation); // Use negative value to invert vertical rotation
        }
    }


    IEnumerator MouseDetection()
    {
        angleForMouseDetection.x -= horizontalInput;
        angleForMouseDetection.y += verticalInput;

        yield return new WaitForSecondsRealtime(0.08f);

        if (Mathf.Abs(angleForMouseDetection.x) > Mathf.Abs(angleForMouseDetection.y))
        {
            isXMovement = true;
            isYMovement = false;
        }
        else
        {
            isYMovement = true;
            isXMovement = false;
        }

        

    }

    


    ///Zoom Functionality

    void ZoomCamera(float zoomInput)
    {
        // Calculate the new distance to the target
        Debug.Log("zooming : ");
        // Calculate the new distance to the target
        float newDistance = Vector3.Distance(transform.position, target.position) + zoomInput;

        // Clamp the distance to stay within the specified range
        newDistance = Mathf.Clamp(newDistance, minZoomDistance, maxZoomDistance);

        // Calculate the new position based on the new distance
        Vector3 newPosition = transform.position + transform.forward * (newDistance - Vector3.Distance(transform.position, target.position));

        // Apply the new position to the camera
        transform.position = newPosition;
    }

    void MouseWheeling()
    {
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheelInput > 0f)
        {
            ZoomIn();
        }
        else if (scrollWheelInput < 0f)
        {
            ZoomOut();
        }
    }

    private void ZoomIn()
    {
        ZoomCamera(-zoomSpeed);
    }
    private void ZoomOut()
    {
        ZoomCamera(zoomSpeed);
    }

    private void CameraComponentHandle(bool isActive)
    {
        //cameraTesting.enabled = isActive;
    }

    private void OnDestroy()
    {
        CameraComponentHandler -= CameraComponentHandle;
    }

}
