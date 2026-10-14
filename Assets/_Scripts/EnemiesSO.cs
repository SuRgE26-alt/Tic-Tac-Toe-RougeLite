using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemiesSO : ScriptableObject
{
    public int _enemyHealth;
    public int _enemyDamage;

    public int[] _difficltyLevel = { 1, 2, 3 };
}
