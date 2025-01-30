using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

// This class should be added to every canvas that represents a dekstop window
// It handles the focus of the window (the depth of canvas so the focused oen to be first visible)
[RequireComponent(typeof(Canvas))]
public class ApplicationManager : MonoBehaviour
{
    [SerializeField]
    private Sprite _iconSprite;
    [SerializeField]
    private string _name;

    private Canvas _canvas;

    private static int _id = 0;
    public int ID { get; private set; }

    public void Init() {
        ID = _id++;
        _canvas = GetComponent<Canvas>();
    }

    public void Close() {
        this.gameObject.SetActive(false);
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
