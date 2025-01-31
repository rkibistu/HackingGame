using UnityEngine;
using UnityEngine.EventSystems;

public class DragableWindow : MonoBehaviour, IDragHandler
{
    [Tooltip("The rect transform of the panel that you want to be moved on drag. An interesting use is to define smaller zone so you can drag a bigger window")]
    [SerializeField]
    private RectTransform _transform = null;


    public void OnDrag(PointerEventData eventData) {
        _transform.position += new Vector3(eventData.delta.x, eventData.delta.y);
    }
}
