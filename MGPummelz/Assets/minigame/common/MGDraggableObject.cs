using UnityEngine;

using rccg.frontend;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MGDraggableObject :MGObject, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector3 originalPosition;

    public bool inDrag = false;

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = this.transform.position;
        GetComponent<Image>().raycastTarget = false;
        inDrag = true;
        
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector3 v3 = Camera.main.ScreenToWorldPoint(eventData.position);
        v3.z = this.transform.position.z;
        this.transform.position = v3;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        this.transform.position = originalPosition;
        GetComponent<Image>().raycastTarget = true;
        inDrag = false;
    }

}
