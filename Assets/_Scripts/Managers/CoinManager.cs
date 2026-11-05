using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    [SerializeField] private int _coins;

    public int Coins => _coins;
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
    }

    public void AddCoins(int amount)
    {
        _coins += amount;
        OnChanged?.Invoke();
    }

    public bool TrySpend(int amount)
    {
        if (amount > _coins) return false;

        _coins -= amount;
        OnChanged?.Invoke();
        return true;
    }
}