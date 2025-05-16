using System.Collections.Generic;
using System.IO;
using TMPro;
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
    [SerializeField]
    private TextMeshProUGUI _currentObjectiveText;
    [SerializeField]
    private GameObject _currentObjectivePanel;

    private TaskList _tasks;
    private Dictionary<string, Task> _journalRows = new();

    private Task _currentTask;

    public static TasksController Instance { get; private set; }
    private void Awake() {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

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
            //ActivateTask("check-door");
            //GameplayController.Instance.StartLevel();
        }
    }


    //ATENTION: an inactive but compelte task will return FALSE here
    //  this was not a mistake
    public bool CheckIfComplete(string taskId) {
        // If it is not added in the journal it means it was not activated
        // An inactive task can be complete, but we will return this as not coimplete
        Debug.Log(taskId);
        if (_journalRows.ContainsKey(taskId)) {
            Debug.Log(_journalRows[taskId].done);
            return _journalRows[taskId].done;
        }
        Debug.Log("false");
        return false;
    }

    // if markInactiveToo == false -> only an active task can be marked
    // if markInactiveToo == true -> an existing but inactive task can be marked too
    public void Mark(string taskId, bool complete = true, bool markInactiveToo = false) {

        foreach (var task in _tasks.tasks) {

            if (task.id == taskId) {
                MarkTaskAsComplete(task);
            }
            else {
                foreach (var step in task.steps) {
                    if (step.id == taskId) {
                        MarkStepAsComplete(task, step, task.active);
                    }
                }
            }

        }
    }

    private void MarkTaskAsComplete(Task task) {
        MarkAllStepsAsComplete(task);
        task.done = true;

        CheckForActionsAfterCompletion(task);

        if (task.active) {
            task.row.Mark();
            if (_currentTask != null && task.id == _currentTask.id) {
                RenewCurrentObjective();
            }
        }
    }
    private void MarkStepAsComplete(Task parent, Step step, bool isActive) {
        step.done = true;
        if (isActive) {
            step.row.Mark();
        }

        bool taskComplete = true;
        foreach (var s in parent.steps) {
            if (s.done == false) {
                taskComplete = false;
                break;
            }
        }
        if (taskComplete == true) {
            MarkTaskAsComplete(parent);
        }
    }
    private void MarkAllStepsAsComplete(Task task) {
        foreach (var step in task.steps) {
            step.done = true;
            if (task.active == true) {
                step.row.Mark();
            }
        }
    }

    public bool CheckCurrentTask(string id)
    {
        if (_currentTask.id == id)
            return true;
        return false;
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
            task.row = taskRow;
            _journalRows[task.id] = task;

            // Change current objective every time you activate a new task
            // This could be changed later. Maybe a dedicated script for the panel so
            // we add some aniamtions and effects
            UpdateCurrentObjectivePanel(task);

            if (task.gameobjectToEnableOnActivation != null)
                GameplayController.Instance.EnablePopup(task.gameobjectToEnableOnActivation);

            foreach (var step in task.steps) {
                row = Instantiate(_subtaskRowPrefab, _journalContentContainer);
                taskRow = row.GetComponent<TaskRow>();
                taskRow.Init(step);

                //_journalRows[task.id] = taskRow;
                step.row = taskRow;
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
            _journalRows[task.id] = task;
            _journalRows[task.id].row = taskRow;

            foreach (var step in task.steps) {
                row = Instantiate(_subtaskRowPrefab, _journalContentContainer);
                taskRow = row.GetComponent<TaskRow>();
                taskRow.Init(step);

                step.row = taskRow;
                //_journalRows[task.id] = taskRow;
            }
        }
        RenewCurrentObjective();
    }

    // Returns next active task
    private Task GetNextTask() {
        foreach (var activeTask in _journalRows.Values) {
            if (activeTask.done == false)
                return activeTask;
        }
        return null;
    }

    private void RenewCurrentObjective() {
        Task task = GetNextTask();
        UpdateCurrentObjectivePanel(task);
    }

    //updates the cotnent of the panel
    private void UpdateCurrentObjectivePanel(Task task) {
        _currentTask = task;
        if (task == null) {
            _currentObjectiveText.text = "";
            _currentObjectivePanel.SetActive(false);
        }
        else {
            _currentObjectivePanel.SetActive(true);
            _currentObjectiveText.text = task.title;
        }
    }


    private void CheckForActionsAfterCompletion(Task task) {
        if (task == null)
            return;

        //story
        if (task.storyIdToStart != null) {
            DialogueController.Instance.SkipCurrentStoryCompletely();
            DialogueController.Instance.PlayStory(task.storyIdToStart);
        }

        //tasks
        if (task.taskIdToComplete != null) {
            Mark(task.id, true, true);
        }
        if (task.taskIdToStart != null) {
            ActivateTask(task.taskIdToStart);
        }

        //gameobjects to enable
        if (task.gameobjectToEnableOnCompletion != null) {
            GameplayController.Instance.EnablePopup(task.gameobjectToEnableOnCompletion);
        }
    }
}
