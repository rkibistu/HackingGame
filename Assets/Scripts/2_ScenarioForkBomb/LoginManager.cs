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

    public string correctUsername = "admin"; // Hardcoded username
    public string correctPassword = "admin"; // Hardcoded password

    void Start()
    {
        //_loginButton.onClick.AddListener(OnLoginButtonClicked);
        _errorLoginText.gameObject.SetActive(false);
        _submitWebpage.SetActive(false);
    }

    public void OnLoginButtonClicked()
    {
        string enteredUsername = _usernameInputField.text;
        string enteredPassword = _passwordInputField.text;

        if (enteredUsername == correctUsername && enteredPassword == correctPassword)
        {
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
