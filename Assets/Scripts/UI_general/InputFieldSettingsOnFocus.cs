using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_InputField))]
public class InputFieldSettingsOnFocus : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    private void OnDisable()
    {
        OnInputUnfocus();
    }

    public void OnSelect(BaseEventData eventData)
    {
        OnInputFocus();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        OnInputUnfocus();
    }

    private void OnInputFocus()
    {
        // Replace with your custom logic
        UIController.Instance.CanOpenIngameMenu = false;
    }

    private void OnInputUnfocus()
    {
        // Replace with your custom logic
        UIController.Instance.CanOpenIngameMenu = true;
    }
}
