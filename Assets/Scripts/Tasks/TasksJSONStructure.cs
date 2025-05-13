using UnityEngine;

public class TasksJSONStructure : MonoBehaviour
{
    [System.Serializable]
    public class Step
    {
        public string id;
        public string title;
        public string description;
        public bool done;
    }

    [System.Serializable]
    public class Task
    {
        public string id;
        public string title;
        public string description;
        public int reward;
        public bool done;
        public bool active; //not all tasks should be displayed in the list. Only active ones. They are activated during gameplay
        public Step[] steps;
    }

    [System.Serializable]
    public class TaskList
    {
        public Task[] tasks;
    }

}
