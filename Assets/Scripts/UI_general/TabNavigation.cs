using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TabNavigation : MonoBehaviour
{
    [Tooltip("List of TMP_InputFields")]
    public TMP_InputField[] fields;

    int current = 0;

    void Start()
    {
        
    }

    void Update()
    {
        if (fields.Length == 0) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            current = shift
                ? (current - 1 + fields.Length) % fields.Length
                : (current + 1) % fields.Length;
            SelectField(current);
        }
    }

    void SelectField(int idx)
    {
        var f = fields[idx];
        EventSystem.current.SetSelectedGameObject(f.gameObject, null);
        f.ActivateInputField();
    }
}
