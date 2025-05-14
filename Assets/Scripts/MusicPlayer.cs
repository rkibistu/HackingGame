using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] tracks;
    public Text trackNameText;

    bool manuallyStopped = false;

    private int currentTrackIndex = 0;

    void Start()
    {
        PlayTrack(currentTrackIndex);
    }

    void Update()
    {
        if (audioSource.clip != null && !manuallyStopped)
        {
            // Use a small threshold to detect end of track
            if (!audioSource.isPlaying && audioSource.time >= audioSource.clip.length - 0.1f)
            {
                NextTrack();
            }
        }
    }

    public void PlayTrack(int index)
    {
        if (index < 0 || index >= tracks.Length) return;

        currentTrackIndex = index;
        audioSource.clip = tracks[currentTrackIndex];
        audioSource.Play();

        if (trackNameText != null)
            trackNameText.text = audioSource.clip.name;
    }

    public void Play()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    public void Pause()
    {
        if (audioSource.isPlaying)
            audioSource.Pause();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void NextTrack()
    {
        int next = (currentTrackIndex + 1) % tracks.Length;
        PlayTrack(next);
    }

    public void PreviousTrack()
    {
        int prev = (currentTrackIndex - 1 + tracks.Length) % tracks.Length;
        PlayTrack(prev);
    }
}
