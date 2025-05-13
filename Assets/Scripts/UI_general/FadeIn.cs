using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FadeIn : MonoBehaviour
{
    [SerializeField] 
    private float _fadeInDuration = 2f;
    [SerializeField]
    private bool _runOnEnable = true;

    private Image _image;
    private float _fadeTimer;
    private bool _isFading = false;


    private void OnEnable() {
        _image = GetComponent<Image>();
        if(_runOnEnable)
            StartFadeIn();
    }

    public void StartFadeIn() {
        if (_image != null) {
            _fadeTimer = 0f;
            _isFading = true;

            // Start from transparent
            Color color = _image.color;
            color.a = 0f;
            _image.color = color;
        }
    }

    private void Update() {
        if (!_isFading || _image == null)
            return;

        _fadeTimer += Time.deltaTime;
        float alpha = Mathf.Clamp01(_fadeTimer / _fadeInDuration); // fade in from 0 to 1

        Color currentColor = _image.color;
        currentColor.a = alpha;
        _image.color = currentColor;

        if (_fadeTimer >= _fadeInDuration) {
            _isFading = false;
        }
    }
}
