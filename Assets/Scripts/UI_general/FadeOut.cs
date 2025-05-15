using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FadeOut : MonoBehaviour
{
    [SerializeField] 
    private float _fadeOutDuration = 2f;
    [SerializeField]
    private bool _runOnEnable = true;

    private Image _image;
    private float _fadeTimer;
    private bool _isFading = false;


    private void OnEnable() {
        _image = GetComponent<Image>();
        if(_runOnEnable)
            StartFadeOut();
    }

    public void StartFadeOut() {
        if (_image != null) {
            _fadeTimer = _fadeOutDuration;
            _isFading = true;
        }
        else {
            Debug.LogWarning("UIFadeOut: Image reference is missing.");
        }
    }

    private void Update() {
        if (!_isFading || _image == null)
            return;

        _fadeTimer -= Time.deltaTime;
        float alpha = Mathf.Clamp01(_fadeTimer / _fadeOutDuration);

        Color currentColor = _image.color;
        currentColor.a = alpha;
        _image.color = currentColor;

        if (_fadeTimer <= 0f) {
            _isFading = false;
        }
    }
}
