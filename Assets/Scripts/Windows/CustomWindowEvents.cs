using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomWindowEvents : MonoBehaviour, IDragHandler, IPointerClickHandler {

    [System.Serializable]
    public class DragEvent : UnityEvent<PointerEventData> { }
    [System.Serializable]
    public class ClickEvent : UnityEvent<PointerEventData> { }

    public DragEvent OnDragEvent;
    public ClickEvent OnClickEvent;

    public void OnDrag(PointerEventData eventData) {
        OnDragEvent?.Invoke(eventData);
    }
    public void OnPointerClick(PointerEventData eventData) {
        OnClickEvent?.Invoke(eventData);
    }
}
