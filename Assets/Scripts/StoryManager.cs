using TMPro;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _speakerText;
    [SerializeField]
    private TextMeshProUGUI _contentText;

    public static StoryManager Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    public void PlayStory(string storyId) {

    }
}
