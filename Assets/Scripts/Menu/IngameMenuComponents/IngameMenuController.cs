using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private List<GameObject> _allPanels = new();

    void Start() {
        AggregateAllPanels();
    }

    // Update is called once per frame
    void Update() {

    }

    public void SwitchToGamePanel() {
        DisableAllPanels();
        _gamePanel.SetActive(true);
    }
    public void SwitchToControlsPanel() {
        DisableAllPanels();
        _controlsPanel.SetActive(true);
    }
    public void SwitchToVideoPanel() {
        DisableAllPanels();
        _videoPanel.SetActive(true);
    }
    public void SwitchToAreyousurePanel() {
        DisableAllPanels();
        _areYouSurePanel.SetActive(true);
    }

    public void Close() {
        _root.SetActive(false);
    }
    public void Open() {
        _root.SetActive(true);
    }

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
        foreach(var panel in _allPanels) {
            panel.SetActive(false);
        }
    }
}
