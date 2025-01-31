using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeableWindowDownRight : MonoBehaviour, IDragHandler {
    [Tooltip("The rect transform of the panel that you want to be resized.")]
    [SerializeField] private RectTransform _transform = null;

    [Tooltip("Minimum width limit to prevent excessive compression.")]
    [SerializeField] private float minWidth = 500f;
    [Tooltip("Minimum height limit to prevent excessive compression.")]
    [SerializeField] private float minHeight = 500f;

    public void OnDrag(PointerEventData eventData) {
        ResizeRight(eventData);
        ResizeDown(eventData);
    }

    private void ResizeRight(PointerEventData eventData) {
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
    private void ResizeDown(PointerEventData eventData) {
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
}
