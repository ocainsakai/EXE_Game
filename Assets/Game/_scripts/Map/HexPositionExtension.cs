using System.Linq;
using UnityEngine;

public static class  HexPositionExtension
{
    public static bool HasRight(this Vector2Int pos, Vector2Int orther)
    {
        return pos.GetRight().Contains(orther);
    }
    public static Vector2Int[] GetLeft(this Vector2Int pos)
    {
        var leftTiles = new Vector2Int[2];
        if (pos.y % 2 == 0)
        {
            leftTiles[0] = new Vector2Int(pos.x, pos.y - 1);
            leftTiles[1] = new Vector2Int(pos.x - 1, pos.y - 1);
        }
        else
        {
            leftTiles[0] = new Vector2Int(pos.x, pos.y - 1);
            leftTiles[1] = new Vector2Int(pos.x + 1, pos.y - 1);
        }
        return leftTiles;
    }
    public static Vector2Int[] GetRight(this Vector2Int pos)
    {
        var rightTiles = new Vector2Int[2];
        if (pos.y % 2 == 0)
        {
            rightTiles[0] = new Vector2Int(pos.x, pos.y+1);
            rightTiles[1] = new Vector2Int(pos.x-1, pos.y+1);
        }
        else
        {
            rightTiles[0] = new Vector2Int(pos.x, pos.y + 1);
            rightTiles[1] = new Vector2Int(pos.x + 1, pos.y + 1);
        }
        return rightTiles;
    }
}