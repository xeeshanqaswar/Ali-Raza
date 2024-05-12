using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class oncamera : MonoBehaviour
{
    #region Private
    private Quaternion wantedRotation = Quaternion.identity;
    private Quaternion currentRotation = Quaternion.identity;
    private Quaternion collisionRot = Quaternion.identity;
    [HideInInspector]
    public Vector3 targetPosition, lastFollowerPosition, lastTargetPosition, collisionPos = Vector3.zero;
    private int direction = 1;
    private float orbitX = 0f;
    private float orbitY = 0f;
    private float orbitResetTimer = 0f;
    private bool useOrbitInTPSCameraMode = true;
    #endregion

    #region Public
    public GameObject player;
    public GameObject pivot;

    public float minOrbitY = -20f;
    public float maxOrbitY = 80f;

    public float orbitXSpeed = 7.5f;
    public float orbitYSpeed = 5f;

    private float playerSpeed = 0f;
    public static oncamera instance;

    public float TPSDistance;
    public float TPSHeight;
    public float TPSRotationDamping;

    public float CameraRotationSpeed;

    public bool ispressing = false;
    public Camera Camera;



    //public Camera mainCamera;
    public float zoomSpeed = 10.0f;
    public float zoomTime = 10.0f;
    public float minZoom = 1.0f;
    public float maxZoom = 10.0f;

    [SerializeField] bool isZooming = false;
    [SerializeField] bool zoomin = false;



    #endregion

    private void Awake()
    {
        Camera.rect = new Rect(0.47f, 0.0f, 1f, 1.0f);
        Debug.Log(Camera.rect.x);
        Camera.gameObject.SetActive(false);
        Camera.gameObject.SetActive(true);
        instance = this; 
        //ispressing = true;
    }

    private void Update()
    {
        orbitX += orbitXSpeed  * Time.deltaTime * CameraRotationSpeed * orbitDirection;
        if (Input.GetMouseButton(0))
        {

        }
        if (Input.GetMouseButton(1))
        {
            Zoom();
        }

    }


    float orbitDirection = 1f;
    public void OnDrag(PointerEventData pointerData)
    {
        orbitX += pointerData.delta.x * orbitXSpeed * .02f;
        orbitY -= pointerData.delta.y * orbitYSpeed * .02f;
        
        orbitResetTimer = 0f;
        ORBIT();
        ispressing = true;
        
        if (pointerData.delta.x > 0)
        {
            orbitDirection = 1f;
        }
        else
        {
            orbitDirection = -1f;
        }
        

    }

    /*
    public Transform vehicleRoot;
    public float x = 0;
    transform.RotateAround(vehicleRoot.position, Vector3.up, x* Time.deltaTime);*/
    private void LateUpdate()
    {
        

        if (ispressing)
        {
            wantedRotation = player.transform.rotation * Quaternion.AngleAxis((direction == 1 ? 220 : 180) + (useOrbitInTPSCameraMode ? orbitX : 0), Vector3.up);
            wantedRotation = wantedRotation * Quaternion.AngleAxis((useOrbitInTPSCameraMode ? orbitY : 0), Vector3.right);
            
            if (Time.time > 1)
                currentRotation = Quaternion.Lerp(currentRotation, wantedRotation, TPSRotationDamping * Time.deltaTime);
            else
                currentRotation = wantedRotation;

            // Rotates camera by Z axis for tilt effect.

            // Set the position of the camera on the x-z plane to distance meters behind the target.
            targetPosition = player.transform.localPosition;  
            targetPosition -= (currentRotation) * Vector3.forward * (TPSDistance * Mathf.Lerp(1f, .75f, (player.GetComponent<Rigidbody>().velocity.magnitude * 3.6f) / 100f));
            targetPosition += Vector3.up * (TPSHeight * Mathf.Lerp(1f, .75f, (player.GetComponent<Rigidbody>().velocity.magnitude * 3.6f) / 100f));

            transform.position = targetPosition;

           // this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, new Vector3(/*TPSTiltAngle*/5 / 10f, 0f, 0f), Time.deltaTime * 3f);

            // Always look at the target.
            transform.LookAt(player.transform);
            transform.eulerAngles = new Vector3(currentRotation.eulerAngles.x + (/*TPSPitchAngle*/15 * Mathf.Lerp(1f, .75f, (player.GetComponent<Rigidbody>().velocity.magnitude * 3.6f) / 100f)), transform.eulerAngles.y, -Mathf.Clamp(/*TPSTiltAngle*/0, -/*TPSTiltMaximum*/15, /*TPSTiltMaximum*/15) + /*TPSYawAngle*/0);

            // Past positions used for proper smooting related with speed.
            lastFollowerPosition = transform.position;
            lastTargetPosition = targetPosition;

            // Collision positions and rotations that affects pivot of the camera.
            collisionPos = Vector3.Lerp(new Vector3(collisionPos.x, collisionPos.y, collisionPos.z), Vector3.zero, Time.unscaledDeltaTime * 5f);
            
            if (Time.deltaTime != 0)
                collisionRot = Quaternion.Lerp(collisionRot, Quaternion.identity, Time.deltaTime * 5f);

            // Lerping position and rotation of the pivot to collision.
            pivot.transform.localPosition = Vector3.Lerp(pivot.transform.localPosition, collisionPos, Time.deltaTime * 10f);
            pivot.transform.localRotation = Quaternion.Lerp(pivot.transform.localRotation, collisionRot, Time.deltaTime * 10f);
        }

       

    }
    void ORBIT()
    {

        // Clamping Y.
        orbitY = Mathf.Clamp(orbitY, minOrbitY, maxOrbitY);

        wantedRotation = Quaternion.Euler(orbitY, orbitX, 0f);

        if (playerSpeed > 120f && Mathf.Abs(orbitX) > 1f)
            orbitResetTimer += Time.deltaTime;

        if (playerSpeed > 10f && orbitResetTimer >= 2f)
        {
            orbitX = 0f;
            orbitY = 0f;
            orbitResetTimer = 0f;

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
            Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, maxZoom, elapsedTime / zoomSpeed);
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }


        isZooming = false;
        zoomin = true;
    }
    IEnumerator ZoomOut()
    {
        isZooming = true;

        float elapsedTime = 0f;

        while (elapsedTime < zoomTime)
        {
            Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, minZoom, elapsedTime / zoomSpeed);
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }

        isZooming = false;
        zoomin = false;
    }
}
