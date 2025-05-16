using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class NamedAudioClip {
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1.0f;

    [Tooltip("Optional: Play the sound at this position. If null, plays at AudioControllerBasic's position.")]
    public Transform playLocation;
}

public class AudioControllerBasic : MonoBehaviour
{
    [Header("Add Audio Clips Here")]
    public List<NamedAudioClip> _audioClips = new List<NamedAudioClip>();

    private Dictionary<string, NamedAudioClip> _audioClipDict = new Dictionary<string, NamedAudioClip>();

    public static AudioControllerBasic Instance { get; private set; }


    private void Awake() {
        // Singletone
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        Instance = this;

        // Populate the dictionary from the list
        foreach (var namedClip in _audioClips) {
            if (!_audioClipDict.ContainsKey(namedClip.name)) {
                _audioClipDict.Add(namedClip.name, namedClip);
            }
            else {
                Debug.LogWarning("Duplicate audio clip name: " + namedClip.name);
            }
        }
    }

    public void PlaySound(string clipName) {
        if (_audioClipDict.TryGetValue(clipName, out NamedAudioClip namedClip)) {
            Vector3 position = namedClip.playLocation != null ? namedClip.playLocation.position : transform.position;
            AudioSource.PlayClipAtPoint(namedClip.clip, position, namedClip.volume);
        }
        else {
            Debug.LogWarning("Sound not found: " + clipName);
        }
    }
}
