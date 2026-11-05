using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private RelicCard _cardPrefab;
    [SerializeField] private Transform _cardParent;
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private string _mapSceneName = "Map";

    private readonly List<RelicCard> _cards = new List<RelicCard>();

    void Start()
    {
        RelicManager relics = RelicManager.Instance;
        CoinManager coins = CoinManager.Instance;
        if (relics == null || coins == null)
        {
            Debug.LogError("ShopManager: RelicManager or CoinManager is missing.");
            return;
        }

        foreach (RelicSO relic in relics.Catalog)
        {
            RelicSO item = relic;
            RelicCard card = Instantiate(_cardPrefab, _cardParent);
            card.Init(item, () => relics.TryPurchase(item));
            _cards.Add(card);
        }

        relics.OnChanged += Refresh;
        coins.OnChanged += Refresh;
        Refresh();
    }

    void OnDestroy()
    {
        if (RelicManager.Instance != null) RelicManager.Instance.OnChanged -= Refresh;
        if (CoinManager.Instance != null) CoinManager.Instance.OnChanged -= Refresh;
    }

    void Refresh()
    {
        RelicManager relics = RelicManager.Instance;
        int coins = CoinManager.Instance.Coins;

        _coinsText.text = coins.ToString();

        for (int i = 0; i < _cards.Count; i++)
        {
            RelicSO relic = relics.Catalog[i];
            _cards[i].UpdateState(relics.Owns(relic), coins >= relic._price);
        }
    }

    public void ContinueToMap()
    {
        SceneManager.LoadScene(_mapSceneName);
    }
}