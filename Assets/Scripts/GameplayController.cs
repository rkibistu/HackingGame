using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _deactivateWhileMenu;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            foreach (var obj in _deactivateWhileMenu)
            {
                obj.SetActive(false);
            }
            MenuController.Instance.Show();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            MenuController.Instance.Hide();
            foreach (var obj in _deactivateWhileMenu)
            {
                obj.SetActive(true);
            }
        }
    }
}
