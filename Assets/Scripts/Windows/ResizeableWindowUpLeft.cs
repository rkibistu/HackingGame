using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeableWindowUpLeft : MonoBehaviour, IDragHandler {
    [Tooltip("The rect transform of the panel that you want to be resized.")]
    [SerializeField] private RectTransform _transform = null;

    [Tooltip("Minimum width limit to prevent excessive compression.")]
    [SerializeField] private float minWidth = 500f;
    [Tooltip("Minimum height limit to prevent excessive compression.")]
    [SerializeField] private float minHeight = 500f;

    public void OnDrag(PointerEventData eventData) {
        ResizeLeft(eventData);
        ResizeUp(eventData);
    }

    private void ResizeLeft(PointerEventData eventData) {
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
            Debug.Log(_transform.rect.width + "   " + dragX);
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
    private void ResizeUp(PointerEventData eventData) {
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
