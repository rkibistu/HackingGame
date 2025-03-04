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

        public string TitleText {
            get { return _titleText.text; }
            set { _titleText.text = value; }
        }
        public string Price {
            get { return _priceText.text; }
            set { _priceText.text = value; }
        }
    }

}