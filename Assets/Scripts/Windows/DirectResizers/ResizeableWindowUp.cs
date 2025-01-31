using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeableWindowUp : MonoBehaviour, IDragHandler {
    [Tooltip("The rect transform of the panel that you want to be resized.")]
    [SerializeField] private RectTransform _transform = null;

    [Tooltip("Minimum height limit to prevent excessive compression.")]
    [SerializeField] private float minHeight = 400f;

    public void OnDrag(PointerEventData eventData) {
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
