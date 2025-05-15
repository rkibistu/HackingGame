using System.Globalization;
using System.Text.RegularExpressions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScenarioSQL {

    public class PurchasableItem : MonoBehaviour {
        [SerializeField]
        private TextMeshProUGUI _titleText;
        [SerializeField]
        private TextMeshProUGUI _priceText;
        [SerializeField]
        private Button _buyButton;

        public Action<PurchasableItem> OnBuyClicked;

        public string TitleText {
            get { return _titleText.text; }
            set { _titleText.text = value; }
        }
        public string Price {
            get { return _priceText.text; }
            set { _priceText.text = value; }
        }

        public float GetPriceValue() {
            return CleanAndParseFloat(_priceText.text);
        }

        public void OnButtonBuy() {
            OnBuyClicked?.Invoke(this);
        }

        private static float CleanAndParseFloat(string input) {
            // Remove non-numeric characters except dot and minus sign
            string cleaned = Regex.Replace(input, @"[^0-9\.]", "");

            // Try parsing to float
            if (float.TryParse(cleaned, NumberStyles.Float, CultureInfo.InvariantCulture, out float result)) {
                return Math.Abs(result); // ensure positive
            }

            throw new FormatException($"Invalid input: {input}");
        }
    }

}