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
            string username = "admin@cybergame.local";
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
            string email = "admin@cybergame.local";
            string password = "changeme";


            var webclient = WebClientService.Instance;
            webclient.Login(email, password, (success, message) =>
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
    }
}
