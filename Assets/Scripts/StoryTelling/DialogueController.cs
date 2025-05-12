using System.IO;
using System.Linq;
using UnityEngine;
using static DialogueJSONStructure;
using static TasksJSONStructure;

public class DialogueController : MonoBehaviour
{
    [SerializeField]
    private string _jsonFilename;

    private StoryList _story;

    private void Start()
    {
        _story = LoadStory();
    }

    // Play the dialogue/story with specified id. Play from last line
    // Returns true if there are more lines in this dialogue
    public bool Play(string id)
    {
        Story story = GetStory(id);
        if (story == null)
            return false;
        if(story.currentLine >= story.lines.Count())
            return false;

        Line line = story.lines[story.currentLine];
         

        story.currentLine++;
        return story.currentLine >= story.lines.Count();
    }
    private Story GetStory(string id)
    {
        foreach (var story in _story.stories)
        {
            if (story.id == id)
            {
                return story;
            }
        }
        return null;
    }
    private StoryList LoadStory()
    {
        // Assumes the JSON file is located in Assets/Resources/dialogue.json
        string path = Path.Combine(Application.streamingAssetsPath, _jsonFilename);
        if (!File.Exists(path))
        {
            Debug.LogWarning("Task file not found at: " + path);
            return new StoryList { stories = new Story[0] };
        }

        string json = File.ReadAllText(path);
        StoryList taskList = JsonUtility.FromJson<StoryList>(json);
        Debug.Log("Loaded tasks from: " + path);
        return taskList;
    }
}
