using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _playerHealth = 100f;
    private float _currentPlayerHealth;

    GameController _gameController;

    void Start()
    {
        _currentPlayerHealth = _playerHealth;
    }

    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
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
