using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace WebserverAPI
{
    public class AuthManager : MonoBehaviour
    {
        [Header("Login Fields")]
        [SerializeField] private TMP_InputField usernameLoginField;
        [SerializeField] private TMP_InputField passwordLoginField;
        public void Login()
        {
            // implement logic for getting username and password from input fields
            string username = usernameLoginField.text;
            string password = passwordLoginField.text;


            var webclient = WebClientService.Instance;
            webclient.Login(username, password, (success, message) =>
            {
                if (success)
                {
                    // implement login success logic here -> redirect to first game
                    Debug.Log("Login successful: " + message);
                }
                else
                {
                    // implement login failed logic here -> remain in login page, display login error message
                    Debug.LogError("Login failed: " + message);
                }
            });

            
        }

        [Header("Register Fields")]
        [SerializeField] private TMP_InputField usernameRegistrationField;
        [SerializeField] private TMP_InputField passwordRegistrationField;
        [SerializeField] private TMP_InputField institutionRegistrationField;

        public void Register()
        {
            // implement logic for getting username, password and institution name from input fields
            string username = usernameRegistrationField.text;
            string password = passwordRegistrationField.text;
            string institutionName = institutionRegistrationField.text;


            var webclient = WebClientService.Instance;
            webclient.Register(username, password, institutionName, (success, message) =>
            {
                if (success)
                {
                    // implement register success logic here -> redirect to login page
                    Debug.Log("Registration successful: " + message);
                }
                else
                {

                    if (message.Contains("Username already exists!")){
                        // implement logic to inform user that the username already exists
                        Debug.LogError("Username already exists: " + message);
                    }
                    else
                    {
                        // implement login failed logic here -> remain in register page, display register error message
                        Debug.LogError("Registration failed: " + message);
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
    }
}
