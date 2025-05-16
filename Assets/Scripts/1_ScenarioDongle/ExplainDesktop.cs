using UnityEngine;

public class ExplainDesktop : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private string homeStoryId;

    [SerializeField]
    private string NASStoryId;

    [SerializeField]
    private string rootStoryId;

    [SerializeField]
    private string steamStoryId;


    public void OnClickHome()
    {
        StoryManager.Instance.PlayStory(homeStoryId);
    }

    public void OnClickNAS()
    {
        StoryManager.Instance.PlayStory(NASStoryId);
    }

    public void OnClickRoot()
    {
        StoryManager.Instance.PlayStory(rootStoryId);
    }

    public void OnClickSteam()
    {
        StoryManager.Instance.PlayStory(steamStoryId);
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
