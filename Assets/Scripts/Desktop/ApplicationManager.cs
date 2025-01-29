using UnityEngine;
using UnityEngine.UI;

// This class should be added to every canvas that represents a dekstop window
// It handles the focus of the window (the depth of canvas so the focused oen to be first visible)
[RequireComponent(typeof(Canvas))]
public class ApplicationManager : MonoBehaviour
{
    [SerializeField]
    private DesktopManager _desktop;
    [SerializeField]
    private Sprite _iconSprite;
    [SerializeField]
    private string _name;
    [SerializeField]
    private GameObject _taskbarEntryPrefab;
  


    private TaskbarEntry _taskbarEntry;
    private Canvas _canvas;

    private void OnEnable() {
        _canvas = GetComponent<Canvas>();
        Open();
    }

    // Called OnEnable. An application is opened by setting the gameobject as active
    private void Open() {
        _desktop.BringInFront(_canvas);

        _taskbarEntry = Instantiate(_taskbarEntryPrefab).GetComponent<TaskbarEntry>();
        _taskbarEntry.Init(_iconSprite, this);
        _desktop.AddToTaskbar(_taskbarEntry);
    }

    public void Close() {
        //remove from taskbar
        this.gameObject.SetActive(false);
    }

    public void Focus() {
        _desktop.BringInFront(_canvas);
    }
    
    public Sprite GetIcon() {
        return _iconSprite;
    }
    public string GetName() {
        return _name;
    }
}
