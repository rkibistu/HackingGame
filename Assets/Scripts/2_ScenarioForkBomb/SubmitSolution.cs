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
    private string _storySubmitId;
    [SerializeField]
    private string _taskBeforeSubmitId;
    [SerializeField]
    private string _taskForSuccessSubmitId;

    [SerializeField]
    private TMP_Text _errorSubmitMessage;

    private void OnEnable()
    {
        Debug.LogError("SubmitSolution OnEnable called");
        Debug.LogError("SubmitSolution OnEnable active self: " + _taskBeforeSubmitId + " "+ _taskForSuccessSubmitId + " "+ _storySubmitId);
        if (_submitWebpage.activeSelf)
        {
            Debug.LogError("SubmitSolution OnEnable active self");
            
            _errorSubmitMessage.text = "";
            if (TasksController.Instance.CheckCurrentTask(_taskBeforeSubmitId))
            {
                DialogueController.Instance.PlayStory(_storySubmitId);
            }
        }
    }

    public void OnSubmitButtonClicked()
    {
        //int phase = Interpreter.Instance.GetPhase(_terminalManager.Name);

        if (TasksController.Instance.CheckCurrentTask(_taskForSuccessSubmitId))
        {
            _recoveryWebpage.SetActive(true);
            _submitWebpage.SetActive(false);
        }
        else
        {
            _errorSubmitMessage.text = "No file uploaded.";
        }

        //if (phase == 1)
        //{
        //    _recoveryWebpage.SetActive(true);
        //    _submitWebpage.SetActive(false);
        //}
        //else
        //{
        //    Debug.Log("TODO");
        //}

    }
}
