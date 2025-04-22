using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Tooltip("The gameobject that contains the menu")]
    [SerializeField]
    private GameObject _menuContainer;
    public static MenuController Instance { get; private set; }

    private bool _initialInit = true;
    private Camera _gameCamera = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);

        if (!_initialInit)
        {
            //called every time except on first load at start game
            _menuContainer.SetActive(false);
        }

        _initialInit = false;
    }

    public void Show()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _menuContainer.SetActive(true);
    }
    public void Hide()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _menuContainer.SetActive(false);
    }

}
