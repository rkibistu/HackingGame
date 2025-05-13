using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField]
    [Tooltip("Intro paper, opened by letter under the door")]
    private GameObject _letterPanel;
    [SerializeField]
    [Tooltip("Panel with the lsit of tasks")]
    private GameObject _taskPanel;
    [SerializeField]
    [Tooltip("Panel with the story dialogue")]
    private GameObject _storyPanel;
    [SerializeField]
    [Tooltip("Main menu")]
    private GameObject _menu;

    [Header("Elements")]
    [SerializeField]
    private GameObject _crosshair;
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
