using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{
    //public bool isLerping = false;
    [SerializeField] private string selectableTag;
    public bool isHover = false;
    public bool isHoverableQuestions = false;
    [SerializeField] private Transform endPointTransform;
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;
    private bool isInterpolating;
    [SerializeField] public Camera mainCamera;
    [SerializeField] CameraController cameraController;
    public NeoData neoData;
    [SerializeField] List <Transform> lerpPoints;
    [SerializeField] Zoom zoom;
    [SerializeField] float resetLerpSpeed;
    public Color defaultColor;

    float clickTimer = 0f;
    bool isClicking = false;
    bool isOptionSelected;

    private Transform selectedOption = null;
    private Transform lastHighlightedOption = null;

    public int hoverCount = 0;


    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    void Update()
    {
        #region new working code

        if (RefrenceManager.instance.uIManager.settingsScreen.activeSelf || RefrenceManager.instance.uIManager.endLessonPopup.activeSelf
                || RefrenceManager.instance.uIManager.endSessionPopup.activeSelf)
        {

        }
        else
        {

            if (highlight != null && highlight != selectedOption)
            {
                RemoveHover(highlight);
                highlight = null;
            }

            Ray ray = mainCamera.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                Transform newHighlight = raycastHit.transform;

                if (newHighlight.CompareTag(selectableTag) && newHighlight != highlight && isHover)
                {
                    if (newHighlight.gameObject.GetComponent<GetElements>() != null)
                    {
                        Hover(newHighlight);
                    }

                    if (Input.GetMouseButtonDown(0) && !isInterpolating)
                    {
                        isInterpolating = true;
                        isHover = false;
                    }
                }

                else if (newHighlight.CompareTag(Constants.tagforSelectableOption))
                {
                    if (newHighlight != highlight)
                    {

                        if (highlight != null && highlight != selectedOption)
                        {
                            RemoveHover(highlight);
                        }

                        if (selectedOption == null || selectedOption != newHighlight)
                        {
                            highlight = newHighlight;
                            Hover(highlight);
                        }
                    }

                    if (Input.GetMouseButtonDown(0) && !isInterpolating &&
                        !RefrenceManager.instance.questionManager.ledTaskQuestion)
                    {

                        if (selectedOption != null && selectedOption != newHighlight)
                        {
                            RemoveHover(selectedOption);
                        }

                        selectedOption = newHighlight;

                        if (selectedOption != null)
                        {
                            SelectOption(selectedOption);
                        }


                        RefrenceManager.instance.questionManager.question.OnOptionSelected
                                           (selectedOption.gameObject.GetComponent<GetElements>().SelectableID);

                        SoundManager.manager.ButtonSound();
                    }
                }

                if (isInterpolating)
                {
                    InterpolateToTarget();
                }
            }

        }

        #endregion

    }
    
    void InterpolateToTarget()
    {
        cameraController.CameraComponentHandler(false);
        cameraController.isPositionReset = true;
        //cameraTesting.enabled = false;
        int Id = highlight.GetComponent<GetElements>().Id;
        endPointTransform = lerpPoints[Id];
        float distance = Vector3.Distance(endPointTransform.localPosition, cameraController.target.position);
        
        RefrenceManager.instance.questionManager.currentflag.resetButton.interactable = false;

        isInterpolating = false;
        
        zoom.ZoomActionHandler(false);
    }

    public async Task InterpolateToTarget( int id, Action callback)
    {
       
        zoom.ZoomActionHandler(false);
        cameraController.CameraComponentHandler(false);
        cameraController.isPositionReset = true;
        endPointTransform = lerpPoints[id];
        ResetButtonHandler(false);
        cameraController.camOrbitComponent.CallCoroutine(cameraController.gameObject.transform, endPointTransform.position, endPointTransform.rotation);
        isInterpolating = false;
        callback();
        
    }

    public void ResetButtonHandler(bool isActive)
    {
        RefrenceManager.instance.uIManager.resetPosButton.interactable = isActive;
    }
    
    bool isHovered = false;
    
    public void HoverEffect()
    {
        if (!isHovered)
        {
            isHovered = true;
            var Element = highlight.gameObject.GetComponent<GetElements>();

            if (Element.hoverObjects.Length > 1)
            {
                for (int i = 0; i < Element.hoverObjects.Length; i++)
                {
                    Element.hoverObjects[i].GetComponent<MeshRenderer>().material = Element.hoverMaterial[i];
                    
                }
                return;
            }

            Element.hoverObjects[0].GetComponent<MeshRenderer>().material = Element.hoverMaterial[0];
        }
        
    }

    public void Hover(Transform option)
    {
        if (!isHovered)
        {
            isHovered = true;

            if (RefrenceManager.instance.uIManager.settingsScreen.activeSelf || RefrenceManager.instance.uIManager.endLessonPopup.activeSelf
                || RefrenceManager.instance.uIManager.endSessionPopup.activeSelf)
            {
                
            }
            else
            {
                GetElements obj = option.GetComponent<GetElements>();
                if (obj.hoverObjects.Length > 1)
                {
                    for (int i = 0; i < obj.hoverObjects.Length; i++)
                    {
                        obj.hoverObjects[i].GetComponent<MeshRenderer>().material = obj.hoverMaterial[i];

                    }
                    return;
                }

                obj.hoverObjects[0].GetComponent<MeshRenderer>().material = obj.hoverMaterial[0];
            }
        }
    }

    public void RemoveHover(Transform option)
    {
        if (isHovered)
        {
            isHovered = false;
            GetElements Element = option.GetComponent<GetElements>();

            if (Element.hoverObjects.Length > 1)
            {
                for (int i = 0; i < Element.hoverObjects.Length; i++)
                {
                    Element.hoverObjects[i].GetComponent<MeshRenderer>().material = Element.defaultMaterial[i];

                }
                return;
            }
            Element.hoverObjects[0].GetComponent<MeshRenderer>().material = Element.defaultMaterial[0];
        }
    }

    public void SelectOption(Transform option)
    {
        if (!isHovered)
        {
            isHovered = true;

            if (RefrenceManager.instance.uIManager.settingsScreen.activeSelf || RefrenceManager.instance.uIManager.endLessonPopup.activeSelf
                || RefrenceManager.instance.uIManager.endSessionPopup.activeSelf)
            {

            }
            else
            {

                GetElements obj = option.GetComponent<GetElements>();
                if (obj.hoverObjects.Length > 1)
                {
                    for (int i = 0; i < obj.hoverObjects.Length; i++)
                    {
                        obj.hoverObjects[i].GetComponent<MeshRenderer>().material = obj.hoverMaterial[i];

                    }
                    return;
                }

                obj.hoverObjects[0].GetComponent<MeshRenderer>().material = obj.hoverMaterial[0];
            }
        }
    }

    public void RemoveHoverEffect()
    {
        if (isHovered)
        {
            isHovered = false;
            var Element = highlight.gameObject.GetComponent<GetElements>();

            if (Element.hoverObjects.Length > 1)
            {
                for (int i = 0; i < Element.hoverObjects.Length; i++)
                {
                    Element.hoverObjects[i].GetComponent<MeshRenderer>().material = Element.defaultMaterial[i];
                    
                }
                return;
            }
            Element.hoverObjects[0].GetComponent<MeshRenderer>().material = Element.defaultMaterial[0];
        }
        
    }

}