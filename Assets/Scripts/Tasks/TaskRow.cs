using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class TaskRow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _title;
    [SerializeField]
    private Image _checkmark;

    public UnityEvent<string> OnTaskRowClicked;

    private string _id;

    public void Init(TasksJSONStructure.Task task)
    {
        _title.text = task.title;
        _id = task.id;
        Mark(task.done);
    }
    public void Init(TasksJSONStructure.Step step, bool complete = false)
    {
        _id = step.id;
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

    public void HandleOnClick()
    {
        OnTaskRowClicked?.Invoke(_id);
    }
}
