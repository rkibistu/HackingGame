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
    private string _taskAfterSuccessLoginPageId;
    [SerializeField]
    private string _tastCurrentAllowedLoginPageId;

    void Start()
    {
        //_loginButton.onClick.AddListener(OnLoginButtonClicked);
        _errorLoginText.gameObject.SetActive(false);
        _errorLoginText.text = "Password or username incorrect!";
        _submitWebpage.SetActive(false);
    }

    private void OnEnable()
    {
        _errorLoginText.gameObject.SetActive(false);
    }

    public void OnLoginButtonClicked()
    {
        if (TasksController.Instance.CheckCurrentTask(_tastCurrentAllowedLoginPageId))
        {


            string enteredUsername = _usernameInputField.text;
            string enteredPassword = _passwordInputField.text;

            if (enteredUsername == correctUsername && enteredPassword == correctPassword)
            {
                TasksController.Instance.Mark("login-website");
                TasksController.Instance.ActivateTask(_taskAfterSuccessLoginPageId);
                ClearPanel();
                _loginWebpage.SetActive(false);
                _submitWebpage.SetActive(true);
                _errorLoginText.text = "Password or username incorrect!";
                _errorLoginText.gameObject.SetActive(false); // Disable error text if it was previously active.   
            }
            else
            {
                _errorLoginText.text = "Password or username incorrect!";
                _errorLoginText.gameObject.SetActive(true);
            }
        }
        else
        {
            _errorLoginText.text = "Check the credentials file first!";
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