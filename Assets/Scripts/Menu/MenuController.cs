using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Tooltip("The gameobject that contains the menu")]
    [SerializeField]
    private GameObject _menuContainer;
    [Tooltip("The camera used doring gameplay. This should be inactive while menu is open")]
    [SerializeField]
    private GameObject _gameCamera;

    private bool _initialInit = true;

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

    
}
