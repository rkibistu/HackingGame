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

        //not in json, used at runtime
        public TaskRow row = null;
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
        public string storyIdToStart; //start this story when this task is complete
        public string taskIdToStart; //start this task when this task is complete
        public string taskIdToComplete; //end this task when this task is complete
        public string gameobjectToEnableOnCompletion; // enable gameobject with this name when this task is complete
        public string gameobjectToEnableOnActivation; // enable gameobject with this name when this task is activate

        //not in json, used at runtime
        public TaskRow row = null;
    }

    [System.Serializable]
    public class TaskList
    {
        public Task[] tasks;
    }

}
