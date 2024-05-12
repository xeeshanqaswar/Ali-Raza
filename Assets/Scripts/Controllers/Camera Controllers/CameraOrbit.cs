using System;
using System.Collections;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraOrbit : MonoBehaviour
{
    #region variables
    public Transform target;
    [SerializeField] CameraController cameraController;
    public bool isCameraContraint;
    public bool someConstraint;
    public float someContraintLimit = 50f; 
    [SerializeField] float resetSpeed = 3f;
    [SerializeField] GameObject camera;
    public float distance = 5f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
    public float smoothTime = 0.3f;

    public float yMinLimit = -20f;
    float defaultYMinLimit = -20f;
    float defaultYMaxLimit = 80f;
    public float yMaxLimit = 80f;

    public float xMinLimit = -360;
    public float xMaxLimit = 360;

    [SerializeField]private float _x = 0.0f;
    [SerializeField]private float _y = 0.0f;
    [SerializeField] private bool isReset = false;

    private float _xVelocity = 0.0f;
    private float _yVelocity = 0.0f;

    private Vector3 _mouseSpeed;
    private Vector3 _lastMousePosition;
    [HideInInspector] public Vector3 _initialPosition;
    [HideInInspector] public float _initialDistance;
    [HideInInspector] public Quaternion _initialRotation;

    public bool isRotateX = false;
    public bool isRotateY = false;
    public float lerpSpeed = 5f;

    #endregion


    private void Awake()
    {
        SetPositionAndRotation();
    }
    

    void Start()
    {
        // setting angles of default positions
        SettingAngles();

        // calculating initial transforms
        //_initialPosition = transform.position;
        //_initialRotation = transform.rotation;
        //_initialDistance = distance;

        //calculating initial transforms accroding to the default camera position
        _initialPosition = cameraController.defaultCameraPosition.position;
        _initialRotation = cameraController.defaultCameraPosition.rotation;
        _initialDistance = cameraController.defaultCameraPosition.position.z;

        
        defaultYMinLimit = yMinLimit;
        defaultYMaxLimit = yMaxLimit;
    }

    void LateUpdate()
    {
        RotateAroundObject();
    }



    /// <summary>
    /// Rotating the camera around object
    /// </summary>
    public void RotateAroundObject()
    {

        if (isCameraContraint)
        {
            Stop();
            return;
        }

       
        _mouseSpeed = _lastMousePosition - Input.mousePosition;
        if (target)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Stop();
            }

            if (Input.GetMouseButton(0) )
            {
                
                _xVelocity += xSpeed * Input.GetAxis("Mouse X") * 0.02f;
                _yVelocity += ySpeed * Input.GetAxis("Mouse Y") * 0.02f;
            }
            if (_xVelocity != 0 || _yVelocity != 0)
            {
                _xVelocity = Mathf.Lerp(_xVelocity, 0, Time.deltaTime * smoothTime);
                _yVelocity = Mathf.Lerp(_yVelocity, 0, Time.deltaTime * smoothTime);
            }
            else
            {
                return;
            }

            if (isRotateX)
            {
                _x += _xVelocity;
            }
            if (isRotateY)
            {
                _y -= _yVelocity;
            }

            _y = ClampAngle(_y, yMinLimit, yMaxLimit);
            _x = ClampAngle(_x, xMinLimit, xMaxLimit);

            var rotation = Quaternion.Euler(_y, _x, 0);
            var negDistance = new Vector3(0.0f, 0.0f, -distance);
            var position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }



        _lastMousePosition = Input.mousePosition;
    }


    /// <summary>
    /// Again set the position of the camera whenever camera moves
    /// </summary>
    public void SetPositionAndRotation()
    {
        SettingAngles();
        distance = Vector3.Distance(target.transform.position, camera.transform.position);
        distance -= 1.2f;
        Mathf.Abs(distance);
        var negDistance = new Vector3(0.0f, 0.0f, -distance);
        var position = transform.rotation * negDistance + target.position;
        transform.position = position;
        
    }

   /// <summary>
   /// calling coroutine to acces this from other scripts
   /// </summary>
   /// <param name="innitialTransform"></param>
   /// <param name="targetPosition"></param>
   /// <param name="newTargetRotation"></param>
    public void CallCoroutine(Transform innitialTransform, Vector3 targetPosition, Quaternion newTargetRotation)
    {
        StartCoroutine(TweeningCameraForCustomQuestions(innitialTransform, targetPosition, newTargetRotation));
    }


    /// <summary>
    /// Tweening the camera to custom point
    /// </summary>
    /// <param name="innitialTransform"></param>
    /// <param name="targetPosition"></param>
    /// <param name="newTargetRotation"></param>
    /// <returns></returns>
    IEnumerator TweeningCameraForCustomQuestions(Transform innitialTransform, Vector3 targetPosition, Quaternion newTargetRotation)
    {

        float elapsedTime = 0f;
        Vector3 startingPosition = innitialTransform.position;
        Quaternion startingRotation = innitialTransform.rotation;

        transform.DOMove(targetPosition, 1f);
        transform.DORotateQuaternion(newTargetRotation, 1f).OnComplete(()=> {


           // Debug.Log("Completed: ");
            ResetAfterRotation();

        });


        yield return null;
    }

    /// <summary>
    /// resetting the camera related things when camera reaches target position
    /// </summary>
    public void ResetAfterRotation()
    {

        cameraController.zoom.ResetZoom(cameraController.resetLerpSpeed);
        cameraController.isPositionReset = false;

        if (RefrenceManager.instance.questionManager.currentflag != null)
        {

            if (RefrenceManager.instance.questionManager.currentflag.resetButton.interactable == false)
            {
                RefrenceManager.instance.questionManager.currentflag.resetButton.interactable = true;
            }
        }
    }

    /// <summary>
    /// Set angles when camera reaches target
    /// </summary>
    public void SettingAngles()
    {
        Vector2 angles = transform.eulerAngles;
        float adjustedXRotation = (angles.x > 180f) ? angles.x - 360f : angles.x;
        float adjustedYRotation = (angles.y > 180f) ? angles.y - 360f : angles.y;
        _x = adjustedYRotation;
        _y = adjustedXRotation;
    }

    /// <summary>
    /// Stops camera on mouse click
    /// </summary>
    public void Stop()
    {
        _xVelocity = _yVelocity = 0f;
    }

    /// <summary>
    /// Reset zoom and rotation
    /// </summary>
    /// <param name="callback"></param>
    public void ResetPositionAndRotation(Action callback)
    {
        Stop();
        transform.DOMove(_initialPosition, resetSpeed);
        transform.DORotateQuaternion(_initialRotation, resetSpeed).OnComplete(() =>
        {
            distance = _initialDistance;
            SettingAngles();
            callback();
        });
    }

    /// <summary>
    /// Adds contraint to all constraint and some constraint questions
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    /// <summary>
    /// Reset Constraints for default questions
    /// </summary>
    public void ResetConstraints()
    {
        RefrenceManager.instance.outlineSelection.ResetButtonHandler(true);
        isCameraContraint = false;
        someConstraint = false;
        xMinLimit = -360;
        xMaxLimit = 360;
        yMinLimit = defaultYMinLimit;
        yMaxLimit = defaultYMaxLimit;
    }

    /// <summary>
    /// setting some constraints to some constraint questions
    /// </summary>
    public async void SettingSomeConstraints()
    {

        xMinLimit = _x - someContraintLimit;
        xMaxLimit = _x + someContraintLimit;
        ReverseAngles(_x, xMinLimit, xMaxLimit);
        await Task.Delay(1000);
        yMinLimit = _y - someContraintLimit;
        yMaxLimit = _y + someContraintLimit;
        ReverseAngles(_y, yMinLimit, yMaxLimit);
        someConstraint = true;
    }

    public bool GetCameraConstraintBool()
    {
        return isCameraContraint;
    }

    public bool GetSomeCameraConstraintBool()
    {
        return someConstraint;
    }


    /// <summary>
    /// reverse the angles when when angle less than 0
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void ReverseAngles(float angle, float min, float max)
    {
        if (angle < 0)
        {
            var temp = min;
            min = max;
            max = temp;
        }
    }

    /// <summary>
    /// setting up the speed slider
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeedOnChange(float speed)
    {
        xSpeed = speed;
        ySpeed = speed;
    }

    
}