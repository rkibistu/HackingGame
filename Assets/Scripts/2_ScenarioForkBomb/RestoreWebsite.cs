using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RestoreWebsite : MonoBehaviour
{
    [SerializeField]
    private GameObject _recoveryWebpage;
    [SerializeField]
    private GameObject _loginWebpage;
    [SerializeField]
    private Button _recoveryButton;
    private int _recoveryState = 0;
    private string _restoreWebServerString = "Restart Web Server";
    private string _backupWebServerString = "Download Backup";
    public void OnRecoverButtonClicked()
    {
        Text buttonText = _recoveryButton.GetComponentInChildren<Text>();
        if (_recoveryState == 0)
        {
            // Download Backup
            buttonText.text = _restoreWebServerString;
            _recoveryState++;
        }
        else
        {
            // Restart Website
            buttonText.text = _backupWebServerString;
            _recoveryWebpage.SetActive(false);
            _loginWebpage.SetActive(true);
            _recoveryState--;
        }

    }
}
