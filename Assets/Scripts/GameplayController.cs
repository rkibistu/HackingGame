using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using WebserverAPI;

public class GameplayController : MonoBehaviour
{
    [SerializeField]
    private int _levelIndex = 0;
    [Tooltip("Time to wait before changing scene after the end of level apnel is displayed")]
    [SerializeField]
    private int _endOfLevelDelay = 3;


    public static GameplayController Instance { get; private set; }

    private GameProgressManager _gameProgressManager;

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    protected virtual void Start()
    {
        _gameProgressManager = GetComponent<GameProgressManager>();
    }
    protected virtual void Update()
    {
        // Next line in dialogue
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            if (DialogueController.Instance.IsStoryRunning)
            {
                DialogueController.Instance.Next();
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Unlock cursour from GamePlaycontroller. Debug");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public int GetCurrentLevelIndex()
    {
        return _levelIndex;
    }

    public virtual void StartLevel()
    {
    }
    public virtual void EndLevel()
    {
        StartCoroutine(ChangeSceneWithDelay());
        UIController.Instance.SetActiveEndLevelPanel(true);
    }

    public void ExitToMainMenu()
    {

    }

    public void EnablePopup(string name)
    {
        EnableAllChildsOfGameObject(name);
    }

    private IEnumerator ChangeSceneWithDelay()
    {
        // Wait for the end of level panel to be displayed
        _gameProgressManager?.UpdateProgressLevel(_levelIndex + 1);
        yield return new WaitForSeconds(_endOfLevelDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void EnableAllChildsOfGameObject(string name)
    {
        var obj = GameObject.Find(name);
        if (obj == null)
        {
            Debug.LogWarning("You tried to enable <" + name + "> using one of the jsons file, but a gameobject with this name doesn't exist.");
            return;
        }
        foreach (Transform child in obj.transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
