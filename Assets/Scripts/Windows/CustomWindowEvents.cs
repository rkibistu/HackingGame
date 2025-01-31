using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomWindowEvents : MonoBehaviour, IDragHandler {

    // Declare a UnityEvent with PointerEventData as the parameter
    [System.Serializable]
    public class DragEvent : UnityEvent<PointerEventData> { }

    // The UnityEvent exposed in the Inspector
    public DragEvent OnDragEvent;

    public void OnDrag(PointerEventData eventData) {
        OnDragEvent?.Invoke(eventData);
    }
}
