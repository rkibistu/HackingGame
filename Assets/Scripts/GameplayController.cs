using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [SerializeField]
    private int _levelIndex = 0;
    [SerializeField]
    private List<GameObject> _deactivateWhileMenu;

    public static GameplayController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    protected virtual void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MenuController.Instance.Toggle();
            if (MenuController.Instance.IsActive())
            {
                foreach (var obj in _deactivateWhileMenu)
                {
                    obj.SetActive(false);
                }
            }
            else
            {
                foreach (var obj in _deactivateWhileMenu)
                {
                    obj.SetActive(true);
                }
            }
        }
        
        // this should be called when the level is finished
        if (Input.GetKeyDown(KeyCode.K))
        {
            CompleteLevel();
        }
    }

    public void CompleteLevel()
    {
        MenuController.Instance.CompleteLevel(_levelIndex);
    }

    public virtual void StartLevel() {

    }
}
