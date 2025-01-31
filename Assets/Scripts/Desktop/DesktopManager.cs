using System.Collections.Generic;
using UnityEngine;



public class DesktopManager : MonoBehaviour {
    [SerializeField]
    private TaskbarManager _taskbar;

    private Stack<int> _focusedStack = new Stack<int>();
    private Dictionary<int, ApplicationManager> _runningApps = new Dictionary<int, ApplicationManager>();
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

            //Close focused applciation or desktop
            var focusedApp = GetNextAppInStack();
            if (focusedApp != null) {
                //close current focused app
                _taskbar.RemoveEntry(focusedApp);
                focusedApp.Close();

                //focus next app
                focusedApp = GetNextAppInStack();
                if(focusedApp != null) {
                    Focus(focusedApp);
                    _taskbar.Focus(focusedApp);
                }
            }
            else {
                gameObject.SetActive(false);
            }
        }
    }

    public void OpenApplication(ApplicationManager app) {
        // if the applciation was already opened, we want just
        // to focus it; not to try to open it again
        if (app.IsOpen == true) {
            app.Focus(_maxDepth++);
            return;
        }

        app.Open();
        _taskbar.AddEntry(app);
        Focus(app);
        _runningApps.Add(app.ID, app);
    }


    public void Focus(ApplicationManager app) {
        app.Focus(_maxDepth++);
        _taskbar.Focus(app);
        _focusedStack.Push(app.ID);
    }

    private ApplicationManager GetNextAppInStack() {
        int id;
        ApplicationManager focusedApp = null;
        do {
            if (_focusedStack.Count <= 0)
                return null;

            id = _focusedStack.Pop();
            if (_runningApps.ContainsKey(id) == false)
                continue;

            focusedApp = _runningApps[id];
        } while (focusedApp.IsOpen == false);

        return focusedApp;
    }
}
