using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;


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

    [Header("Settings")]
    [SerializeField]
    private AudioMixer _audioMixer;
    [SerializeField]
    private AudioMixerGroup _sfxMixerGroup;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SetSFXVolume(0.2f);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            SetSFXVolume(1.0f);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            PlaySound("knock-door");
        }
    }

    public void PlaySound(string clipName) {
        if (_audioClipDict.TryGetValue(clipName, out NamedAudioClip namedClip)) {
            Vector3 position = namedClip.playLocation != null ? namedClip.playLocation.position : transform.position;
            //AudioSource.PlayClipAtPoint(namedClip.clip, position, namedClip.volume);

            GameObject tempGO = new GameObject("TempAudio_" + clipName);
            tempGO.transform.position = position;

            AudioSource aSource = tempGO.AddComponent<AudioSource>();
            aSource.clip = namedClip.clip;
            aSource.volume = namedClip.volume;
            aSource.outputAudioMixerGroup = _sfxMixerGroup;
            aSource.spatialBlend = 0f; // Make it 2D if needed
            aSource.Play();

            Destroy(tempGO, namedClip.clip.length + 0.1f); // Clean up after playback
        }
        else {
            Debug.LogWarning("Sound not found: " + clipName);
        }
    }



    public void SetSFXVolume(float volume01)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume01, 0.0001f, 1f)) * 20f;
        _audioMixer.SetFloat("SFXVolume", dB);
    }
}
