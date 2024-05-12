using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Drag_Drop_Question
{
    public class DragObject : MonoBehaviour
    {
        public bool isDragging = false;
        public Vector3 offset;
        public Camera mainCamera;
        public float snapValue = 0.01f;
        public float smoothTime = 0.1f;
        public float zMoveSpeed = .5f;
        public Transform[] transforms;
        public List<Vector3> positions;
        public bool isSelected;
         [SerializeField]     bool slot1;
        [SerializeField] bool slot2;
        [SerializeField] bool slot3;


        public Vector3 initialPosition;
        public List<Collider> correctPositionColliders;  // Colliders for correct positions
        public int number;
        public GameObject currentCorrectPositionCollider;

        void Start()
        {
            foreach (Transform correctPosition in transforms)
            {

             //   correctPositionColliders.Add(correctPosition.gameObject.GetComponent<BoxCollider>());
                 // Add a tag to distinguish correct positions
            correctPosition.tag = "CorrectPosition";
               
            }

        }

        Vector3 originalpostion;
        
        void OnMouseDown()
        {
            if (this.enabled)
            {


                if (RefrenceManager.instance.uIManager.settingsScreen.activeSelf || RefrenceManager.instance.uIManager.endLessonPopup.activeSelf
                    || RefrenceManager.instance.uIManager.endSessionPopup.activeSelf)
                {

                }
                else
                {
                    originalpostion = transform.localPosition;
                    offset = transform.position - GetMouseWorldPos();
                    isDragging = true;
                }
            }
        }

        void OnMouseDrag()
        {
            if (this.enabled)
            {
                if (RefrenceManager.instance.uIManager.settingsScreen.activeSelf || RefrenceManager.instance.uIManager.endLessonPopup.activeSelf
                    || RefrenceManager.instance.uIManager.endSessionPopup.activeSelf)
                {

                }
                else
                {
                    if (isDragging)
                    {
                        Vector3 targetPosition = GetMouseWorldPos() + offset;
                        // Snap to grid for X and Y axes
                        targetPosition.x = Mathf.Round(targetPosition.x / snapValue) * snapValue;
                        targetPosition.y = Mathf.Round(targetPosition.y / snapValue) * snapValue;
                        // Keep the current Z position
                        targetPosition.z = transform.position.z;
                        // Smoothly move towards the target position
                        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothTime);
                    }
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("CorrectPosition"))
            {
         
                currentCorrectPositionCollider = other.gameObject;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("CorrectPosition"))
            {
                FreeLocalSlot();
                isSelected = false;
                RefrenceManager.instance.modelPartsHandler.RemoveSelection(number);
                currentCorrectPositionCollider = null;
            }
        }

        void FreeLocalSlot()
        {
           
            if (slot1) RefrenceManager.instance.modelPartsHandler.SettingSlotfree(1);
            if (slot2) RefrenceManager.instance.modelPartsHandler.SettingSlotfree(2);
            if (slot3) RefrenceManager.instance.modelPartsHandler.SettingSlotfree(3);
            SettingLocalSlotFree();
        }

      void SettingLocalSlotFree()
        {

           slot1=false; slot2=false; slot3=false;

        }

        void OnMouseUp()
        {
            if (this.enabled) {
                if (RefrenceManager.instance.uIManager.settingsScreen.activeSelf || RefrenceManager.instance.uIManager.endLessonPopup.activeSelf
                    || RefrenceManager.instance.uIManager.endSessionPopup.activeSelf)
                {

                }
                else
                {
                    isDragging = false;
                    if (currentCorrectPositionCollider != null)
                    {
                        isSelected = true;
                        //   RefrenceManager.instance.modelPartsHandler.AddSelection(number);
                        SoundManager.manager.ButtonSound();
                        if (currentCorrectPositionCollider.gameObject.name == "Collider1")
                        {
                            if (RefrenceManager.instance.modelPartsHandler.CheckSlot(1))
                            {
                                slot1 = true;
                                RefrenceManager.instance.modelPartsHandler.AssignASlot(1);
                                if (number == 1)
                                {
                                    RefrenceManager.instance.modelPartsHandler.AddSelection(number, true);
                                    //  SoundManager.manager.ButtonSound();
                                }
                                SnapToObject(positions[0]);
                            }
                            else
                            {

                                SnapToObject(originalpostion, true);
                            }
                        }
                        if (currentCorrectPositionCollider.gameObject.name == "Collider2")
                        {
                            if (RefrenceManager.instance.modelPartsHandler.CheckSlot(2))
                            {
                                slot2 = true;
                                RefrenceManager.instance.modelPartsHandler.AssignASlot(2);
                                if (number == 2)
                                {
                                    RefrenceManager.instance.modelPartsHandler.AddSelection(number, true);

                                }
                                SnapToObject(positions[1]);
                            }
                            else
                            {


                                SnapToObject(originalpostion, true);
                            }
                        }
                        if (currentCorrectPositionCollider.gameObject.name == "Collider3")
                        {
                            if (RefrenceManager.instance.modelPartsHandler.CheckSlot(3))
                            {
                                slot3 = true;
                                RefrenceManager.instance.modelPartsHandler.AssignASlot(3);
                                if (number == 3)
                                {
                                    RefrenceManager.instance.modelPartsHandler.AddSelection(number, true);
                                }
                                SnapToObject(positions[2]);
                            }
                            else
                            {

                                SnapToObject(originalpostion, true);
                            }
                        }
                    }
                }
            }

            // Debug.Log("positions are" + currentCorrectPositionCollider.gameObject.GetComponent<Position>().position);

            /*
                        if (currentCorrectPositionCollider != null)
                        {
                            Debug.Log("" + currentCorrectPositionCollider.gameObject.transform.position);
                            SnapToObject(currentCorrectPositionCollider.gameObject.transform.position);
                        }
                        else
                        {
                            // Snap the object back to the initial position
                            SnapToObject(initialPosition);
                        }*/
        }

       async  void SnapToObject(Vector3 targetPosition, bool originalPosition = false)
        {
            // Snap the object to the specified position
            transform.localPosition = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);
          
            if (originalPosition)
            {
                 await Task.Delay(300);
                Debug.Log("this is calling ");
                if (currentCorrectPositionCollider)
                {
                    if (currentCorrectPositionCollider.gameObject.name == "Collider1")
                    {
                        Debug.Log("this is calling 1");
                        if (number == 1)
                        {
                            RefrenceManager.instance.modelPartsHandler.AddSelection(number, true);
                            //  SoundManager.manager.ButtonSound();
                        }
                        slot1 = true;
                        RefrenceManager.instance.modelPartsHandler.AssignASlot(1);

                    }
                    else if (currentCorrectPositionCollider.gameObject.name == "Collider2")
                    {
                        Debug.Log("this is calling 2");
                        if (number == 2)
                        {
                            RefrenceManager.instance.modelPartsHandler.AddSelection(number, true);
                            //  SoundManager.manager.ButtonSound();
                        }
                        slot2 = true;
                        RefrenceManager.instance.modelPartsHandler.AssignASlot(2);
                    }
                    else if (currentCorrectPositionCollider.gameObject.name == "Collider3")
                    {
                        Debug.Log("this is calling 3");
                        if (number == 3)
                        {
                            RefrenceManager.instance.modelPartsHandler.AddSelection(number, true);
                            //  SoundManager.manager.ButtonSound();
                        }
                        slot3 = true;
                        RefrenceManager.instance.modelPartsHandler.AssignASlot(3);
                    }
                }
            }

        }
    public float minZoom = -1f;
        public float maxZoom = 1f;
        public float time;
        void Update()
        {
            if (isDragging)
            {
                // Use the mouse wheel to move along the Z-axis
                float zMovement = Input.GetAxis("Mouse ScrollWheel") * 5;

                // Calculate the new zoom position
                float newZ = transform.position.z + zMovement;

                // Check if the new zoom position is within the specified range
                if (newZ >= minZoom && newZ <= maxZoom)
                {
                    // Use DOTween to smoothly interpolate the current position to the new position
                    transform.DOMoveZ(newZ, time).SetEase(Ease.OutQuad);
                }
            }
        }

        Vector3 GetMouseWorldPos()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = mainCamera.transform.position.z;
            return mainCamera.ScreenToWorldPoint(mousePos);
        }
    }
}

