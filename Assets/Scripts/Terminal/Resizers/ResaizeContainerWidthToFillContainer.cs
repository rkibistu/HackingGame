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
    }

    void Update()
    {
        SetWidth();
    }

    private void SetWidth()
    {
        float siblingWith = _siblingRectTransform ? _siblingRectTransform.rect.width : 0;
        float width = _parentRectTransform.rect.width - siblingWith - _padding;
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }
}
