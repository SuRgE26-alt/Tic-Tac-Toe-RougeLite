using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float _enemyHealth = 100;
    [SerializeField] private float _defense;
    private float _currentEnemyHealth;
    public float Defense => _defense;

    void Start()
    {
        _currentEnemyHealth = _enemyHealth;
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

        _currentEnemyHealth -= damage;
        if (_currentEnemyHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy has died.");
        SceneManager.LoadScene("Map");
    }
}
