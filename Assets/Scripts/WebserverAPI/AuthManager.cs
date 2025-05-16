using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SlimUI.ModernMenu;

namespace WebserverAPI
{
    public class AuthManager : MonoBehaviour
    {
        [SerializeField]
        private UIMenuManager _menuManager;

        [Header("Error Fields")]
        [SerializeField] private TMP_Text errorLoginField;
        [SerializeField] private TMP_Text errorRegisterField;

        [Header("Login Fields")]
        [SerializeField] private TMP_InputField usernameLoginField;
        [SerializeField] private TMP_InputField passwordLoginField;

        [Header("Register Fields")]
        [SerializeField] private TMP_InputField usernameRegistrationField;
        [SerializeField] private TMP_InputField passwordRegistrationField;
        [SerializeField] private TMP_InputField institutionRegistrationField;

        public void Login()
        {
            errorLoginField.text = string.Empty;

            // implement logic for getting username and password from input fields
            string username = usernameLoginField.text;
            string password = passwordLoginField.text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) )
            {
                Debug.LogError("All fields are required.");
                errorLoginField.text = "All fields are required.";
                return;
            }

            var webclient = WebClientService.Instance;
            webclient.Login(username, password, (success, message) =>
            {
                if (success)
                {
                    // implement login success logic here -> redirect to first game
                    //Debug.Log("Login successful: " + message);
                    ClearAllInputFields();
                    _menuManager.SwitchToMainMenu();
                }
                else
                {
                    // implement login failed logic here -> remain in login page, display login error message
                    Debug.LogError("Login failed: " + message);
                    ClearAllInputFields();
                    errorLoginField.text = "The username or password is invalid.";
                }
            });

            
        }

        public void Register()
        {
            errorRegisterField.text = string.Empty;
            // implement logic for getting username, password and institution name from input fields
            string username = usernameRegistrationField.text;
            string password = passwordRegistrationField.text;
            string institutionName = institutionRegistrationField.text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(institutionName))
            {
                Debug.LogError("All fields are required.");
                errorRegisterField.text = "All fields are required.";
                return;
            }

            var webclient = WebClientService.Instance;
            webclient.Register(username, password, institutionName, (success, message) =>
            {
                if (success)
                {
                    // implement register success logic here -> redirect to login page
                    Debug.Log("Registration successful: " + message);
                    _menuManager.SwitchToMainMenu();
                }
                else
                {
                    if (message.Contains("Username already exists!")){
                        // implement logic to inform user that the username already exists
                        Debug.LogError("Username already exists: " + message);
                        ClearAllInputFields();
                        errorRegisterField.text = "Username already exists.";
                    }
                    else
                    {
                        // implement login failed logic here -> remain in register page, display register error message
                        Debug.LogError("Registration failed: " + message);
                        ClearAllInputFields();
                        errorRegisterField.text = "Registration failed.";
                    }
                }
            });


        }

        public void Logout()
        {
            var webclient = WebClientService.Instance;
            webclient.Logout((success, message) =>
            {
                if (success)
                {
                    // implement logout success logic here -> redirect to login page
                    Debug.Log("Logout successful: " + message);
                }
                else
                {
                    // implement logout failed logic here -> remain in game, display logout error message
                    Debug.LogError("Logout failed: " + message);
                }
            });
        }

        /// <summary>
        /// Clear all the input fields in the login and registration panels.
        /// </summary>
        public void ClearAllInputFields()
        {
            usernameLoginField.text = string.Empty;
            passwordLoginField.text = string.Empty;
            usernameRegistrationField.text = string.Empty;
            passwordRegistrationField.text = string.Empty;
            institutionRegistrationField.text = string.Empty;
            errorLoginField.text = string.Empty;
            errorRegisterField.text = string.Empty;
        }

    }
}
