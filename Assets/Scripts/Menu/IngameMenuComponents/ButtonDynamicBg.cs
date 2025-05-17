using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonDynamicBg : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler {

    [SerializeField]
    private GameObject _hoverBackground;

    private bool _isSelected = false;
    public void OnPointerEnter(PointerEventData eventData) {
        // e.g. highlight the button, show tooltip, etc.

        _hoverBackground.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        // e.g. remove highlight or hide tooltip

        if(_isSelected == false)
            _hoverBackground.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData) {
        // Highlight or animate if needed
        _isSelected = true;
        _hoverBackground.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData) {
        // Remove highlight or revert visual changes
        _isSelected = false;
        _hoverBackground.SetActive(false);
    }
}
