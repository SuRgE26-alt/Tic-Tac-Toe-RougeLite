using UnityEngine;

[CreateAssetMenu(fileName = "EnemyPower", menuName = "ScriptableObjects/EnemyPowers", order = 1)]
public class EnemyPowerSO : ScriptableObject
{
    [SerializeField] public BlockTarget _target = BlockTarget.RandomCells;

    [SerializeField] public BlockMode _mode = BlockMode.Lock;

    [SerializeField] public TileType _tileType = TileType.Damage;

    [SerializeField] public int _cellCount = 1;

    [SerializeField, Min(1)] public int _duration = 2;

    private void OnValidate()
    {
        if (_target == BlockTarget.TileType && _tileType == TileType.None)
        {
            Debug.LogWarning($"{name}: Tile Type is None, so this power-up would cover every plain tile.", this);
        }
    }

}
