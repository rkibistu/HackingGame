using UnityEngine;

public class LoginInteractions : MonoBehaviour
{
    [Header("Login story and task IDs")]
    [SerializeField]
    private string _storyLoginId;
    [SerializeField]
    private string _taskBeforeLoginPageId;
    [SerializeField]
    private string _taskAfterLoginPageId;

    void OnEnable()
    {
        if (TasksController.Instance.CheckCurrentTask(_taskBeforeLoginPageId))
        {
            TasksController.Instance.ActivateTask(_taskAfterLoginPageId);
            DialogueController.Instance.PlayStory(_storyLoginId);
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
