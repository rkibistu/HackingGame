using UnityEngine;
using UnityEngine.UI;


// Used to represent on taskabr an open app. It helps to
// refocus the app and see the open apps
[RequireComponent(typeof(Button))]
public class TaskbarEntry : MonoBehaviour {

    [SerializeField]
    private Image _iconImage;
    private Button _button;

    private ApplicationManager _targetApp;
    private DesktopManager _desktop;
    public void Init(Sprite icon, ApplicationManager target, DesktopManager dekstop) {
        _iconImage.sprite = icon;
        _targetApp = target;
        _desktop = dekstop;

        _button = GetComponent<Button>();
        _button.onClick.AddListener(HandleClick);
    }

    public void Focus() {
        
    }

    private void HandleClick() {
        _desktop.Focus(_targetApp);
    }
}
