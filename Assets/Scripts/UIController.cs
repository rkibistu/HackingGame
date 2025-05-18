using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/***
 * This manages all the panel during 
 * 
 */

public class UIController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField]
    [Tooltip("Intro paper, opened by letter under the door")]
    private GameObject _letterPanel;
    [SerializeField]
    [Tooltip("Panel with the lsit of tasks")]
    private GameObject _taskPanel;
    [SerializeField]
    [Tooltip("Panel with the story dialogue")]
    private GameObject _storyPanel;
    [Tooltip("The menu used in game with all setting and help panels")]
    [SerializeField]
    private IngameMenuController _ingameMenu;


    [Header("Elements")]
    [SerializeField]
    private GameObject _crosshair;
    [SerializeField]
    TextMeshProUGUI _hintInteractiveText;
    [SerializeField]
    private GameObject _currentObjectivePanel;
    [SerializeField]
    private GameObject _endLevelPanel;
    [SerializeField]
    private GeneralFeedbackPanel _generalFeedbackPanel;

    [Header("MenuRelated")]
    [SerializeField]
    private List<GameObject> _deactivateWhileMenu;

    public bool CanOpenIngameMenu { get => _canOpenIngameMenu; set { _canOpenIngameMenu = value; } }

    // This exist in the first scene of the game and it is not destroyed
    // This variable will be populated on scene load
    // It is used to work with menu
    private MenuController _menu;
    // used after you close the menu to go back to the last state of the cusor
    private CursorLockMode _lastCursorLockMode;
    private bool _lastCursorVisibility;

    //we don't want to open menu when an input field is focues
    // So every inptufield ahs a script that change this variable when they are focused
    private bool _canOpenIngameMenu = true;

    public static UIController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && CanOpenIngameMenu == true)
        {
            _ingameMenu.Toggle();
        }

        // We don't want to accept other keyboard input if the IngameMenu is active
        if (_ingameMenu.IsOpen())
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsActiveLetterPanel())
            {
                SetActiveLetterPanel(false);
            }
        }
    }

    public void SetMenuController(MenuController menu)
    {
        _menu = menu;
    }

    public void ShowHint(string hintText)
    {
        _hintInteractiveText.enabled = true;
        _hintInteractiveText.text = hintText;
    }

    public void HideHint()
    {
        _hintInteractiveText.enabled = false;
    }

    public void SetActiveStoryPanel(bool active)
    {
        _storyPanel.SetActive(active);
    }
    public bool IsActiveStoryPanel()
    {
        return _storyPanel.activeInHierarchy;
    }

    public void SetActiveLetterPanel(bool active)
    {
        if (active == true)
        {
            // We don't want story on the back oif a letter
            DialogueController.Instance.SkipCurrentStoryCompletely();
            _storyPanel.SetActive(false);
        }

        _letterPanel.SetActive(active);
    }
    public bool IsActiveLetterPanel()
    {
        return _letterPanel.activeInHierarchy;
    }

    public void SetActiveTaskPanel(bool active)
    {
        _taskPanel.SetActive(active);
    }
    public bool IsActiveTaskPanel()
    {
        return _taskPanel.activeInHierarchy;
    }

    public void SetActiveEndLevelPanel(bool active)
    {
        _endLevelPanel?.SetActive(active);
        if(active == true && _ingameMenu.IsOpen())
        {
            _ingameMenu.Toggle();
        }
    }
    public void SetActiveCurrentObjectivePanel(bool active)
    {
        _currentObjectivePanel.SetActive(active);
    }
    public bool IsActiveCurrentObjectivePanel()
    {
        return _currentObjectivePanel.activeInHierarchy;
    }

    public void ShowAndSetGeneralFeedbackPanel(string content, int timeToShow = 5)
    {
        _generalFeedbackPanel.SetText(content);
        _generalFeedbackPanel.DisplayLimited(timeToShow);
    }
}
