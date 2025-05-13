using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static TasksJSONStructure;

public class TasksController : MonoBehaviour {

    [SerializeField]
    private string _jsonFilename;
    [SerializeField]
    private GameObject _taskRowPrefab;
    [SerializeField]
    private GameObject _subtaskRowPrefab;
    [SerializeField]
    private Transform _journalContentContainer;

    private TaskList _tasks;
    private Dictionary<string, TaskRow> _journalRows = new();

    //  Example usage in Start
    void Start() {
        _tasks = LoadTasks();
        PopulateJournal();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.J)) {
            if (Cursor.lockState == CursorLockMode.Locked) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                UIController.Instance.SetActiveTaskPanel(true);
            }
            else {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                UIController.Instance.SetActiveTaskPanel(false);
            }
        }

        //just for testing
        if (Input.GetKeyDown(KeyCode.T)) {
            //Mark("check-room");
            ActivateTask("check-door");
        }
    }

    public void Mark(string taskId, bool complete = true) {
        if (_journalRows.ContainsKey(taskId)) {
            _journalRows[taskId].Mark(complete);
        }
    }

    public void ActivateTask(string id) {
        foreach (var task in _tasks.tasks) {
            if (task.id != id)
                continue;

            if (task.active == true)
                return;

            task.active = true;
            var row = Instantiate(_taskRowPrefab, _journalContentContainer);
            TaskRow taskRow = row.GetComponent<TaskRow>();
            taskRow.Init(task);
            _journalRows[task.id] = taskRow;

            foreach (var step in task.steps) {
                row = Instantiate(_subtaskRowPrefab, _journalContentContainer);
                taskRow = row.GetComponent<TaskRow>();
                taskRow.Init(step);
                _journalRows[task.id] = taskRow;
            }
        }
    }

    // Load TaskList from file
    private TaskList LoadTasks() {
        string path = Path.Combine(Application.streamingAssetsPath, _jsonFilename);
        if (!File.Exists(path)) {
            Debug.LogWarning("Task file not found at: " + path);
            return new TaskList { tasks = new Task[0] };
        }

        string json = File.ReadAllText(path);
        TaskList taskList = JsonUtility.FromJson<TaskList>(json);
        Debug.Log("Loaded tasks from: " + path);
        return taskList;
    }

    // Save TaskList to file
    private void SaveTasks(TaskList taskList) {
        string path = Path.Combine(Application.streamingAssetsPath, _jsonFilename);
        string json = JsonUtility.ToJson(taskList, true); // Pretty print
        File.WriteAllText(path, json);
        Debug.Log("Saved tasks to: " + path);
    }

    private void PopulateJournal() {
        foreach (var task in _tasks.tasks) {
            if (task.active == false) {
                continue;
            }

            var row = Instantiate(_taskRowPrefab, _journalContentContainer);
            TaskRow taskRow = row.GetComponent<TaskRow>();
            taskRow.Init(task);
            _journalRows[task.id] = taskRow;

            foreach (var step in task.steps) {
                row = Instantiate(_subtaskRowPrefab, _journalContentContainer);
                taskRow = row.GetComponent<TaskRow>();
                taskRow.Init(step);
                _journalRows[task.id] = taskRow;
            }
        }
    }
}
