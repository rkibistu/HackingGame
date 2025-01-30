using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class TaskbarManager : MonoBehaviour
{
    [SerializeField]
    private DesktopManager _desktop;
    [SerializeField]
    private GameObject _taskbarEntryPrefab;
    [SerializeField]
    private Transform _taskbarLeft;

    // the id of the app and the associated taskbar entry
    // the id can be used to get the app from the desktopmanager
    public Dictionary<int, TaskbarEntry> _entries = new Dictionary<int, TaskbarEntry>();

    public void AddEntry(ApplicationManager app) {
        var taskbarEntry = Instantiate(_taskbarEntryPrefab).GetComponent<TaskbarEntry>();
        taskbarEntry.Init(app.GetIcon(), app, _desktop);
        taskbarEntry.transform.SetParent(_taskbarLeft);
        _entries.Add(app.ID, taskbarEntry);
    }
    public void RemoveEntry(ApplicationManager app) {
        Destroy(_entries[app.ID].gameObject);
        _entries.Remove(app.ID);
    }
    public void Focus(ApplicationManager app) {
        foreach(var key in _entries.Keys) {
            if(key.Equals(app.ID)) {
                _entries[key].Focus();
            }
            else {
                _entries[key].Defocus();
            }
        }
    }
}
