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

    [Header("Recovery webpage story and task IDs")]
    [SerializeField]
    private string _storyRecoveryWebsiteId;
    [SerializeField]
    private string _taskRecoveryWebsiteId;
 

    [SerializeField]
    private TMP_Text _downloadBackuptMessage;

    private void OnEnable()
    {
        if (_recoveryWebpage.activeSelf)
        {
            if (TasksController.Instance.CheckCurrentTask(_taskRecoveryWebsiteId))
            {
                DialogueController.Instance.PlayStory(_storyRecoveryWebsiteId);
            }
        }
    }

    public void OnRecoverButtonClicked()
    {
        TextMeshProUGUI buttonText = _recoveryButton.GetComponentInChildren<TextMeshProUGUI>();
        if (_recoveryState == 0)
        {
            // Download Backup
            Interpreter.Instance.AdvanceByAction("database_downloaded");
            buttonText.text = _restoreWebServerString;
            _recoveryState++;
            _downloadBackuptMessage.text = "Backup downloaded. Restarting web server...";
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
