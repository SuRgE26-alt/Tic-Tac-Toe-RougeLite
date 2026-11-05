using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [ContextMenu("Test: Set health to 1")]
    void TestSetLow() { TakeDamage(CurrentHealth - 1); }

    [SerializeField] private float _maxEnemyHealth = 100;
    [SerializeField] private float _defense;
    [SerializeField] private Image _enemyHealthbarImage;
    private float _currentEnemyHealth;
    public float Defense => _defense;
    public float CurrentHealth => _currentEnemyHealth;

    void Start()
    {
        _currentEnemyHealth = _maxEnemyHealth;
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
        _currentEnemyHealth = _maxEnemyHealth;
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

    public void Heal(float amount)
    {
        if (amount <= 0) return;
        _currentEnemyHealth = Mathf.Min(_currentEnemyHealth + amount, _maxEnemyHealth);
        Debug.Log($"Healed {amount}, now {_currentEnemyHealth}/{_maxEnemyHealth}");
    }

    void HealthBarUpdate()
    {
        float target = (float)_currentEnemyHealth / _maxEnemyHealth;
        _enemyHealthbarImage.fillAmount = Mathf.Lerp(_enemyHealthbarImage.fillAmount, target, 5f * Time.deltaTime);
    }

    void Die()
    {
        Debug.Log("Enemy has died.");
        //SceneManager.LoadScene("Map");
    }
}
