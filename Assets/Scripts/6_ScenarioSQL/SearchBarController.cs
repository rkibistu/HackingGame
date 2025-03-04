using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

namespace ScenarioSQL {
    public class SearchBarController : MonoBehaviour {

        [SerializeField]
        private List<GameObject> _purchasableObjects;
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

        private void FilterAndCheckSqlInjection(string input) {
            //case insensitive
            //remove double spaces

            // Normalize spaces (replace multiple spaces with a single space)
            string normalizedInput = Regex.Replace(input, @"\s+", " ").Trim();

            string filterQuery = input.Split(";")[0]; // 6090%'
            Debug.Log("q: " + filterQuery);
            //apply search
            string filterValue = filterQuery.Split("%")[0];
            Filter(filterValue);


            string updateQuery = input.Split(";")[1]; //  update products set price=49.99 where name like ' % 6090 % '
            string commentQuery = input.Split(";")[2]; //  #

            Debug.Log(filterQuery);
            Debug.Log(updateQuery);
            Debug.Log(commentQuery);


            //6090%'; update products set price=49.99 where name like ' %6090% '; #
            //6090 '; update products set price=49.99 where name like ' %6090% '; #
            //6090'; update products set price=49.99 where name like ' %6090% '; #
        }
        private void Filter(string input) {

            //6090
            //6090faf
            //6090;
            string normalizedInput = Regex.Replace(input, @"\s+", " ").Trim();
            string filterQuery = normalizedInput.Split(";")[0]; // 6090%'

            foreach (var obj in _purchasableObjects) {
                PurchasableItem item = obj.GetComponent<PurchasableItem>();
                if (Regex.IsMatch(item.TitleText, Regex.Escape(filterQuery), RegexOptions.IgnoreCase)) {
                    item.gameObject.SetActive(true);
                }
                else {
                    item.gameObject.SetActive(false);
                }
            }
        }

        

    }
}
