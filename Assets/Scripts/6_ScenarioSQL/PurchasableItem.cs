using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
}
