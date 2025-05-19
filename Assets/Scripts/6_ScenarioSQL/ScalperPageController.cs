using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

namespace ScenarioSQL {
    public class ScalperPageController : MonoBehaviour {

        [SerializeField]
        private List<GameObject> _purchasableObjects;
        [SerializeField]
        private TMP_InputField _internalSearchInput;
        [SerializeField]
        private TMP_InputField _browserSearchBarText;
        [SerializeField]
        private string _paramName = "name";

        [Header("Feedback - buy item")]
        [SerializeField]
        private GameObject _feedbackPanelBuyItem;
        [SerializeField]
        private TextMeshProUGUI _feedbackTextBuyItem;
        [SerializeField]
        private int _feedbackTextBuyItemLifetime = 3;

        [Header("Feedback - sql injection")]
        [SerializeField]
        private GameObject _feedbackPanelSqlInjection;
        [SerializeField]
        private TextMeshProUGUI _feedbackTextSqlInjection;

        [Header("Popups")]
        [Tooltip("Used to explain how to use sql injection on this simulated site")]
        [SerializeField]
        private GameObject _howToSqlPopup;
        [Tooltip("The id that has to be compelted before showing the howToSql popup")]
        [SerializeField]
        private string _taskIdToActivateHowToSql = "scan-sqlmap";


        //some state variables
        private bool _firstTimeOnSite = true;
        private bool _firstTimeSearch = true;
        private bool _firstTimeOnSiteAfterSqlmap = true;

        //Enabled and Disabled are called every time the Browser search bar is used
        // so we can use this methods to filter the params
        private void OnEnable() {

            if (_firstTimeOnSite) {
                _firstTimeOnSite = false;
                TasksController.Instance.Mark("check-desktop");
                TasksController.Instance.ActivateTask("try-search");

                DialogueController.Instance.SkipCurrentStoryCompletely();
                DialogueController.Instance.PlayStory("try-search");
            }
            EnablePopupSqlInjection();


            _internalSearchInput.text = "";

            if (!_browserSearchBarText.text.Contains("?") || !_browserSearchBarText.text.Contains("=")) {
                Filter(_internalSearchInput.text);
                return;
            }

            string paramsurl = _browserSearchBarText.text.Split('?')[1];
            if (paramsurl.Length > 0) {
                //?name=value
                string paramName = paramsurl.Split("=")[0];
                if (paramName == _paramName) {
                    TriggerStoryFirstTimeSearchByName();
                    Filter(paramsurl.Substring(paramsurl.IndexOf('=') + 1));
                }
            }

        }
        private void OnDisable() {

        }

        private void Start() {
            //Register callback for Buy button oif every item
            foreach (var obj in _purchasableObjects) {
                var item = obj.GetComponent<PurchasableItem>();
                if (item != null) {
                    item.OnBuyClicked += HandleBuyClicked;
                }
            }
        }

        public void ApplySearch() {
            //update main url
            TriggerStoryFirstTimeSearchByName();

            if (_browserSearchBarText.text.Contains("?")) {
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
            _feedbackPanelSqlInjection.SetActive(false);

            string normalizedInput = Regex.Replace(input, @"\s+", " ").Trim();

            // Get text until first ';' and use it to filter
            //TODO: should we mark it as wrong inejction if % or ' is missing ???
            string filterQuery = normalizedInput.Split(";")[0]; // 6090%'
            filterQuery = Regex.Replace(filterQuery, @"%", "");
            filterQuery = Regex.Replace(filterQuery, @"'", "");
            filterQuery.TrimEnd();
            filterQuery.TrimStart();

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
                Debug.Log("Injection incomplete");
                FeedbackSqlInjection("Injection incomplete");
                return;
            }  

            string updateQuery = injectionInput.Split(";")[0]; //  update products set price=49.99 where name like ' % 6090 % '
            string commentQuery = injectionInput.Split(";")[1]; //  #
            //TODO: invalidate ijection if commentQuery is missing??? or something??

            string missingWord;
            bool queryOkay = CheckQuery(updateQuery, out missingWord);
            if (queryOkay == false)
            {
                FeedbackSqlInjection("Missing/wrong word in query: " + missingWord);
                return;
            }

            string price, targetName;
            string pricePattern = @"\s+price\s*=\s*(\d+(\.\d+)?)";
            string targetNamePattern = @"'([^']*)'";

            Match priceMatch = Regex.Match(updateQuery, pricePattern, RegexOptions.IgnoreCase);
            Match targetNameatch = Regex.Match(updateQuery, targetNamePattern);

            if(priceMatch.Success == false) {
                FeedbackSqlInjection("Wrong price pattern!");
                return;
            }
            if(targetNameatch.Success == false) {
                FeedbackSqlInjection("Wrong target name pattern!");
                return;
            }

            if (priceMatch.Success && targetNameatch.Success) {
                price = priceMatch.Groups[1].Value;
                targetName = targetNameatch.Groups[1].Value;
                targetName = Regex.Replace(targetName, @"%", "");

                foreach (var obj in _purchasableObjects) {
                    PurchasableItem item = obj.GetComponent<PurchasableItem>();
                    if (Regex.IsMatch(item.TitleText, Regex.Unescape(targetName), RegexOptions.IgnoreCase)) {
                        item.Price = "price: " + price;
                        TasksController.Instance.Mark("use-sqlinjection");
                    }
                }
            }
  
            //6090%'; update products set price=49.99 where name like '%6090%'; #
            //6090 '; update products set price=49.99 where name like ' %6090% '; #
            //6090'; update products set price=49.99 where name like ' %6090% '; #
        }

        private bool CheckQuery(string query, out string missingWord) {
            missingWord = null;
            var words = new List<string>(query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

            List<string> wordsToFind = new List<string>() { 
                "update", "products", "set", "where", "name", "like"
            };
            int index = 0;
            string currentWordToCheck = words[0];

            foreach (var word in words) {
                currentWordToCheck = wordsToFind[index];
                if(word.ToLower() == currentWordToCheck) {
                    index++;
                }

                if(index >= wordsToFind.Count) {
                    return true;
                }
            }

            missingWord = currentWordToCheck;
            return false;

        }

        private void TriggerStoryFirstTimeSearchByName() {
            if (_firstTimeSearch) {
                DialogueController.Instance.SkipCurrentStoryCompletely();
                DialogueController.Instance.PlayStory("scan-sqlmap");
                TasksController.Instance.Mark("try-search");
                TasksController.Instance.ActivateTask("scan-sqlmap");
                _firstTimeSearch = false;
            }
        }

        // If it is first time accesing the page after usign sqlmap -> enable popup HowToSql
        private void EnablePopupSqlInjection() {
            bool taskCompleted = TasksController.Instance.CheckIfComplete(_taskIdToActivateHowToSql);
            if (taskCompleted == true && _firstTimeOnSiteAfterSqlmap == true) {
                _firstTimeOnSiteAfterSqlmap = false;
            } 
        }

        private void HandleBuyClicked(PurchasableItem item) {
            Debug.Log($"[ScalperPageController] Item bought: {item.TitleText} for {item.GetPriceValue()}");
            // Do anything else here, like updating UI, inventory, etc.

            float price = item.GetPriceValue();
            float balance = GameplayScenario6.Instance.PersonalBalance;
            if(balance >= price) {
                EnableAndSetFeedbackText("SUCCESS! You bought: " + item.TitleText + " at " + price + "$", Color.green);
                TasksController.Instance.Mark("buy-something", true, true);
            }
            else {
                EnableAndSetFeedbackText("You don't have enough money! Your balance is: " + balance + "$", Color.red);
            }
        }

        private void EnableAndSetFeedbackText(string text, Color color) {
            _feedbackTextBuyItem.text = text;
            _feedbackTextBuyItem.color = color;
            _feedbackPanelBuyItem.gameObject.SetActive(true);
            StartCoroutine(DisableObjectWithDelayCoroutine(_feedbackPanelBuyItem));
        }

        // Enable panel with feedback and set text accordingly
        private void FeedbackSqlInjection(string text) {
            _feedbackPanelSqlInjection.SetActive(true);
            _feedbackTextSqlInjection.text = text;
        }

        private IEnumerator DisableObjectWithDelayCoroutine(GameObject obj) {
            yield return new WaitForSeconds(_feedbackTextBuyItemLifetime);
            if (obj != null) {
                obj.SetActive(false);
            }
        }
    }
}
