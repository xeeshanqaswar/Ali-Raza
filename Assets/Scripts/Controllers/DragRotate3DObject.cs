using System.Collections;
using System.Collections.Generic;
using Drag_Drop_Question;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragRotate3DObject : MonoBehaviour
{
    /// <summary>
    /// script current not in use will be deleted later
    /// </summary>


    [SerializeField] private Camera _camera;
    [Header("Speed")] public float RotationSpeed = 10f;

    [Header("X")] public bool RotateX = true;
    public bool InvertX = false;

    private int _xMultiplier
    {
        get { return InvertX ? -1 : 1; }
    }

    [Header("Y")] public bool RotateY = true;
    public bool InvertY = false;

    private int _yMultiplier
    {
        get { return InvertY ? -1 : 1; }
    }

    [Header("Inertia")] public bool UseInertia = false;
    public float SlowSpeed = 1f;
    
    [SerializeField]private bool beingDragged = false;

    private Vector3 speed = Vector3.zero;
    private Vector3 averageSpeed = Vector3.zero;

    private Vector2 lastMousePosition = Vector2.zero;

    private bool _canDrag=true;
    private void OnEnable()
    {
        DragModelPartsHandler.OnBeginDragModelParts += DisableDragToRotate;
        DragModelPartsHandler.OnEndDragModelParts += EnableDragToRotate;
    }

    private void OnDisable()
    {
        DragModelPartsHandler.OnBeginDragModelParts -= DisableDragToRotate;
        DragModelPartsHandler.OnEndDragModelParts -= EnableDragToRotate;
    }
    void Update()
    {
        if(!_canDrag)
            return;
/*        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;

        }*/
        if (Input.GetMouseButtonDown(0))
        {
            speed = averageSpeed = Vector3.zero;
        }
        if (Input.GetMouseButton(0))
        {
            beingDragged = true;
        }
        if (Input.GetMouseButton(0) && beingDragged&&!EventSystem.current.IsPointerOverGameObject())
        {
            var mouseX = Input.GetAxis("Mouse X");
            var mouseY = Input.GetAxis("Mouse Y");
            var mouseDelta = (new Vector2(mouseX,mouseY)) * 100;
            mouseDelta.Set(mouseDelta.x / Screen.width, mouseDelta.y / Screen.height);

            //speed = new Vector3((-Input.GetAxis("Mouse X")) * _xMultiplier, (Input.GetAxis("Mouse Y")) * _yMultiplier, 0.0F);
            speed = new Vector3(-mouseDelta.x * _xMultiplier, mouseDelta.y * _yMultiplier, 0);
            averageSpeed = Vector3.Lerp(averageSpeed, speed, Time.deltaTime * 5);
        }
        else
        {
            if (beingDragged)
            {
                speed = averageSpeed;
                beingDragged = false;
            }

            if (UseInertia)
            {
                float i = Time.deltaTime * SlowSpeed;
                speed = Vector3.Lerp(speed, Vector3.zero, i);
            }
            else
            {
                speed = Vector3.zero;

            }
        }

        if (speed != Vector3.zero)
        {
            if (RotateX)
                transform.Rotate(_camera.transform.up , (speed.x * RotationSpeed), Space.World);
            if (RotateY)
                transform.Rotate(_camera.transform.right , (speed.y * RotationSpeed), Space.World);

        }

        lastMousePosition = Input.mousePosition;
    }
    private void EnableDragToRotate()
    {
        _canDrag = true;
    }
    private void DisableDragToRotate()
    {
        _canDrag = false;
    }

    public void Stop()
    {
        speed = Vector3.zero;
    }



}