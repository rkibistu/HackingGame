using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

namespace ScenarioSQL {
    public class AppStoreWebPageController : MonoBehaviour {

        [SerializeField]
        private List<GameObject> _appObjects;
        [SerializeField]
        private TMP_InputField _internalSearchInput;
        [SerializeField]
        private TMP_InputField _browserSearchBarText;

        [SerializeField]
        private string _paramName = "name";

        //Enabled and Disabled are called every time the Browser search bar is used
        // so we can use this methods to filter the params
        private void OnEnable() {

            _internalSearchInput.text = "";

            if (!_browserSearchBarText.text.Contains("?") || !_browserSearchBarText.text.Contains("=")) {
                Filter(_internalSearchInput.text);
                return;
            }

            string paramsurl = _browserSearchBarText.text.Split('?')[1];
            if (paramsurl.Length > 0) {
                //?name=value
                string paramName = paramsurl.Split("=")[0];
                string paramValue = paramsurl.Split("=")[1];
                if (paramName == _paramName) {
                    Filter(paramValue);
                }
            }

        }
        private void OnDisable() {

        }

        public void ApplySearch() {
            //update main url

            if(_browserSearchBarText.text.Contains("?")) {
                string baseUrl = _browserSearchBarText.text.Split('?')[0];
                _browserSearchBarText.text = baseUrl + "?" + _paramName + "=" + _internalSearchInput.text;
            }
            else {
                _browserSearchBarText.text = _browserSearchBarText.text + "?" + _paramName + "=" + _internalSearchInput.text;
            }

            //update the page content
            Filter(_internalSearchInput.text);
        }
        private void Filter(string input) {

            
        }

       
    }
}
