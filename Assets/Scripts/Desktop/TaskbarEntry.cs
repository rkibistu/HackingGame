using UnityEngine;
using UnityEngine.UI;


// Used to represent on taskabr an open app. It helps to
// refocus the app and see the open apps
[RequireComponent(typeof(Button))]
public class TaskbarEntry : MonoBehaviour {

    [SerializeField]
    private Image _iconImage;
    
    private Button _button;

    private Color _focusedColor;
    private Color _normalColor;

    private ApplicationManager _targetApp;
    private DesktopManager _desktop;
    public void Init(Sprite icon, ApplicationManager target, DesktopManager dekstop) {
        _iconImage.sprite = icon;
        _targetApp = target;
        _desktop = dekstop;

        _button = GetComponent<Button>();
        _button.onClick.AddListener(HandleClick);

        _focusedColor = _button.colors.selectedColor;
        _normalColor = _button.colors.normalColor;
    }

    public void Focus() {
        ColorBlock cb = _button.colors;
        cb.normalColor = _focusedColor;
        _button.colors = cb;
    }
    public void Defocus() {
        ColorBlock cb = _button.colors;
        cb.normalColor = _normalColor;
        _button.colors = cb;
    }

    private void HandleClick() {
        _desktop.Focus(_targetApp);
    }

    
}
