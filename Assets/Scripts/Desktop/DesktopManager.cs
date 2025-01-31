using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DesktopManager : MonoBehaviour {
    [SerializeField]
    private TaskbarManager _taskbar;

    [SerializeField]
    private GraphicRaycaster raycaster;
    [SerializeField]
    private EventSystem eventSystem;

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

        //Check if any window was clicked
        if (Input.GetMouseButtonDown(0)) {
            PointerEventData pointerData = new PointerEventData(eventSystem) {
                position = Input.mousePosition
            };

            // Perform a raycast
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);
            

            // Loop through results and find the first UI element with the specified tag
            foreach (RaycastResult result in results) {
                if (result.gameObject.CompareTag("window")) // Check if it has the "Window" tag
                {
                    Focus(result.gameObject.GetComponent<Window>().GetAppAssociated());
                    result.gameObject.GetComponent<Window>().OnClick(pointerData);
                    break;
                }
            }
        }
    }

    public void OpenApplication(ApplicationManager app) {
        // if the applciation was already opened, we want just
        // to focus it; not to try to open it again
        if (app.IsOpen == true) {
            app.Focus();
            return;
        }

        app.Open();
        _taskbar.AddEntry(app);
        Focus(app);
        _runningApps.Add(app.ID, app);
    }

    public void Focus(ApplicationManager app) {
        if (app == null)
            return;

        app.Focus();
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
