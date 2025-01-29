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

    public void Init(Sprite icon, ApplicationManager target) {
        _iconImage.sprite = icon;
        _targetApp = target;

        _button = GetComponent<Button>();
        _button.onClick.AddListener(HandleClick);
    }

    private void HandleClick() {
        _targetApp.Focus();
    }
}
