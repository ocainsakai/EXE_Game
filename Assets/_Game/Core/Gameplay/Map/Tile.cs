using UnityEngine;

// 1. Tile đơn giản - chỉ có position và có thể đi được không
public class Tile
{
    public Vector2Int Position;
    public bool IsWalkable = true;
    public TileType Type = TileType.Nothing;
    public object Occupant = null; 
    public Tile(int x, int y)
    {
        Position = new Vector2Int(x, y);
    }
}

public enum TileType
{
    Nothing,
    Player,
    Enemy,
    Boss,
}
