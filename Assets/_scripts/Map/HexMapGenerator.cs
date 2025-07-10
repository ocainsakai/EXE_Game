using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class HexMapGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    public GameObject hexTilePrefab; // Assign the Hextile prefab in Inspector
    public int columns = 5; // Total number of columns (including the first column)
    public float hexSize = 1f; // Size of each hexagon
    
    [Header("Hexagon Properties")]
    public float hexWidth = 3f;    // (300 / PPU)
    public float hexHeight = 2.598f; // (300 / PPU) * 0.866
    
    [Header("Grid Generation")]
    public bool generateOnStart = true;
    public Transform gridParent; // Parent object to hold all hex tiles
    
    private List<GameObject> hexTiles = new List<GameObject>();
    
    void Start()
    {
        if (generateOnStart)
        {
            GenerateHexGrid();
        }
    }
    
    public void GenerateHexGrid()
    {
        // Clear existing grid
        ClearGrid();
        
        // Create parent if not assigned
        if (gridParent == null)
        {
            GameObject parent = new GameObject("HexGrid");
            gridParent = parent.transform;
        }
        
        // First column: only one hex at row 1 (middle)
        CreateHexTile(1, 0);
        // Other columns: three hexes at rows 0, 1, 2
        for (int col = 1; col < columns; col++)
        {
            for (int row = 0; row < 3; row++)
            {
                CreateHexTile(row, col);
            }
        }
    }
    
    private void CreateHexTile(int row, int col)
    {
        if (hexTilePrefab == null)
        {
            Debug.LogError("Hex tile prefab not assigned!");
            return;
        }
        
        // Calculate position for point-top hexagon
        Vector3 position = CalculateHexPosition(row, col);
        
        // Instantiate hex tile
        GameObject hexTile = Instantiate(hexTilePrefab, position, Quaternion.identity, gridParent);
        hexTile.name = $"Hex_{row}_{col}";
        
        // Store reference
        hexTiles.Add(hexTile);
        
        // Optional: Add tile data component for future use
        HexTileData tileData = hexTile.GetComponent<HexTileData>();
        if (tileData == null)
        {
            tileData = hexTile.AddComponent<HexTileData>();
        }
        tileData.Initialize(row, col, position);
    }
    
    private Vector3 CalculateHexPosition(int row, int col)
    {
        // For point-top hexagons:
        // Even rows are offset by half a hex width
        float xOffset = col * hexWidth;
        if (row % 2 == 1) // Odd rows
        {
            xOffset += hexWidth * 0.5f;
        }
        
        float yOffset = row * hexHeight * 0.75f; // 0.75 = 3/4 of hex height for proper stacking
        
        return new Vector3(xOffset, yOffset, 0);
    }
    
    public void ClearGrid()
    {
        foreach (GameObject tile in hexTiles)
        {
            if (tile != null)
            {
                DestroyImmediate(tile);
            }
        }
        hexTiles.Clear();
    }
    
    // Get hex tile at specific grid coordinates
    public GameObject GetHexTile(int row, int col)
    {
        int index = row * columns + col;
        if (index >= 0 && index < hexTiles.Count)
        {
            return hexTiles[index];
        }
        return null;
    }
    
    // Get all hex tiles
    public List<GameObject> GetAllHexTiles()
    {
        return new List<GameObject>(hexTiles);
    }
    
    // Convert world position to grid coordinates
    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        // This is a simplified conversion - you might need to adjust based on your specific needs
        int col = Mathf.RoundToInt(worldPosition.x / hexWidth);
        int row = Mathf.RoundToInt(worldPosition.y / (hexHeight * 0.75f));
        
        return new Vector2Int(col, row);
    }
    
    // Convert grid coordinates to world position
    public Vector3 GridToWorld(int row, int col)
    {
        return CalculateHexPosition(row, col);
    }
}

// Helper class to store tile data
public class HexTileData : MonoBehaviour
{
    public int row;
    public int col;
    public Vector3 worldPosition;
    public bool isOccupied = false;
    
    public void Initialize(int r, int c, Vector3 pos)
    {
        row = r;
        col = c;
        worldPosition = pos;
    }
}
