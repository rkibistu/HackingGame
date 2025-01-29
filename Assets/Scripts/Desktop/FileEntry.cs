using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

// Should be added to a clcickable folder/file/app icon
// It is used to open a folder/file/app etc

[RequireComponent(typeof(Button))]
public class FileEntry : MonoBehaviour {
    [SerializeField]
    private ApplicationManager _toOpen;
    [SerializeField]
    private Image _iconImage;
    [SerializeField]
    private TextMeshProUGUI _nameText;

    private float _doubleClickTime = 0.3f;
    private int _clickCount = 0;
    private Button _button;

    private void Start() {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(HandleClick);

        if (_toOpen != null) {

            _iconImage.sprite = _toOpen.GetIcon();
            _nameText.text = _toOpen.GetName();
        }
    }

    private void HandleClick() {
        _clickCount++;

        if (_clickCount == 1) {
            StartCoroutine(ClickTimer());
        }
        else if (_clickCount == 2) {
            DoubleClickAction();
            _clickCount = 0;
        }
    }

    void DoubleClickAction() {
        _toOpen?.gameObject.SetActive(true);
    }

    private IEnumerator ClickTimer() {
        yield return new WaitForSeconds(_doubleClickTime);
        _clickCount = 0;
    }
}
