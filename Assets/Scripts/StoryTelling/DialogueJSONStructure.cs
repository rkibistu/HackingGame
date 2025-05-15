using UnityEngine;

public class DialogueJSONStructure : MonoBehaviour
{
    [System.Serializable]
    public class Line
    {
        public string speaker;
        public string title;
        public string content;
        public string taskIdToComplete;
        public string taskIdToStart;
        public string gameobjectToEnable; // enable gameobject with this name when this storyline si played
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
