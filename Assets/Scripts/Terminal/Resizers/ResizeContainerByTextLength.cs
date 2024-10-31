using TMPro;
using UnityEngine;

public class ResizeContainerByTextLength : MonoBehaviour
{
    private TextMeshProUGUI _textMeshPro;
    private RectTransform _rectTransform;

    [SerializeField]
    private float padding = 0.0f;

    void Awake()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
        _rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        AdjustWidth();
    }

    private void AdjustWidth()
    {
        // Get the preferred width based on current text content
        float preferredWidth = _textMeshPro.preferredWidth;

        // Update the RectTransform width with padding (optional)
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, preferredWidth + padding);
    }
}
