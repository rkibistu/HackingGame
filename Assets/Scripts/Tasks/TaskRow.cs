using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskRow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _title;
    [SerializeField]
    private Image _checkmark;

    public void Init(TasksJSONStructure.Task task)
    {
        _title.text = task.title;
        Mark(task.done);
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
}
