using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskRow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _title;
    [SerializeField]
    private Image _checkmark;

    private TasksJSONStructure.Task _task = null;

    public void Init(TasksJSONStructure.Task task, bool complete = false)
    {
        _task = task;
        _title.text = task.title;
        Mark(complete);
    }
    public void Init(TasksJSONStructure.Step step, bool complete = false)
    {
        _title.text = step.title;
        Mark(complete);
    }
    public void Mark(bool complete = true)
    {
        if (complete)
        {
            _checkmark.gameObject.SetActive(true);
        }
        else
        {
            _checkmark.gameObject.SetActive(false);
        }
    }

    //returns null if this row is associated with a step, not a task
    public TasksJSONStructure.Task GetTask() {
        return _task;
    }
}
