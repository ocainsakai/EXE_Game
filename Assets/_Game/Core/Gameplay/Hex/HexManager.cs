using System.Collections.Generic;
using UnityEngine;

public class HexManager : MonoBehaviour
{
    [SerializeField] private Hex hexPrefab;
    [SerializeField] private float hexSize = 1f;

    private readonly Dictionary<Vector2Int, Hex> hexes = new();
    public void GenerateGrid(IHexLayoutGenerator layout)
    {
        foreach (var pos in layout.GenerateLayout())
        {
            if (hexes.ContainsKey(pos)) continue; 
            var worldPos = CalcWorldPosition(pos);
            var tile = Instantiate(hexPrefab, transform);
            tile.transform.SetLocalPositionAndRotation(worldPos, Quaternion.identity);
            tile.Initialize(pos, this);
            hexes[pos] = tile;
        }
    }

    public Hex GetHexAt(Vector2Int pos)
    {
        return hexes.TryGetValue(pos, out var hex) ? hex : null;
    }
    public bool IsRightOfHex(Vector2Int from, Vector2Int to)
    {
        return to.y > from.y;
    }
    public bool IsNeighbor(Vector2Int a, Vector2Int b)
    {
        var directions = new Vector2Int[]
        {
            new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, 1),
            new Vector2Int(-1, 0), new Vector2Int(0, -1), new Vector2Int(1, -1)
        };
        foreach (var dir in directions)
        {
            if (a + dir == b)
                return true;
        }
        return false;
    }
    public bool IsNeighbor(Hex a, Hex b)
    {
        return IsNeighbor(a.HexPosition, b.HexPosition);
    }
    public bool IsNeighbor(Hex a, Vector2Int b)
    {
        return IsNeighbor(a.HexPosition, b);
    }
    public bool IsNeighborRightOf(Hex from, Hex to)
    {
        return IsRightOfHex(from.HexPosition, to.HexPosition) && IsNeighbor(from, to);
    }
    public bool IsNeighborRightOf(Hex from, Vector2Int to)
    {
        return IsRightOfHex(from.HexPosition, to) && IsNeighbor(from.HexPosition, to);
    }
    public bool IsNeighborRightOf(Vector2Int from, Vector2Int to)
    {
        return IsRightOfHex(from, to) && IsNeighbor(from, to);
    }
    public bool IsNeighborLeftOf(Hex from, Hex to)
    {
        return !IsRightOfHex(from.HexPosition, to.HexPosition) && IsNeighbor(from, to);
    }
    public bool IsNeighborLeftOf(Hex from, Vector2Int to)
    {
        return !IsRightOfHex(from.HexPosition, to) && IsNeighbor(from.HexPosition, to);
    }
    public IEnumerable<Hex> GetAllHexes()
    {
        return hexes.Values;
    }
    private Vector3 CalcWorldPosition(Vector2Int gridPos)
    {
        float hexWidth = Mathf.Sqrt(3f) * hexSize;
        float hexHeight = 2f * hexSize;

        float x = gridPos.x * hexWidth + ((gridPos.y % 2 == 0) ? 0 : hexWidth / 2f);
        float y = gridPos.y * (hexHeight * 0.75f);

        return new Vector3(y, x, 0);
    }

}