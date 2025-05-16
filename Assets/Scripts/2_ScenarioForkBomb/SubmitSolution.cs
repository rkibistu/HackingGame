using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubmitSolution : MonoBehaviour
{
    [SerializeField]
    private GameObject _recoveryWebpage;
    [SerializeField]
    private GameObject _submitWebpage;
    [SerializeField]
    private Button _submitButton;
    [SerializeField]
    private TerminalManager _terminalManager;

    [Header("Submit asignment story and task IDs")]
    [SerializeField]
    private string _taskForAllowSubmitId;
    [SerializeField]
    private string _taskForSuccessSubmitId;

    [SerializeField]
    private TMP_Text _errorSubmitMessage;

    public void OnSubmitButtonClicked()
    {
        if (TasksController.Instance.CheckCurrentTask(_taskForAllowSubmitId))
        {
            TasksController.Instance.ActivateTask(_taskForSuccessSubmitId);
            _recoveryWebpage.SetActive(true);
            _submitWebpage.SetActive(false);
        }
        else
        {
            _errorSubmitMessage.text = "No file uploaded.";
        }
    }
}
