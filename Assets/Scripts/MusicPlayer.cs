using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject _visualRoot;
    [SerializeField]
    private bool _startOn = false;

    public AudioSource audioSource;
    public AudioClip[] tracks;
    public TMP_Text trackNameText;

    bool manuallyStopped = false;

    private int currentTrackIndex = 0;

    public RectTransform[] vuBars; // Assign these in the Inspector
    public float sensitivity = 50f;
    public float smoothSpeed = 10f;

    private float[] spectrum = new float[64];
    private float[] bandLevels;

    bool shuffleMode = true;

    void Start()
    {
        bandLevels = new float[vuBars.Length];

        //Always playtrack so you ahve one track loaded when the menu button is pressed
        PlayTrack(currentTrackIndex);
        if (_startOn == true)
        {
            _visualRoot.SetActive(true);
        }
        else
        {
            Pause();
            _visualRoot.SetActive(false);
        }
    }

    void Update()
    {
        if (!manuallyStopped && audioSource.clip != null &&
            !audioSource.isPlaying && audioSource.time >= audioSource.clip.length - 0.1f)
        {
            if (shuffleMode)
                ShuffleTrack();
            else
                NextTrack();
        }

        // Get frequency spectrum
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        // Divide spectrum into bands (evenly for simplicity)
        int samplesPerBand = spectrum.Length / vuBars.Length;

        for (int i = 0; i < vuBars.Length; i++)
        {
            float sum = 0f;
            for (int j = 0; j < samplesPerBand; j++)
            {
                int index = i * samplesPerBand + j;
                sum += spectrum[index];
            }

            float avg = sum / samplesPerBand;
            float target = avg * sensitivity;

            // Smooth transition
            bandLevels[i] = Mathf.Lerp(bandLevels[i], target, Time.deltaTime * smoothSpeed);

            float scaleY = Mathf.Clamp01(bandLevels[i]);
            vuBars[i].localScale = new Vector3(1, scaleY, 1);
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

    public void Toggle()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            _visualRoot.SetActive(true);
        }
        else
        {
            audioSource.Pause();
            _visualRoot.SetActive(false);
        }
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

    public void ShuffleTrack()
    {
        if (tracks.Length <= 1) return;

        int newIndex;

        do
        {
            newIndex = Random.Range(0, tracks.Length);
        } while (newIndex == currentTrackIndex); // avoid repeating current track

        PlayTrack(newIndex);
    }

}


public class VUMeter : MonoBehaviour
{
    public AudioSource audioSource;
    public RectTransform vuBar; // UI bar to scale
    public float sensitivity = 100f;
    public float smoothSpeed = 10f;

    float[] samples = new float[256];
    float currentVolume = 0f;

    void Update()
    {
        if (audioSource == null || vuBar == null) return;

        // Get audio samples
        audioSource.GetOutputData(samples, 0);

        // Compute RMS (Root Mean Square) for perceived loudness
        float sum = 0f;
        foreach (float sample in samples)
        {
            sum += sample * sample;
        }

        float rms = Mathf.Sqrt(sum / samples.Length);
        float targetVolume = rms * sensitivity;

        // Smooth scaling
        currentVolume = Mathf.Lerp(currentVolume, targetVolume, Time.deltaTime * smoothSpeed);

        // Update UI height (scale Y)
        vuBar.localScale = new Vector3(1, Mathf.Clamp01(currentVolume), 1);
    }
}