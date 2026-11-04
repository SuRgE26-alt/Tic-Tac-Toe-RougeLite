using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [ContextMenu("Test: Add 10 Defense")]
    void TestAddDefense() { AddDefense(10); }

    [SerializeField] private float _playerHealth = 100f;
    [SerializeField] private float _defense;
    private float _currentPlayerHealth;
    public float Defense => _defense;

    GameController _gameController;

    void Start()
    {
        _currentPlayerHealth = _playerHealth;
    }

    void Update()
    {
        
    }

    public void AddDefense(int amount)
    {
        if (amount <= 0) return;
        _defense += amount;
        Debug.Log($"Defense +{amount}, now {_defense}");
    }

    public void ResetDefense()
    {
        _defense = 0;
    }

    public void TakeDamage(float damage)
    {
        float absorbed = Mathf.Min(_defense, damage);
        _defense -= absorbed;
        damage -= absorbed;

        if (damage <= 0) return;

        _currentPlayerHealth -= damage;
        if (_currentPlayerHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        _gameController.SetResultText("Enemy Wins!");
    }

    public void SetGameControllerRef(GameController controller)
    {
        _gameController = controller;
    }
}
