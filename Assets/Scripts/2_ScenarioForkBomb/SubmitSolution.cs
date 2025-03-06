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

    public void OnSubmitButtonClicked()
    {
        _recoveryWebpage.SetActive(true);
        _submitWebpage.SetActive(false);
    }
}
