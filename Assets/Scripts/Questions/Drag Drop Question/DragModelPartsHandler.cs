using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Drag_Drop_Question
{
    public class DragModelPartsHandler : MonoBehaviour
    {
        public static event Action OnBeginDragModelParts;
        public static event Action OnEndDragModelParts;
        private Vector3 _initialMousePosition;
        public Camera _camera;
        private Transform _draggedObject;
        /*
                private void Awake()
                {

                }

                private void Update()
                {
                    MouseDown();
                    MouseDrag();
                    MouseUp();
                }

                private void MouseDown()
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        GetDragObject();
                    }
                }

                private void MouseDrag()
                {
                    if (!Input.GetMouseButton(0))
                        return;
                    if(_draggedObject==null)
                        return;
                    _draggedObject.position = MouseWorldPosition();
                }

                private void MouseUp()
                {
                    if(!Input.GetMouseButtonUp(0))
                        return;
                    _draggedObject = null;
                    OnEndDragModelParts?.Invoke();
                }

                private void GetDragObject()
                {
                    var ray = _camera.ScreenPointToRay(Input.mousePosition);

                    if (!Physics.Raycast(ray, out var hit))
                        return;
                    if(!hit.transform.TryGetComponent(out DragObject draggableObject))
                        return;
                    _draggedObject = draggableObject.transform;
                    OnBeginDragModelParts?.Invoke();
                }

                private Vector2 MouseWorldPosition()
                {
                    return _camera.ScreenToWorldPoint(Input.mousePosition);
                }*/

        private void Update()
        {
            //if (RefrenceManager.instance.uIManager.settingsScreen.activeSelf || RefrenceManager.instance.uIManager.endLessonPopup.activeSelf
            //    || RefrenceManager.instance.uIManager.endSessionPopup.activeSelf)
            //{

            //}
            //else
            //{
                MouseDown();
                MouseDrag();
                MouseUp();
            //}
        }

        private void MouseDown()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GetDragObject();
            }
        }

        private void MouseDrag()
        {
            if (!Input.GetMouseButton(0) || _draggedObject == null)
                return;

            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 currentMouseWorldPos = _camera.ScreenToWorldPoint(new Vector3(currentMousePosition.x, currentMousePosition.y, _draggedObject.position.z));
            Vector3 offset = currentMouseWorldPos - _initialMousePosition;

            _draggedObject.position += new Vector3(offset.x, offset.y, 0);
            _initialMousePosition = currentMouseWorldPos;
        }

        private void MouseUp()
        {
            if (!Input.GetMouseButtonUp(0))
                return;

            _draggedObject = null;
            OnEndDragModelParts?.Invoke();
        }

        private void GetDragObject()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit))
                return;

            if (!hit.transform.TryGetComponent(out DragObject draggableObject))
                return;

            _draggedObject = draggableObject.transform;
            _initialMousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            OnBeginDragModelParts?.Invoke();
        }


    }
}
