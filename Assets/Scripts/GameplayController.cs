using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayController : MonoBehaviour {
    [SerializeField]
    private int _levelIndex = 0;

    public static GameplayController Instance { get; private set; }

    protected virtual void Awake() {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    protected virtual void Start() {

    }
    protected virtual void Update() {
        // Next line in dialogue
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) {
            if (DialogueController.Instance.IsStoryRunning) {
                DialogueController.Instance.Next();
            }
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            Debug.Log("Unlock cursour from GamePlaycontroller. Debug");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public int GetCurrentLevelIndex() {
        return _levelIndex;
    }

    public virtual void StartLevel() {
    }

    public void ExitToMainMenu() {

    }

    public void EnablePopup(string name) {
        EnableAllChildsOfGameObject(name);
    }
    private void EnableAllChildsOfGameObject(string name) {
        var obj = GameObject.Find(name);
        if (obj == null) {
            Debug.LogWarning("You tried to enable <" + name + "> using one of the jsons file, but a gameobject with this name doesn't exist.");
            return;
        }
        foreach (Transform child in obj.transform) {
            child.gameObject.SetActive(true);
        }
    }
}
