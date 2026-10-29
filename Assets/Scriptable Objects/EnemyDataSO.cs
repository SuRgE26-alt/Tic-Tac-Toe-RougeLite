using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyDataSO", order = 1)]
public class EnemyDataSO : ScriptableObject
{
    [Header("Difficulty Settings")]
    [SerializeField, Range(1, 9)] public int _lookaheadDepth = 2;
    [SerializeField, Range(0, 1)] public float _mistackeChance = 0.3f;

    [Header("Enemy Powers")]
    [SerializeField] private bool _hasPowers;

}
