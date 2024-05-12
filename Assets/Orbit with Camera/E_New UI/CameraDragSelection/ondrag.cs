using UnityEngine;
using UnityEngine.EventSystems;

public class ondrag : MonoBehaviour, IDragHandler ,IEndDragHandler
{

    public void OnDrag(PointerEventData pointerData)
    {
		oncamera.instance.OnDrag(pointerData);
	}

    public void OnEndDrag(PointerEventData eventData)
    {
		//oncamera.instance.ispressing = false;
	}

}
