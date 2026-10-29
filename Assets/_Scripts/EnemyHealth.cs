using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float _enemyHealth = 100;
    private float _currentEnemyHealth;

    void Start()
    {
        _currentEnemyHealth = _enemyHealth;
    }

    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
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
