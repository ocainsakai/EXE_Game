using UnityEngine;
using System.Collections.Generic;
// 2. Map don gi?n - ch? luu tiles và cung c?p access co b?n
public class GridMap
{
    private Tile[,] tiles;
    public int Width { get; private set; }
    public int Height { get; private set; }

    public GridMap(int width, int height)
    {
        Width = width;
        Height = height;
        tiles = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y] = new Tile(x, y);
            }
        }
    }

    public Tile GetTile(int x, int y)
    {
        if (IsInBounds(x, y))
            return tiles[x, y];
        return null;
    }

    public Tile GetTile(Vector2Int pos)
    {
        return GetTile(pos.x, pos.y);
    }

    public bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }
    public List<Tile> GetNeighbors(Tile tile)
    {
        List<Tile> neighbors = new List<Tile>();
        Vector2Int pos = tile.Position;

        Vector2Int[] directions = {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighborPos = pos + dir;
            Tile neighbor = GetTile(neighborPos);
            if (neighbor != null)
                neighbors.Add(neighbor);
        }

        return neighbors;
    }
}
