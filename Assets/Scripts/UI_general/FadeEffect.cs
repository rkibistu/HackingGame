using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FadeEffect : MonoBehaviour
{
    [SerializeField]
    private float _fadeOutDuration = 2f;
    [SerializeField]
    private float _fadeInDuration = 2f;
    [SerializeField]
    private bool _runOnEnable = true;


    private Image _image;
    private float _fadeTimer;
    private bool _isFading = false;
    private bool _in = true;


    private void OnEnable()
    {
        _image = GetComponent<Image>();
        if (_runOnEnable)
            StartFadeOut();
    }

    public void StartFadeOut()
    {
        if (_image != null)
        {
            _fadeTimer = _fadeOutDuration;
            _isFading = true;
            _in = false;

            // Start from transparent
            Color color = _image.color;
            color.a = 1f;
            _image.color = color;
        }
        else
        {
            Debug.LogWarning("UIFadeOut: Image reference is missing.");
        }
    }

    public void StartFadeIn()
    {
        if (_image != null)
        {
            _fadeTimer = 0f;
            _isFading = true;
            _in = true;

            // Start from transparent
            Color color = _image.color;
            color.a = 0f;
            _image.color = color;
        }
    }

    public void FadeOutUpdate()
    {
        if (!_isFading || _image == null)
            return;

        _fadeTimer -= Time.deltaTime;
        float t = Mathf.Clamp01(1 - _fadeTimer / _fadeOutDuration);

        // Use non-linear easing for smoother fade
        float easedAlpha = 1f - Mathf.Pow(t, 2f); // ease-out quad

        Color currentColor = _image.color;
        currentColor.a = easedAlpha;
        _image.color = currentColor;

        Debug.Log(easedAlpha + "  " + t + "  " + _fadeTimer);
   
        if (_fadeTimer <= 0f)
        {
            _isFading = false;
        }
    }

    public void FadeInUpdate()
    {
        if (!_isFading || _image == null)
            return;

        _fadeTimer += Time.deltaTime;
        float t = Mathf.Clamp01(_fadeTimer / _fadeInDuration);

        // Use non-linear easing for smoother fade
        float easedAlpha = Mathf.Pow(t, 2f); // ease-out quad

        Color currentColor = _image.color;
        currentColor.a = easedAlpha;
        _image.color = currentColor;

        if (_fadeTimer >= _fadeInDuration)
        {
            _isFading = false;
        }
    }

    private void Update()
    {

        if (_in == true)
        {
            FadeInUpdate();
        }
        else
        {
            FadeOutUpdate();
        }
    }
}
