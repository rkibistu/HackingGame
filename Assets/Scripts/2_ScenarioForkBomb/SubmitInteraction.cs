using TMPro;
using UnityEngine;

public class SubmitInteraction : MonoBehaviour
{
    [Header("Submit asignment story and task IDs")]
    [SerializeField]
    private string _storySubmitId;
    [SerializeField]
    private string _taskBeforeSubmitId;

    [SerializeField]
    private TMP_Text _errorSubmitMessage;

    void OnEnable()
    {
        _errorSubmitMessage.text = "";
        if (TasksController.Instance.CheckCurrentTask(_taskBeforeSubmitId))
        {
            DialogueController.Instance.PlayStory(_storySubmitId);
        }   
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
