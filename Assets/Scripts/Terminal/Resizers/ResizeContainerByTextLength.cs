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

        if(Input.GetKeyDown(KeyCode.Space)) {

            Debug.Log("---");
            Debug.Log(GetCharacterWidth('a'));
            Debug.Log(GetCharacterWidth('b'));
            Debug.Log(GetCharacterWidth('c'));
            Debug.Log(GetCharacterWidth(';'));
        }
    }

    private void AdjustWidth()
    {
        // Get the preferred width based on current text content
        float preferredWidth = _textMeshPro.preferredWidth;

        // Update the RectTransform width with padding (optional)
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, preferredWidth + padding);
    }

    private float GetCharacterWidth(char character)
    {
        // Create a temporary TextMeshProUGUI instance to measure the character
        TextMeshProUGUI tempText = new GameObject("TempText").AddComponent<TextMeshProUGUI>();
        tempText.font = _textMeshPro.font; // Use the same font as the sample text field

        // Set the text to the single character
        tempText.text = character.ToString();

        // Calculate the preferred width
        float width = tempText.GetPreferredValues().x;

        // Destroy the temporary GameObject to clean up
        Destroy(tempText.gameObject);

        return width;
    }
}
