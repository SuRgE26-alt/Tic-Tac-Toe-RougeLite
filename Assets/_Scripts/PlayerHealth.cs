using Map;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _maxPlayerHealth = 100f;
    [SerializeField] private float _defense;
    [SerializeField] private Image _playerHealthbarImage;
    private float _currentPlayerHealth;
    public float Defense => _defense;
    public float CurrentHealth => _currentPlayerHealth;

    GameController _gameController;

    void Start()
    {
        _currentPlayerHealth = PlayerProgress.HasHealth ? PlayerProgress.Health : _maxPlayerHealth;
    }

    void Update()
    {
        HealthBarUpdate();
    }

    public void AddDefense(float amount)
    {
        if (amount <= 0) return;
        _defense += amount;
        Debug.Log($"Defense +{amount}, now {_defense}");
    }

    public void ResetDefense()
    {
        _defense = 0;
    }

    public void ResetHealth()
    {
        _currentPlayerHealth = _maxPlayerHealth;
        PlayerProgress.SaveHealth(_currentPlayerHealth);
    }

    public void TakeDamage(float damage)
    {
        float absorbed = Mathf.Min(_defense, damage);
        _defense -= absorbed;
        damage -= absorbed;

        if (damage <= 0) return;

        _currentPlayerHealth -= damage;

        PlayerProgress.SaveHealth(_currentPlayerHealth);

        if (_currentPlayerHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (amount <= 0) return;
        _currentPlayerHealth = Mathf.Min(_currentPlayerHealth + amount, _maxPlayerHealth);
        PlayerProgress.SaveHealth(_currentPlayerHealth);
        Debug.Log($"Healed {amount}, now {_currentPlayerHealth}/{_maxPlayerHealth}");
    }

    private void Die()
    {
        Debug.Log("Player has died.");
    }

    void HealthBarUpdate()
    {
        float target = (float)_currentPlayerHealth / _maxPlayerHealth;
        _playerHealthbarImage.fillAmount = Mathf.Lerp(_playerHealthbarImage.fillAmount, target, 5f * Time.deltaTime);
    }

    public void SetGameControllerRef(GameController controller)
    {
        _gameController = controller;
    }
}
