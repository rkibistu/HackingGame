using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IngameMenuController : MonoBehaviour {

    [SerializeField]
    private GameObject _root;

    [Header("Panels")]
    [SerializeField]
    private GameObject _gamePanel;
    [SerializeField]
    private GameObject _controlsPanel;
    [SerializeField]
    private GameObject _videoPanel;
    [SerializeField]
    private GameObject _areYouSurePanel;
    [SerializeField]
    private GameObject _objectivesPanel;

    private List<GameObject> _allPanels = new();
    private GameObject _currentPanel = null;

    private Animator _animator;

    // used after you close the menu to go back to the last state of the cusor
    private CursorLockMode _lastCursorLockMode;
    private bool _lastCursorVisibility;

    void Start() {
        _animator = GetComponent<Animator>();

        AggregateAllPanels();

        DisableAllPanels();
        _currentPanel = _gamePanel;
        _currentPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update() {

    }

    public void SwitchToGamePanel() {

        if (_currentPanel == _objectivesPanel) {
            // No need to do enything more,
            // the animation will do everything
            // and will call OpenCurrentPanel to open the new pane;
            // after aniamtion ends
            _animator.Play("CloseObjectivePanel");
        }
        else {
            DisableAllPanels();
            _gamePanel.SetActive(true);
        }
        _currentPanel = _gamePanel;
    }
    public void SwitchToControlsPanel() {
        if (_currentPanel == _objectivesPanel) {
            _animator.Play("CloseObjectivePanel");
        }
        else {
            DisableAllPanels();
            _controlsPanel.SetActive(true);
        }
        _currentPanel = _controlsPanel;
    }
    public void SwitchToVideoPanel() {
        if (_currentPanel == _objectivesPanel) {
            _animator.Play("CloseObjectivePanel");
        }
        else {
            DisableAllPanels();
            _videoPanel.SetActive(true);
        }

        _currentPanel = _videoPanel;
    }
    public void SwitchToAreyousurePanel() {
        if (_currentPanel == _objectivesPanel) {
            _animator.Play("CloseObjectivePanel");
        }
        else {
            DisableAllPanels();
            _areYouSurePanel.SetActive(true);
        }

        _currentPanel = _areYouSurePanel;
    }

    public void SwitchToObjectivePanel() {
        //No need to disable panels -> the aniamtion will do everything
        _currentPanel = _objectivesPanel;
        _animator.Play("OpenObjectivesPanel");
    }

    //Called by aniamtion that closes objective panel
    // Used to open the new panel after the aniamtion
    // The _currentPanel is set when the button is pressed
    public void OpenCurrentPanel() {
        _currentPanel.SetActive(true);
    }

    public void Toggle() {
        if (_root.activeInHierarchy)
            Close();
        else
            Open();
    }
    public void Close() {
        Cursor.lockState = _lastCursorLockMode;
        Cursor.visible = _lastCursorVisibility;

        _root.SetActive(false);
    }
    public void Open() {
        _lastCursorLockMode = Cursor.lockState;
        _lastCursorVisibility = Cursor.visible;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _root.SetActive(true);
    }
    public bool IsOpen() { return gameObject.activeInHierarchy; }

    public void ExitToMainMenu() {
        SceneManager.LoadScene("StartMenu");
    }

    private void AggregateAllPanels() {
        _allPanels.Clear();
        if (_gamePanel)
            _allPanels.Add(_gamePanel);
        if (_controlsPanel)
            _allPanels.Add(_controlsPanel);
        if (_videoPanel)
            _allPanels.Add(_videoPanel);
        if (_areYouSurePanel)
            _allPanels.Add(_areYouSurePanel);
    }
    private void DisableAllPanels() {
        foreach (var panel in _allPanels) {
            panel.SetActive(false);
        }
    }
}
