using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

// This class should be added to every canvas that represents a dekstop window
// It handles the focus of the window (the depth of canvas so the focused oen to be first visible)
[RequireComponent(typeof(Canvas))]
public class ApplicationManager : MonoBehaviour {
    [Tooltip("The actual window of the app. This will be enabled and disabled to simulate the app opening and closing")]
    [SerializeField]
    private GameObject _window;

    [Tooltip("The icon used to represent the app")]
    [SerializeField]
    private Sprite _iconSprite;
    [Tooltip("The name used to represent the app")]
    [SerializeField]
    private string _name;

    private Canvas _canvas;

    public bool IsOpen { get { return _window.activeInHierarchy; } }
    public int ID { get; private set; }

    private static int _id = 0;

    public void Open() {
        ID = _id++;
        _canvas = GetComponent<Canvas>();
        _window.SetActive(true);
    }

    public void Close() {
        _window.SetActive(false);
    }

    public void Focus(int sortingOrder) {
        _canvas.sortingOrder = sortingOrder;
    }

    public Sprite GetIcon() {
        return _iconSprite;
    }
    public string GetName() {
        return _name;
    }

}
