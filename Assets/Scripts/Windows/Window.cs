using UnityEngine;
using UnityEngine.EventSystems;

public class Window : MonoBehaviour
{
    [Tooltip("Minimum width limit to prevent excessive compression.")]
    [SerializeField] private float minWidth = 500f;
    [Tooltip("Minimum height limit to prevent excessive compression.")]
    [SerializeField] private float minHeight = 400f;

    private RectTransform _transform;

    private void Start() {
        
        _transform = GetComponent<RectTransform>();
    }

    public void Drag(PointerEventData eventData) {
        _transform.position += new Vector3(eventData.delta.x, eventData.delta.y);
    }

    public void ResizeLeft(PointerEventData eventData) {
        float dragX = eventData.delta.x;
        Vector2 newSize = _transform.sizeDelta;
        Vector3 newPosition = _transform.position;

        if (dragX < 0) // Dragging left: Expand
        {
            newSize.x += Mathf.Abs(dragX);
            newPosition.x += dragX / 2;
        }
        else if (dragX > 0) // Dragging right: Compress
        {
            if (_transform.rect.width - dragX >= minWidth) {
                newSize.x -= dragX;
                newPosition.x += dragX / 2;
            }
            else {
                // If next step would break minWidth, limit the change
                float allowedShrink = _transform.rect.width - minWidth;
                newSize.x -= allowedShrink;
                newPosition.x += allowedShrink / 2;
            }
        }

        // Apply 
        _transform.sizeDelta = newSize;
        _transform.position = newPosition;

    }
    public void ResizeRight(PointerEventData eventData) {
        float dragX = eventData.delta.x;
        Vector2 newSize = _transform.sizeDelta;
        Vector3 newPosition = _transform.position;

        if (dragX > 0) // Dragging right: Expand
        {
            newSize.x += dragX;
            newPosition.x += dragX / 2;
        }
        else if (dragX < 0) // dragging left -> compress
        {
            if (_transform.rect.width + dragX >= minWidth) {

                newSize.x += dragX;
                newPosition.x += dragX / 2;
            }
            else {
                // If next step would break minWidth, limit the change
                float allowedShrink = _transform.rect.width - minWidth;
                newSize.x -= allowedShrink;
                newPosition.x -= allowedShrink / 2;
            }
        }

        // Apply
        _transform.sizeDelta = newSize;
        _transform.position = newPosition;
    }
    public void ResizeDown(PointerEventData eventData) {
        float dragY = eventData.delta.y;
        Vector2 newSize = _transform.sizeDelta;
        Vector3 newPosition = _transform.position;

        if (dragY < 0) // Dragging down: Expand
        {
            newSize.y += Mathf.Abs(dragY);
            newPosition.y += dragY / 2;
        }
        else if (dragY > 0) // Dragging right: Compress
        {
            if (_transform.rect.height - dragY >= minHeight) {
                newSize.y -= dragY;
                newPosition.y += dragY / 2;
            }
            else {
                // If next step would break minHeight, limit the change
                float allowedShrink = _transform.rect.height - minHeight;
                newSize.y -= allowedShrink;
                newPosition.y += allowedShrink / 2;
            }
        }

        // Apply 
        _transform.sizeDelta = newSize;
        _transform.position = newPosition;
    }
    public void ResizeUp(PointerEventData eventData) {
        float dragY = eventData.delta.y;
        Vector2 newSize = _transform.sizeDelta;
        Vector3 newPosition = _transform.position;

        if (dragY > 0) // Dragging right: Expand
        {
            newSize.y += dragY;
            newPosition.y += dragY / 2;
        }
        else if (dragY < 0) // dragging left -> compress
        {
            if (_transform.rect.height + dragY >= minHeight) {

                newSize.y += dragY;
                newPosition.y += dragY / 2;
            }
            else {
                // If next step would break minHeight, limit the change
                float allowedShrink = _transform.rect.height - minHeight;
                newSize.y -= allowedShrink;
                newPosition.y -= allowedShrink / 2;
            }
        }

        // Apply
        _transform.sizeDelta = newSize;
        _transform.position = newPosition;
    }
}
