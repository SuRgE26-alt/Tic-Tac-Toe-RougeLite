using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public enum TileType
{
    Damage,
    Shield,
    Heal,
    None
}


[CreateAssetMenu(fileName = "TilesSO", menuName = "ScriptableObjects/GridTile/Tile", order = 1)]
public class TilesSO : ScriptableObject
{
     public TileType _tileType;
     public Sprite _tileSprite;
    [Tooltip("Keep this 0 if the tile has no damage")]
     public float _attackTileDamage;
}
