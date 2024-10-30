using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _hintInteractiveText;

    public void ShowHint(string hintText)
    {
        _hintInteractiveText.enabled = true;
        _hintInteractiveText.text = hintText;
    }

    public void HideHint()
    {
        _hintInteractiveText.enabled = false;
    }
}
