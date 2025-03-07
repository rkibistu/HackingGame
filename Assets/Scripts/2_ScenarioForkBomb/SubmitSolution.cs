using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubmitSolution : MonoBehaviour
{
    [SerializeField]
    private GameObject _recoveryWebpage;
    [SerializeField]
    private GameObject _submitWebpage;
    [SerializeField]
    private Button _submitButton;
    [SerializeField]
    private TerminalManager _terminalManager;

    public void OnSubmitButtonClicked()
    {
        int phase = Interpreter.Instance.GetPhase(_terminalManager.Name);
        Debug.Log("Phase: " + phase);
        if (phase == 1)
        {
            _recoveryWebpage.SetActive(true);
            _submitWebpage.SetActive(false);
        }
        else
        {
            Debug.Log("TODO");
        }

    }
}
