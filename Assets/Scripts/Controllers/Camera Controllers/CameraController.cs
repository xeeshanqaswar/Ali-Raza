using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;
using System.Threading.Tasks;

public class CameraController : MonoBehaviour
{

    /// <summary>
    /// Drop Down will be removed in next build
    /// </summary>

    #region Refrences
    RefrenceManager refrenceManager;
    public CameraOrbit camOrbitComponent;
    public bool isPositionReset = false;
    public Transform target; //Pivot to zoom
    Vector3 initialPosition; //Pivot to zoom
    Quaternion initialRotation; //Pivot to zoom
    [SerializeField] Vector2 angle; // rotations
    Vector2 initialAngle; // to store initial rotation
    [SerializeField] Vector3 offset; 
    public float distance = 6; //Distance
    float innitialDistance; //Distance
    [SerializeField] float distanceFromLeft = 5; //Distance
    [SerializeField] float distanceFromUp = 0.5f; //Distance
    [SerializeField] float lerpSpeed = 10.0f; // Speed of the angle lerp
    [SerializeField] float lerpSpeedDuplicate = 5.0f; //
    [SerializeField] float innitialLerpSpeed; // Speed of the angle lerp
    [SerializeField] float smoothness = 5.0f; // Smoothness of the lerping
    [SerializeField] float sensitivity = 2.5f; // Smoothness of the lerping
    float movementSpeed = 2.0f; // Smoothness of the lerping
    [SerializeField] float movementSpeedDuplicate = 2.0f;
    float rotationSpeed = 2.0f; // Smoothness of the lerping
    Quaternion targetRotation;
    [SerializeField] Vector3 cameraBoundsMin; // for orbit rotation bound
    [SerializeField] Vector3 cameraBoundsMax;

    [SerializeField] float ZoomDistanceValue = 4;
    [SerializeField] bool zoomInOut = false;
    [SerializeField] bool isZooming = false;
    [SerializeField] bool flag = false;
    bool IsResetting = false;
    [SerializeField] bool editorTesting = false;
    public Camera _camera;
    public float minZoomDistance = 2.0f;
    public float maxZoomDistance = 10.0f;
    private float currentZoomDistance;
    public static Action ResettingCamera;
    public static Action CameraTransition;

    public Transform cameraVideoPosition;
    public Transform defaultCameraPosition;

   
    //XY Bounds Variables
    [SerializeField]
    private bool IsXYBound;
    private bool isMouseDown = false;
    private bool isXMovement = false;
    private bool isYMovement = false;
    private bool resettingPosition;

    float mouseX;
    float mouseY;

    [SerializeField]
    private bool ZoomWithScroll;
    private float angleX;
    private float angleY;
    private Vector2 angleForMouseDetection;
    [SerializeField] private float inertia=1f;
    public Zoom zoom;
    //public DragRotate3DObject dragRotate3DObject;
    public float resetLerpSpeed;
    #endregion

    private void Awake()
    {
        refrenceManager = RefrenceManager.instance;
        CameraTransition += CameraMovementForImageQuestions;
        StartCoroutine(UnboundXY());

    }

    public void SetModelToVideoPosition()
    {
        transform.position = cameraVideoPosition.position;
        transform.rotation = cameraVideoPosition.rotation;

        //refrenceManager.questionManager.shouldResetPositions = true;

        RefrenceManager.instance.flagsHandler.UnraiseFlags();       //if last question was flag and user ends the lesson

        //zoom._targetZoom = zoom._defaultZoomValue;
        zoom.cam.fieldOfView = zoom.initialZoomValue;
        //zoom.cam.fieldOfView = zoom._defaultZoomValue;
    }


    private void Start()
    {
        //_camera.rect = new Rect(0.47f, 0f, 1f, 1f);
        Cursor.visible = true;
        innitialLerpSpeed = lerpSpeed;
        currentZoomDistance = Vector3.Distance(transform.position, target.position);
        innitialDistance = distance;
        initialAngle = angle;
        //OrbitAroundObject();
        initialPosition = transform.position;
        initialRotation = transform.rotation;

    }

    private void OnEnable()
    {
        ResettingCamera += ResetPositions;
        //dragRotate3DObject.enabled = true;
        //ResetPositions();
    }


    void OnDropdownValueChanged(int index)
    {
        switch (index)
        {
            case 0:
                BoundXY();
                break;

            case 1:
                StartCoroutine(UnboundXY());
                break;

        }
    }

    private void OnDestroy()
    {
        ResettingCamera -= ResetPositions;
        CameraTransition -= CameraMovementForImageQuestions;

    }

    public IEnumerator UnboundXY()
    {
        IsXYBound = false;
        yield return new WaitForSecondsRealtime(0.5f);
        camOrbitComponent.isRotateX = true;
        camOrbitComponent.isRotateY = true;
       
    }

    public void BoundXY()
    {
        IsXYBound = true;
        camOrbitComponent.isRotateX = false;
        camOrbitComponent.isRotateY = false;
    }

    public void Update()
    {
        
        if (IsXYBound)
        {
            GettingXYAxis();
        }

    }

    private bool isResetting = false;

    private void ResetCameraToInitialPosition()
    {
        if (!isResetting)
        {
            //isResetting = true;
            // StartCoroutine(LerpToInitialPosition(initialPosition, initialRotation, innitialDistance));
            //dragRotate3DObject.Stop();
            //transform.DOMove(initialPosition, resetLerpSpeed);
            //transform.DORotateQuaternion(initialRotation, resetLerpSpeed);

            zoom.ResetZoom(resetLerpSpeed);
            camOrbitComponent.ResetPositionAndRotation(()=> {

                 ReEnableCameraThings();

            });
            


        }
    }

    private async void  ReEnableCameraThings()
    {
        
        await Task.Delay(500);
        CameraComponentHandler(true);
        if (!refrenceManager.lessonScreen.isGameEnded)
        {
            Debug.Log("Zoom in enabled");
            zoom.ZoomActionHandler(true);
        }
        isPositionReset = false;
    }

    private IEnumerator LerpToInitialPosition(Vector3 targetPosition, Quaternion targetRotation, float targetDistance)
    {
        isResetting = true;
        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;
        Quaternion startingRotation = transform.rotation;

        while ((elapsedTime / lerpSpeed) < 1)
        {
            zoom.ResetZoom(elapsedTime / lerpSpeed);
            transform.position = Vector3.Lerp(transform.position, targetPosition, elapsedTime / lerpSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, elapsedTime / (lerpSpeedForRotation));
            distance = Mathf.Lerp(distance, targetDistance, elapsedTime / lerpSpeed);

            elapsedTime += Time.smoothDeltaTime;
            yield return new WaitForEndOfFrame();
        }

        // distance = targetDistance;
        // transform.position = targetPosition;
        // transform.rotation = targetRotation;
        isResetting = false;
    }


    private void LateUpdate()
    {
        //if (EventSystem.current.IsPointerOverGameObject())
        //{
        //    return;
        //}


        if (editorTesting)
            //OrbitAroundObject();

        

        if (Input.GetMouseButton(0))
        {
            
            //OrbitAroundObject();
        }

        if (Input.GetMouseButtonDown(0) && !isMouseDown)
        {
            isMouseDown = true;
            isXMovement = false;
            isYMovement = false;
        }

        if (isMouseDown && !isXMovement && !isYMovement)
        {
            if (IsXYBound)
            {
                StartCoroutine(MouseDetection());
            }
        }

        //
        // else if (Input.GetMouseButton(1))
        // {
        //     if (!ZoomWithScroll)
        //         Zoom();
        // }
        //
        //
        // if(ZoomWithScroll)
         /*  MouseWheeling();*/

        if (Input.GetMouseButtonUp(0))
        {
            angleForMouseDetection.x = 0;
            angleForMouseDetection.y = 0;

            isMouseDown = false;
        }

    }
    

    #region Orbit
    public void OrbitAroundObject()
    {
        // Update target rotation based on mouse input
        /*
                GettingXYAxes();
                Bound360Rotate();
                // Update the target rotation using lerp

                // XY Bounds

                if (IsXYBound && resettingPosition)
                {

                    if (isXMovement)
                    {
                        RotatingInXDirection();
                    }
                    else if (isYMovement)
                    {
                        RotatingInYDirection();

                    }


                }
                else 
                {

                    RotationWithoutXYBounds();

                }

                if (this.gameObject.activeInHierarchy)
                    StartCoroutine(LerpRotation(targetRotation));
                UpdateTransform();*/




        
        Bound360Rotate();

        // XY Bounds
        if (IsXYBound && resettingPosition)
        {
            
        }
        else
        {
            RotationWithoutXYBounds();
        }

        if (this.gameObject.activeInHierarchy)
        {
            //LerpRotationCoroutine(targetRotation);
            //transform.DORotateQuaternion(targetRotation,inertia);
        }

        UpdateTransform();





    }

    public void GettingXYAxis()
    {
        GettingXYAxes();
        if (isXMovement)
        {
            RotatingInXDirection();
        }
        else if (isYMovement)
        {
            RotatingInYDirection();
        }
    }


    public float roationlerp=0.2f;

    private async void LerpRotationCoroutine(Quaternion targetRot)
    {

        await LerpRotation(targetRot);
        
        transform.DORotateQuaternion(targetRot, roationlerp).SetEase(Ease.OutCirc);
       

    }

    private async Task LerpRotation(Quaternion targetRot)
    {
        Debug.Log(target);
        float elapsedtime = 0;

        while (elapsedtime < 1.0f)
        {
            elapsedtime += Time.fixedDeltaTime * smoothness;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, elapsedtime);
            await Task.Yield();
        }
    }

    /*    private void LerpRotationCoroutine(Quaternion targetRot)
        {
            StartCoroutine(LerpRotation(targetRot));
        }


        private IEnumerator LerpRotation(Quaternion targetRot)
        {
            Debug.Log(target);
           float elapsedtime = 0;
            while (elapsedtime < 1.0f)
            {
                elapsedtime += Time.fixedDeltaTime * smoothness;
               transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, elapsedtime / (lerpSpeedForRotationDuplicate));

                yield return null;
            }

        }*/


    void RotatingInXDirection()
    {
        angle.x -= mouseY;
        angleX = angle.x;
        angleForMouseDetection.y = 0;
        targetRotation = Quaternion.Euler(angle.x, angleY, 0);
       
    }

    void RotatingInYDirection()
    {
        angle.y += mouseX;
        angleY = angle.y;
        angleForMouseDetection.x = 0;
        targetRotation = Quaternion.Euler(angleX, angle.y, 0);
    }

    void RotationWithoutXYBounds()
    {
        angle.x -= mouseY;
        angle.y += mouseX;
        targetRotation = Quaternion.Euler(angle.x, angle.y, 0);
        angleX = angle.x;
        angleY = angle.y;
        resettingPosition = true;
    }

    void Bound360Rotate()
    {
        if (flag)
        {
            angle.x = Mathf.Clamp(angle.x, cameraBoundsMin.x, cameraBoundsMax.x);
            angle.y = Mathf.Clamp(angle.y, cameraBoundsMin.y, cameraBoundsMax.y);
        }
    }

    private void UpdateTransform()
    {
        Vector3 position = targetRotation * new Vector3(distanceFromLeft, distanceFromUp, -distance);
        
        if (!IsResetting)
        {
            transform.position = position;
          
            return;
        }

    }

    void GettingXYAxes()
    {
        mouseX = Input.GetAxis("Mouse X") * sensitivity;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity;
    }


    #endregion


    #region Zoom in/out
    void MouseWheeling()
    {
        if (!zoomInOut)
            return;
        Vector3 pos = transform.position;
        if (Input.GetAxis("Mouse ScrollWheel") < 0 )
        {

            StartCoroutine(SmoothZoomOut(innitialLerpSpeed));
          
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            
            StartCoroutine(SmoothZoomIn());

        }
        
    }



    [SerializeField] bool zoomin = false;
    void Zoom()
    {
        if (!zoomin)
        {

            if (!isZooming)
            {
                StartCoroutine(SmoothZoomIn());
            }
        }
        else
        {
            if (!isZooming)
            {
                StartCoroutine(SmoothZoomOut(innitialLerpSpeed));
            }
        }


        //OrbitAroundObject();
    }

    IEnumerator MouseDetection()
    {
        angleForMouseDetection.x -= mouseY;
        angleForMouseDetection.y += mouseX;

        yield return new WaitForSecondsRealtime(0.08f);

        if (Mathf.Abs(angleForMouseDetection.x) > Mathf.Abs(angleForMouseDetection.y))
        {
            isXMovement = true;
            isYMovement = false;
            //Debug.Log("Rotating in X: ");
            camOrbitComponent.isRotateX = false;
            camOrbitComponent.isRotateY = true;
            
            //dragRotate3DObject.RotateX = false;
            //dragRotate3DObject.RotateY = true;
        }
        else
        {
            isYMovement = true;
            isXMovement = false;
            //Debug.Log("Rotating in Y: ");
            camOrbitComponent.isRotateX = true;
            camOrbitComponent.isRotateY = false;

            //dragRotate3DObject.RotateX = true;
            //dragRotate3DObject.RotateY = false;
        }
    }
   
    private void UpdateTransformForZoomIn()
    {
        Vector3 position = targetRotation * new Vector3(distanceFromLeft, distanceFromUp, -distance + target.position.z) + offset;

        if (!IsResetting)
        {
            transform.position = position;
            return;
        }

        CoroutineForLerping(transform, initialPosition,initialRotation, initialAngle,innitialDistance);

    }

    public void CoroutineForLerping(Transform initialTransform, Vector3 targetPosition, Quaternion targetRotation, Vector2 targetAngle, float targetDistance)
    {
        StartCoroutine(LerpToInitialPosition(initialTransform, targetPosition, targetRotation, targetAngle, targetDistance));
    }

    public  float lerpSpeedForRotation = 0.495f;
    public float lerpSpeedForRotationDuplicate = 0.495f;

    IEnumerator LerpToInitialPosition(Transform innitialTransform, Vector3 targetPosition, Quaternion targetRotation, Vector2 targetAngle, float targetDistance)
    {
       
        float elapsedTime = 0f;
        Vector3 startingPosition = innitialTransform.position;
        Quaternion startingRotation = innitialTransform.rotation;
     
        while (elapsedTime < lerpSpeedDuplicate)
        {

            
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / lerpSpeedDuplicate);
            transform.rotation = Quaternion.Slerp(startingRotation, targetRotation, elapsedTime / (lerpSpeedForRotation));
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }
        distance = targetDistance;
        angle = targetAngle;
        //OrbitAroundObject();
        CameraComponentHandler(true);
        IsResetting = false;
    }

    private IEnumerator SmoothZoomIn()
    {
        isZooming = true;
        Debug.Log("Zooming In: ");
        float startDistance = distance;
        float targetDistance = distance - ZoomDistanceValue;  // Adjust this value to control zoom speed
        targetDistance = Mathf.Clamp(targetDistance, minZoomDistance, maxZoomDistance);  // Ensure it doesn't go below a minimum value (e.g., 1)
        
        Debug.Log("Target Distance"+ targetDistance);
        float elapsedTime = 0f;

        while (elapsedTime < lerpSpeed)
        {
            distance = Mathf.Lerp(startDistance, targetDistance, elapsedTime / lerpSpeed);
            UpdateTransformForZoomIn();
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        distance = targetDistance;
        //UpdateTransform();
        isZooming = false;
        zoomin = true;
        Debug.Log("last zoom in distance" + targetDistance);
    }

    private IEnumerator SmoothZoomOut(float lerpSpeedAnimation)
    {
        isZooming = true;
        float startDistance = distance;
        Debug.Log("Zooming Out: ");
        float targetDistance = distance + ZoomDistanceValue;  // Adjust this value to control zoom speed
        targetDistance = Mathf.Clamp(targetDistance, minZoomDistance, maxZoomDistance);  // Ensure it doesn't go below a minimum value (e.g., 1)
        float elapsedTime = 0f;
        lerpSpeed = lerpSpeedAnimation;
        while (elapsedTime < lerpSpeed)
        {
            distance = Mathf.Lerp(startDistance, targetDistance, elapsedTime / lerpSpeed);
            UpdateTransformForZoomIn();
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        distance = targetDistance;
        UpdateTransform();
        isZooming = false;
        zoomin = false;
        
    }
    
    #endregion

    public async void ResetPositions()
    {
        if (!isPositionReset && !camOrbitComponent.isCameraContraint && !camOrbitComponent.someConstraint)
        {
            isPositionReset = true;
            CameraComponentHandler(false);
            zoom.enabled = false;
            angle = initialAngle;
            distance = innitialDistance;
            IsResetting = true;
            resettingPosition = false;
            ResetCameraToInitialPosition();
            zoomin = false;
            if (refrenceManager.outlineSelection.isHoverableQuestions)
            {
                StartCoroutine(ActivatingHoverEffect());
            }
        }

        await Task.Delay(1500);

    }

    public bool GetIsPositionReset()
    {
        return isPositionReset;
    }

    IEnumerator ActivatingHoverEffect()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        refrenceManager.outlineSelection.isHover = true;
    }

    public void CameraMovementForImageQuestions()
    {
  
        if (distance <= innitialDistance)
        {

            distance += 0.5f;
            Vector3 targetPosition = new Vector3(distanceFromLeft, distanceFromUp, distance);
            transform.DORotateQuaternion(initialRotation, resetLerpSpeed);
            transform.DOMove(targetPosition, Constants.imageCorotuneDelay).SetEase(Ease.Linear);
        }


    }

    public void UpdatePositionsAfterHover(Transform hightledObject)
    {
        CameraComponentHandler(true);
        angle.x = hightledObject.rotation.x;
        angle.y = hightledObject.rotation.y;
        //target = hightledObject.transform;
        distance = Vector3.Distance(transform.position, hightledObject.position);
        //OrbitAroundObject();
    }

    public void CameraComponentHandler(bool isActive)
    {
        camOrbitComponent.enabled = isActive;
        GetComponent<CameraController>().enabled = isActive;

    }

    private void OnDisable()
    {
        //dragRotate3DObject.enabled = false;
    }
}
