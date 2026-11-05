using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyDataSO", order = 1)]
public class EnemyDataSO : ScriptableObject
{
    [Header("Difficulty Settings")]
    [SerializeField, Range(1, 9)] public int _lookaheadDepth = 2;
    [SerializeField, Range(0, 1)] public float _mistakeChance = 0.3f;

    [Header("Enemy Powers")]
    [SerializeField] public bool _hasPowers;
    [SerializeField] public EnemyPowerSO[] _powerUps;

    [Header("Rewards")]
    [SerializeField, Min(0)] public int _coinReward = 10;
    [SerializeField, Min(0)] public int _healReward = 15;

}
