using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

// This class should be added to every canvas that represents a dekstop window
// It handles the focus of the window (the depth of canvas so the focused oen to be first visible)
public class ApplicationManager : MonoBehaviour {
    [Tooltip("The actual window of the app. This will be enabled and disabled to simulate the app opening and closing")]
    [SerializeField]
    private Window _window;

    [Tooltip("The icon used to represent the app")]
    [SerializeField]
    private Sprite _iconSprite;
    [Tooltip("The name used to represent the app")]
    [SerializeField]
    private string _name;

    public bool IsOpen { get { return _window.gameObject.activeInHierarchy; } }
    public int ID { get; private set; }

    private static int _id = 0;

    public void Open() {
        ID = _id++;

        _window.gameObject.SetActive(true);
        _window.SetAppAssociated(this);
    }

    public void Close() {
        _window.SetAppAssociated(null);
        _window.gameObject.SetActive(false);
    }

    public void Focus() {
        gameObject.transform.SetAsLastSibling();
    }

    public Sprite GetIcon() {
        return _iconSprite;
    }
    public string GetName() {
        return _name;
    }

}
