using SlimUI.ModernMenu;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebserverAPI;

public class MenuController : MonoBehaviour
{
    [Tooltip("The gameobject that contains the menu")]
    [SerializeField]
    private GameObject _menuContainer;
    [SerializeField]
    private UILevels _levelsController;
    //public static MenuController Instance { get; private set; }

    [SerializeField]
    private GameProgressManager _gameProcessManager;

    private bool _initialInit = true;

    private void Awake()
    {
        //if (Instance != null && Instance != this)
        //    Destroy(gameObject);

        //Instance = this;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown((KeyCode.Alpha1)))
        {

            _gameProcessManager.UpdateProgressLevel(1, true);
        }
        if (Input.GetKeyDown((KeyCode.Alpha2)))
        {

            _gameProcessManager.UpdateProgressLevel(2, true);
        }
        if (Input.GetKeyDown((KeyCode.Alpha3)))
        {
            _gameProcessManager.UpdateProgressLevel(3, true);
        }
        if (Input.GetKeyDown((KeyCode.Alpha4)))
        {

            _gameProcessManager.UpdateProgressLevel(4,true);
        }
        if (Input.GetKeyDown((KeyCode.Alpha5)))
        {

            _gameProcessManager.UpdateProgressLevel(5, true);
        }

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);

        if (!_initialInit)
        {
            //called every time except on first load at start game
            _menuContainer.SetActive(false);
            UIController.Instance.SetMenuController(this);
        }

        _initialInit = false;
    }
    public void Toggle()
    {
        if (_menuContainer.activeInHierarchy == false)
            Show();
        else
            Hide();

    }
    public void Show()
    {
        _menuContainer.SetActive(true);
    }
    public void Hide()
    {
        _menuContainer.SetActive(false);
    }
    public bool IsActive() { return _menuContainer.activeInHierarchy == true; }
}
