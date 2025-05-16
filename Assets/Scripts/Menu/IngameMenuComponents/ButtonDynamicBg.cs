using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonDynamicBg : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler {

    [SerializeField]
    private GameObject _hoverBackground;

    private bool _isSelected = false;
    public void OnPointerEnter(PointerEventData eventData) {
        Debug.Log("Mouse entered the button area");
        // e.g. highlight the button, show tooltip, etc.

        _hoverBackground.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        Debug.Log("Mouse exited the button area");
        // e.g. remove highlight or hide tooltip

        if(_isSelected == false)
            _hoverBackground.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData) {
        Debug.Log("Button selected!");
        // Highlight or animate if needed
        _isSelected = true;
        _hoverBackground.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData) {
        Debug.Log("Button deselected!");
        // Remove highlight or revert visual changes
        _isSelected = false;
        _hoverBackground.SetActive(false);
    }
}
