using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace WebserverAPI
{
    public class AuthManager : MonoBehaviour
    {
        public async void Login()
        {
            // implement logic for getting username and password from input fields
            string username = "admin";
            string password = "changeme";


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

        public async void Register()
        {
            // implement logic for getting username, password and institution name from input fields
            string email = "admin1";
            string password = "changeme";
            string institutionName = "My Institution";


            var webclient = WebClientService.Instance;
            webclient.Register(email, password, institutionName, (success, message) =>
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
    }
}
