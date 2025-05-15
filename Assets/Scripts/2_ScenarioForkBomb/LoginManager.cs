using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _usernameInputField;
    [SerializeField]
    private TMP_InputField _passwordInputField;
    [SerializeField]
    private Button _loginButton;
    [SerializeField]
    private Button _logoutButton;
    [SerializeField]
    private GameObject _loginWebpage;
    [SerializeField]
    private GameObject _submitWebpage;
    [SerializeField]
    private TextMeshProUGUI _errorLoginText;

    [Header("Login credentials")]
    [SerializeField]
    private string correctUsername = "admin";
    [SerializeField]
    private string correctPassword = "admin";

    [Header("Login story and task IDs")]
    [SerializeField]
    private string _storyLoginId;
    [SerializeField]
    private string _taskBeforeLoginPageId;
    [SerializeField]
    private string _taskAfterLoginPageId;
    [SerializeField]
    private string _taskAfterSuccessLoginPageId;

    void Start()
    {
        //_loginButton.onClick.AddListener(OnLoginButtonClicked);
        _errorLoginText.gameObject.SetActive(false);
        _submitWebpage.SetActive(false);
    }

    void OnEnable()
    {
        if (_loginWebpage.activeSelf)
        {
            if (TasksController.Instance.CheckCurrentTask(_taskBeforeLoginPageId))
            {
                TasksController.Instance.ActivateTask(_taskAfterLoginPageId);
                DialogueController.Instance.PlayStory(_storyLoginId);
            }
        }
    }

    public void OnLoginButtonClicked()
    {
        string enteredUsername = _usernameInputField.text;
        string enteredPassword = _passwordInputField.text;

        if (enteredUsername == correctUsername && enteredPassword == correctPassword)
        {
            Debug.Log("Login successful" + _taskAfterSuccessLoginPageId);

            TasksController.Instance.ActivateTask(_taskAfterSuccessLoginPageId);
            ClearPanel();
            _loginWebpage.SetActive(false);
            _submitWebpage.SetActive(true);
            _errorLoginText.gameObject.SetActive(false); // Disable error text if it was previously active.   
        }
        else
        {
            _errorLoginText.gameObject.SetActive(true);
        }
    }

    public void OnLogoutButtonClicked()
    {
            _loginWebpage.SetActive(true);
            _submitWebpage.SetActive(false);
    }

    private void ClearPanel()
    {
        _errorLoginText.gameObject.SetActive(false);
        _usernameInputField.text = "";
        _passwordInputField.text = "";
    }
}
