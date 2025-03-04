using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

            string paramsurl = _browserSearchBarText.text.Split('?')[1];
            if (paramsurl.Length > 0) {
                //?name=value
                string paramName = paramsurl.Split("=")[0];
                string paramValue = paramsurl.Split("=")[1];
                if (paramName == _paramName && paramValue.Length > 0) {
                    Filter(paramValue);
                }
            }
        }
        private void OnDisable() {

        }

        public void ApplySearch() {
            //update main url
            string baseUrl = _browserSearchBarText.text.Split('?')[0];
            _browserSearchBarText.text = baseUrl + "?" + _paramName + "=" +  _internalSearchInput.text;
            //update the page content
            Filter(_internalSearchInput.text);
        }

        private void Filter(string name) {

            foreach (var obj in _purchasableObjects) {
                PurchasableItem item = obj.GetComponent<PurchasableItem>();
                if (item.TitleText.Contains(name)) {
                    item.gameObject.SetActive(true);
                }
                else {
                    item.gameObject.SetActive(false);
                }
            }
        }
    }
}
