using UnityEngine;

public class DialogueJSONStructure : MonoBehaviour
{
    [System.Serializable]
    public class Line
    {
        public string speaker;
        public string title;
        public string content;
    }

    [System.Serializable]
    public class Story
    {
        public string id;
        public Line[] lines;
        public int currentLine = 0;
    }

    [System.Serializable]
    public class StoryList
    {
        public Story[] stories;
    }
}
