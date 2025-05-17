using System.IO;
using System.Linq;
using UnityEngine;
using static DialogueJSONStructure;
using static TasksJSONStructure;
using TMPro;
using System.Collections;

public class DialogueController : MonoBehaviour
{
    [SerializeField]
    private string _jsonFilename;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI _speakerText;
    [SerializeField]
    private GameObject _nextText;
    [SerializeField]
    private TextMeshProUGUI _contentText;
    private TypewriterEffect _typewriter;

    public bool IsStoryRunning => UIController.Instance.IsActiveStoryPanel();

    private StoryList _story;
    private string _currentStoryId;
    private bool _currentLineComplete = true;

    public static DialogueController Instance { get; private set; }
    private void Awake() {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    private void Start()
    {
        _typewriter = _contentText.GetComponent<TypewriterEffect>();
        if (_typewriter == null)
            Debug.LogError("Typewriter should not be null!");
        _story = LoadStory();
    }

    // Update UI with the next line in specified story
    public void PlayStory(string id) {

        //_storyPanel.SetActive(true);
        UIController.Instance.SetActiveStoryPanel(true);
        ClearState();

        Line nextLine = null;
        GetNextLineInStory(id, out nextLine);

        if (nextLine != null) {
            _currentStoryId = id;
            _currentLineComplete = false;
            _typewriter.CompleteTextRevealed += ContinueStory;

            CheckForActions(nextLine);

            _speakerText.text = nextLine.speaker;
            _typewriter.FeedText(nextLine.content);
        }
        else {
            _currentStoryId = "";
            //_storyPanel.SetActive(false);
            UIController.Instance.SetActiveStoryPanel(false);
        }
    }
    private void ContinueStory() {
        _currentLineComplete = true;

        _nextText.SetActive(true);
    }

    public void Next() {
        if (_currentLineComplete == false)
        {
            _typewriter.Skip();
            return;
        }

        Line nextLine = null;
        GetNextLineInStory(_currentStoryId, out nextLine);

        if (nextLine != null) {
            _nextText.SetActive(false);
            _currentLineComplete = false;

            CheckForActions(nextLine);

            _speakerText.text = nextLine.speaker;
            _typewriter.FeedText(nextLine.content);
        }
        else {
            _currentStoryId = "";
            _currentLineComplete = true;
            _typewriter.CompleteTextRevealed -= ContinueStory;

            //_storyPanel.SetActive(false);
            UIController.Instance.SetActiveStoryPanel(false);
        }
    }

    //skip story and mark all lines as read
    public void SkipStoryCompletely(string id) {
        Story story = GetStory(id);
        if (story == null)
            return;
        if (story.currentLine >= story.lines.Count())
            return;

        story.currentLine = story.lines.Count();
    }
    public void SkipCurrentStoryCompletely() {
        if (!IsStoryRunning)
            return;

        Story story = GetStory(_currentStoryId);
        if (story == null)
            return;
        if (story.currentLine >= story.lines.Count())
            return;

        story.currentLine = story.lines.Count();

        ClearState();
        UIController.Instance.SetActiveStoryPanel(false);
    }

    private void ClearState() {
        _currentStoryId = "";
        _currentLineComplete = true;
        _nextText.SetActive(false);
        _typewriter.CompleteTextRevealed -= ContinueStory;
    }

    // Return the next line from the story with specified id.
    // Returns true if there are more lines in this story
    private bool GetNextLineInStory(string id, out Line nextLine)
    {
        nextLine = null;

        Story story = GetStory(id);
        if (story == null)
            return false;
        if(story.currentLine >= story.lines.Count())
            return false;

        nextLine = story.lines[story.currentLine];
         

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

    private void CheckForActions(Line storyLine) {
        if (storyLine.taskIdToComplete != null)
            TasksController.Instance.Mark(storyLine.taskIdToComplete);
        
        if(storyLine.taskIdToStart != null)
            TasksController.Instance.ActivateTask(storyLine.taskIdToStart);

        if(storyLine.gameobjectToEnable != null)
            GameplayController.Instance.EnablePopup(storyLine.gameobjectToEnable);

        if(storyLine.endLevel != null && storyLine.endLevel == true)
        {
            GameplayController.Instance.EndLevel();
        }
    }
}
