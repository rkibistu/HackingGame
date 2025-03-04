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
        private void Filter(string input) {

            string normalizedInput = Regex.Replace(input, @"\s+", " ").Trim();

            // Get text until first ';' and use it to filter
            //TODO: should we mark it as wrong inejction if % or ' is missing ???
            string filterQuery = normalizedInput.Split(";")[0]; // 6090%'
            filterQuery = Regex.Replace(filterQuery, @"%", "");
            filterQuery = Regex.Replace(filterQuery, @"'", "");

            foreach (var obj in _purchasableObjects) {
                PurchasableItem item = obj.GetComponent<PurchasableItem>();
                if (Regex.IsMatch(item.TitleText, Regex.Unescape(filterQuery), RegexOptions.IgnoreCase)) {
                    item.gameObject.SetActive(true);
                }
                else {
                    item.gameObject.SetActive(false);
                }
            }

            // parse and execute sql injection
            if(normalizedInput.Contains(";"))
                ParseAndExecuteInjection(normalizedInput.Substring(normalizedInput.IndexOf(";") + 1));
        }

        private void ParseAndExecuteInjection(string injectionInput) {

            if (!injectionInput.Contains(";")) {
                Debug.Log("Inejction incomplete");
                return;
            }

            string updateQuery = injectionInput.Split(";")[0]; //  update products set price=49.99 where name like ' % 6090 % '
            string commentQuery = injectionInput.Split(";")[1]; //  #
            //TODO: invalidate ijection if commentQuery is missing??? or something??

            string price, targetName;
            string pricePattern = @"\s+price\s*=\s*(\d+(\.\d+)?)";
            string targetNamePattern = @"'([^']*)'";

            Match priceMatch = Regex.Match(updateQuery, pricePattern, RegexOptions.IgnoreCase);
            Match targetNameatch = Regex.Match(updateQuery, targetNamePattern);

            if (priceMatch.Success && targetNameatch.Success) {
                price = priceMatch.Groups[1].Value;
                targetName = targetNameatch.Groups[1].Value;
                targetName = Regex.Replace(targetName, @"%", "");

                foreach (var obj in _purchasableObjects) {
                    PurchasableItem item = obj.GetComponent<PurchasableItem>();
                    if (Regex.IsMatch(item.TitleText, Regex.Unescape(targetName), RegexOptions.IgnoreCase)) {
                        item.Price = "price: " + price;
                    }
                }
            }
  
            //6090%'; update products set price=49.99 where name like '%6090%'; #


            //6090 '; update products set price=49.99 where name like ' %6090% '; #
            //6090'; update products set price=49.99 where name like ' %6090% '; #
        }




    }
}
