using UnityEngine;

public class ResaizeContainerWidthToFillContainer : MonoBehaviour
{
    private RectTransform _rectTransform;
    [SerializeField]
    private RectTransform _parentRectTransform;
    [SerializeField]
    private RectTransform _siblingRectTransform;

    [SerializeField]
    private float _padding = 0.0f;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _parentRectTransform = _rectTransform.parent.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetWidth();
        }
    }

    private void SetWidth()
    {
        float width = _parentRectTransform.rect.width - _siblingRectTransform.rect.width - _padding;
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        Debug.Log(width);
    }
}
