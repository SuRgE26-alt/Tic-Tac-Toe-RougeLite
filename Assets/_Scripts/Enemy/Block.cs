using System.Collections.Generic;

public enum BlockTarget { RandomCells, TileType }
public enum BlockMode { Lock, Nullify }

public class Block
{
    public BlockTarget _target;
    public BlockMode _mode;
    public TileType _tileType;      // used when the target is TileType
    public List<int> _cells;        // used when the target is RandomCells (cell numbers 0 to 8)
    public int _movesLeft;          // the player's moves remaining
}