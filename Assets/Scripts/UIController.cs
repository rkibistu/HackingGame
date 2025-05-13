using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour {
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

    public static UIController Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (IsActiveLetterPanel()) {
                SetActiveLetterPanel(false);
            }
        }
    }

    public void ShowHint(string hintText) {
        _hintInteractiveText.enabled = true;
        _hintInteractiveText.text = hintText;
    }

    public void HideHint() {
        _hintInteractiveText.enabled = false;
    }

    public void SetActiveStoryPanel(bool active) {
        _storyPanel.SetActive(active);
    }
    public bool IsActiveStoryPanel() {
        return _storyPanel.activeInHierarchy;
    }

    public void SetActiveLetterPanel(bool active) {
        if (active == true) {
            // We don't want story on the back oif a letter
            DialogueController.Instance.SkipCurrentStoryCompletely();
            _storyPanel.SetActive(false);
        }

        _letterPanel.SetActive(active);
    }
    public bool IsActiveLetterPanel() {
        return _letterPanel.activeInHierarchy;
    }
}
