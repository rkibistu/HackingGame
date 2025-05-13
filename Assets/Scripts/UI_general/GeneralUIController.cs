using UnityEngine;

/**
 * This should controll which UI panels are open and itneractions with them
 */

public class GeneralUIController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField]
    [Tooltip("Intro paper, opened by letter under the door")]
    private GameObject _letterPanel;
    [SerializeField]
    [Tooltip("Panels with the lsit of tasks")]
    private GameObject _taskPanel;
    [SerializeField]
    [Tooltip("Main menu")]
    private GameObject _menu;
    [SerializeField]
    private GameObject _crosshair;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
