using System.Collections.Generic;
using UnityEngine;



public class DesktopManager : MonoBehaviour {
    [SerializeField]
    private Transform _taskbarLeft;

    private Stack<ApplicationManager> _focusedStack = new Stack<ApplicationManager>();
    private int _maxDepth = 0;

    private void OnEnable() {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        GetComponent<Canvas>().sortingOrder = _maxDepth++;
    }
    private void OnDisable() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (_focusedStack.Count > 0) {

                var focusedApp = GetNextAppInStack();
                focusedApp?.Close();
            }
            else {
                gameObject.SetActive(false);
            }
        }
    }

    public void OpenApplication(ApplicationManager app) {
        if (app.gameObject.activeInHierarchy == true) {
            app.Focus(_maxDepth++);
            return;
        }

        app.gameObject.SetActive(true);
        Focus(app);
        app.Init();
    }

    public void Focus(ApplicationManager app) {
        app.Focus(_maxDepth++);
        _focusedStack.Push(app);
    }

    public void AddToTaskbar(TaskbarEntry taskbarEntry) {
        taskbarEntry.transform.SetParent(_taskbarLeft);
    }

    private ApplicationManager GetNextAppInStack() {
        ApplicationManager focusedApp = null;
        do {
            if (_focusedStack.Count <= 0)
                return null;
            focusedApp = _focusedStack.Pop();
        } while (focusedApp.gameObject.activeInHierarchy == false);

        return focusedApp;
    }
}
