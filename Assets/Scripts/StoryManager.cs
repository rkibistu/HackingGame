using TMPro;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
  

    public static StoryManager Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    public void PlayStory(string storyId) {

    }
}
