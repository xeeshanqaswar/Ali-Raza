using System;
using System.Collections;
using DG.Tweening;
using Drag_Drop_Question;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class Zoom : MonoBehaviour
{
    #region variables
    [SerializeField] public Camera cam;
    bool isZoomEnabledForThisQiuestions = false;
    //minimum zoom value & maximum zoom value, change in one step
    [SerializeField] private float _minZoom, _maxZoom;
    [SerializeField] float zoomStep = 2;
    public float distance = 6;
    public float _targetZoom;
    [SerializeField] private float zoomLerpSpeed = 10f;
    public float _defaultZoomValue;
    public float initialZoomValue;
    private bool _isReseting = false;
    [SerializeField] float innitialLerpSpeed;
    [SerializeField] float lerpSpeed = 10.0f;
    [SerializeField] bool zoomInOut = false;
    [SerializeField] bool isZooming = false;
    public float minZoomDistance = 2.0f;
    public float maxZoomDistance = 10.0f;
    [SerializeField] float ZoomDistanceValue = 4;
    private bool _canZoom=true;

    #endregion

    private void Start()
    {
        //_defaultZoomValue = _targetZoom = cam.fieldOfView;
        _defaultZoomValue = _targetZoom = 72f;
        innitialLerpSpeed = lerpSpeed;
    }

    private void OnEnable()
    {
        DragModelPartsHandler.OnBeginDragModelParts += DisableZoom;
        DragModelPartsHandler.OnEndDragModelParts += EnableZoom;
    }

    private void OnDisable()
    {
        DragModelPartsHandler.OnBeginDragModelParts -= DisableZoom;
        DragModelPartsHandler.OnEndDragModelParts -= EnableZoom;
    }
    

    private void Update()
    {
        if(!_canZoom)
            return;
        ZoomValue();
    }


    private void EnableZoom()
    {
        _canZoom = true;
    }

    private void DisableZoom()
    {
        _canZoom = false;
    }

    private void ZoomValue()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scrollData) < 0.01f)
            return;

        _targetZoom -= scrollData * zoomStep;
        _targetZoom = Mathf.Clamp(_targetZoom, _minZoom, _maxZoom);
        
        // Use DOTween for smooth zooming over a specified duration
        float zoomDuration = 0.75f; // Adjust this value to control the speed of the zoom
        DOTween.To(() => cam.fieldOfView, (x) => cam.fieldOfView = x, _targetZoom, zoomDuration);
    }

    public void ResetZoom(float time)
    {

        GetComponent<Zoom>().enabled = false;
        DOTween.To(() => cam.fieldOfView, (x) => cam.fieldOfView = x, _defaultZoomValue, time).OnComplete(() =>ResetFildOfView());
    }

    void ResetFildOfView()
    {

        cam.fieldOfView = _targetZoom = _defaultZoomValue;
        if (isZoomEnabledForThisQiuestions)
        {
            GetComponent<Zoom>().enabled = true;
        }
    }

    /// <summary>
    /// enable/disable zoom component
    /// </summary>
    /// <param name="isActive"></param>
    public void ZoomActionHandler(bool isActive)
    {
        GetComponent<Zoom>().enabled = isActive;
        isZoomEnabledForThisQiuestions = isActive;
    }
    public void ZoomImplmentHandler(float _targetZoom=120)
    {

        DOTween.To(() => cam.fieldOfView, (x) => cam.fieldOfView = x, _targetZoom, 1f);

    }



}