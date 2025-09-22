using UnityEngine;

public static class HexDirectionExtensions
{
    private static readonly Vector2Int[] evenRowOffsets =
    {
        new Vector2Int(0, -1), // NE
        new Vector2Int(1, 0),  // E
        new Vector2Int(0, 1),  // SE
        new Vector2Int(-1, 1), // SW
        new Vector2Int(-1, 0), // W
        new Vector2Int(-1, -1) // NW
    };

    private static readonly Vector2Int[] oddRowOffsets =
    {
        new Vector2Int(1, -1), // NE
        new Vector2Int(1, 0),  // E
        new Vector2Int(1, 1),  // SE
        new Vector2Int(0, 1),  // SW
        new Vector2Int(-1, 0), // W
        new Vector2Int(0, -1)  // NW
    };

    public static Vector2Int GetOffset(this HexDirection dir, int row)
    {
        return (row % 2 == 0)
            ? evenRowOffsets[(int)dir]
            : oddRowOffsets[(int)dir];
    }
}
