using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RelicCard : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _buyLabel;

    public void Init(RelicSO relic, System.Action onBuy)
    {
        _icon.sprite = relic._icon;
        _nameText.text = relic._displayName;
        _descriptionText.text = relic._description;
        _priceText.text = relic._price.ToString();

        _buyButton.onClick.RemoveAllListeners();
        _buyButton.onClick.AddListener(() => onBuy());
    }

    public void UpdateState(bool owned, bool canAfford)
    {
        _buyLabel.text = owned ? "Owned" : "Buy";
        _buyButton.interactable = !owned && canAfford;
    }
}