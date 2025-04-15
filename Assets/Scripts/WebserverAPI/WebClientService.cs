using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace WebserverAPI
{
    /// <summary>
    /// Validation of the certificate is disable at the moment.
    /// 
    /// </summary>
    public class AcceptAllCertificatesHandler : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }



    /// <summary>
    /// Singleton client to interact with a web server
    /// </summary>
    public class WebClientService : MonoBehaviour
    {
        private static WebClientService _instance;

        private string _baseUrl = "https://0x67616d65.xyz/";
        private string _accessToken = string.Empty;
        private string _refreshToken = string.Empty;

        public bool IsAuthenticated => !string.IsNullOrEmpty(_accessToken);

        public string BaseUrl
        {
            get => _baseUrl;
            set => _baseUrl = value;
        }

        public string AccessToken
        {
            get => _accessToken;
            set => _accessToken = value;
        }
        
        public string RefreshToken
        {
            get => _refreshToken;
            set => _refreshToken = value;
        }
        public static WebClientService Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("WebClientAPI");
                    _instance = go.AddComponent<WebClientService>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private string EscapeString(string s)
        {
            return s.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private string DictionaryToJson(Dictionary<string, string> dict)
        {
            List<string> entries = new List<string>();
            foreach (var kvp in dict)
            {
                entries.Add($"\"{EscapeString(kvp.Key)}\":\"{EscapeString(kvp.Value)}\"");
            }
            return "{" + string.Join(",", entries) + "}";
        }


        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="username">User's username</param>
        /// <param name="password">User's password</param>
        /// <param name="callback">Callback to handle the result</param>
        public void Login(string username, string password, Action<bool, string> callback)
        {
            Dictionary<string, string> payload = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password }
            };

            StartCoroutine(PostRequest("login", payload, (success, response) =>
            {
                if (success)
                {
                    try
                    {
                        // expected response { "access_token": ..., "refresh_token": ... }
                        // parse and store tokens
                        LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(response);

                        if (!string.IsNullOrEmpty(loginResponse.access_token) && !string.IsNullOrEmpty(loginResponse.refresh_token))
                        {
                            _accessToken = loginResponse.access_token;
                            _refreshToken = loginResponse.refresh_token;
                            Debug.Log(_accessToken);
                            callback(true, "Login successful");
                        }
                        else
                        {
                            callback(false, "Auth tokens not found in response");
                        }
                    }
                    catch (Exception ex)
                    {
                        callback(false, $"Failed to parse login response: {ex.Message}");
                    }
                }
                else
                {
                    callback(false, response);
                }
            }));
        }

        [Serializable]
        public class LoginResponse
        {
            public string access_token;
            public string refresh_token;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="username">New user's username</param>
        /// <param name="password">New user's password</param>
        /// <param name="institution_name">New user's institution  name</param>
        /// <param name="callback">Callback to handle the result</param>
        public void Register(string username, string password, string institution_name, Action<bool, string> callback)
        { 
            Dictionary<string, string> payload = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password },
                { "institution_name", institution_name }
            };

            StartCoroutine(PostRequest("register", payload, callback));
        }

        /// <summary>
        /// Post data to the webserver using bearer token authentication
        /// </summary>
        /// <param name="route">API route</param>
        /// <param name="data">Data to post</param>
        /// <param name="callback">Callback to handle the result</param>
        public void PostData(string route, Dictionary<string, string> data, Action<bool, string> callback)
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                callback(false, "Not authenticated. Please login first.");
                return;
            }

            StartCoroutine(PostRequest(route, data, callback, true));
        }

        /// <summary>
        /// Get data from the webserver using bearer token authentication if needed
        /// </summary>
        /// <param name="route">API route</param>
        /// <param name="useAuth">Whether to use authentication</param>
        /// <param name="callback">Callback to handle the result</param>
        public void GetData(string route, bool useAuth, Action<bool, string> callback)
        {
            if (useAuth && string.IsNullOrEmpty(_accessToken))
            {
                callback(false, "Not authenticated. Please login first.");
                return;
            }

            StartCoroutine(GetRequest(route, callback, useAuth));
        }

        /// <summary>
        /// Logout the current user
        /// </summary>
        public void Logout()
        {
            _accessToken = string.Empty;
            _refreshToken = string.Empty;
        }

        #region Private Helper Methods

        private IEnumerator GetRequest(string route, Action<bool, string> callback, bool useAuth = false)
        {
            string url = _baseUrl + route;

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                // Atașăm handler-ul pentru a ignora certificatele invalide
                request.certificateHandler = new AcceptAllCertificatesHandler();

                if (useAuth)
                {
                    request.SetRequestHeader("Authorization", "Bearer " + _accessToken);
                }

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    callback(true, request.downloadHandler.text);
                }
                else
                {
                    callback(false, $"Error: {request.error}");
                }
            }
        }

        private IEnumerator PostRequest(string route, Dictionary<string, string> data, Action<bool, string> callback, bool useAuth = false)
        {
            string url = _baseUrl + route;

            string jsonData = DictionaryToJson(data);

            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                if (useAuth)
                {
                    request.SetRequestHeader("Authorization", "Bearer " + _accessToken);
                }

                // Atașăm handler-ul pentru a ignora certificatele invalide
                request.certificateHandler = new AcceptAllCertificatesHandler();

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    callback(true, request.downloadHandler.text);
                }
                else
                {
                    callback(false, $"Error: {request.error}");
                }
            }
        }

        #endregion
    }


    [Serializable]
    public class Serialization<TKey, TValue>
    {
        [SerializeField]
        List<TKey> keys = new List<TKey>();

        [SerializeField]
        List<TValue> values = new List<TValue>();

        public Serialization(Dictionary<TKey, TValue> dict)
        {
            foreach (var kvp in dict)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }

        public Dictionary<TKey, TValue> ToDictionary()
        {
            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            for (int i = 0; i < Math.Min(keys.Count, values.Count); i++)
            {
                dict[keys[i]] = values[i];
            }
            return dict;
        }
    }
}
