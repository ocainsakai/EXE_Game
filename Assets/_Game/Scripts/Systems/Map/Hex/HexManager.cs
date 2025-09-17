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
            var tile = Instantiate(hexPrefab, worldPos, Quaternion.identity, transform);
            tile.Initialize(pos, this);
            hexes[pos] = tile;
        }
    }

    public Hex GetHexAt(Vector2Int pos)
    {
        return hexes.TryGetValue(pos, out var hex) ? hex : null;
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