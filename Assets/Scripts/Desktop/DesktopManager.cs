using UnityEngine;



public class DesktopManager : MonoBehaviour
{
    [SerializeField]
    private Transform _taskbarLeft;

    private int _maxDepth = 0;

    private void OnEnable() {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        BringInFront(GetComponent<Canvas>());
    }

    private void OnDisable() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void BringInFront(Canvas canvas) {
        _maxDepth++;
        canvas.sortingOrder = _maxDepth;
    }

    public void AddToTaskbar(TaskbarEntry taskbarEntry) {
        taskbarEntry.transform.SetParent(_taskbarLeft);
    }
}
