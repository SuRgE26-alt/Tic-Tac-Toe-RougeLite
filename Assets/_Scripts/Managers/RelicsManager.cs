using System.Collections.Generic;
using UnityEngine;

public class RelicManager : MonoBehaviour
{
    public static RelicManager Instance { get; private set; }

    [SerializeField] private List<RelicSO> _catalog;
    [SerializeField] private List<RelicSO> _testOwned;      // testing only: relics owned from the start

    private readonly List<RelicSO> _owned = new List<RelicSO>();

    public IReadOnlyList<RelicSO> Catalog => _catalog;
    public IReadOnlyList<RelicSO> Owned => _owned;

    public event System.Action OnChanged;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        _owned.AddRange(_testOwned);
    }

    public bool Owns(RelicSO relic)
    {
        return _owned.Contains(relic);
    }

    public bool TryPurchase(RelicSO relic)
    {
        if (relic == null || Owns(relic)) return false;
        if (CoinManager.Instance == null || !CoinManager.Instance.TrySpend(relic._price)) return false;

        _owned.Add(relic);
        OnChanged?.Invoke();
        return true;
    }

    public void OnEncounterStart(PlayerHealth player)
    {
        Fire(RelicTrigger.OnEncounterStart, true, player);
    }

    public void OnBoardStart(bool playerMovesFirst, PlayerHealth player)
    {
        Fire(RelicTrigger.OnBoardStart, playerMovesFirst, player);
    }

    void Fire(RelicTrigger trigger, bool playerMovesFirst, PlayerHealth player)
    {
        foreach (RelicSO relic in _owned)
        {
            if (relic._trigger != trigger) continue;
            if (relic._condition == RelicCondition.PlayerMovesFirst && !playerMovesFirst) continue;

            Apply(relic, player);
        }
    }

    void Apply(RelicSO relic, PlayerHealth player)
    {
        switch (relic._effect)
        {
            case RelicEffect.GrantDefense:
                player.AddDefense(relic._amount);
                break;
            case RelicEffect.HealPlayer:
                player.Heal(relic._amount);
                break;
        }

        Debug.Log($"Relic triggered: {relic._displayName}");
    }
}