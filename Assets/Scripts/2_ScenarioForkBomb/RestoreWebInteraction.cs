using TMPro;
using UnityEngine;

public class RestoreWebInteraction : MonoBehaviour
{
    [Header("Recovery webpage story and task IDs")]
    [SerializeField]
    private string _storyRecoveryWebsiteId;
    [SerializeField]
    private string _taskRecoveryWebsiteId;

    void OnEnable()
    {
        if (TasksController.Instance.CheckCurrentTask(_taskRecoveryWebsiteId))
        {
            DialogueController.Instance.PlayStory(_storyRecoveryWebsiteId);
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
