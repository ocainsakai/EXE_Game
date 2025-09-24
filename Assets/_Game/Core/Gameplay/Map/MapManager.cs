using UnityEngine;
using System.Collections.Generic;
// 4. MonoBehaviour để sử dụng trong Unity
public class MapManager : MonoBehaviour
{
    [Header("Map Settings")]
    public int mapWidth = 10;
    public int mapHeight = 10;

    [Header("Rendering")]
    public Transform containter;
    public UITileEntry tilePrefab;
    public Color walkableColor = Color.white;
    public Color unwalkableColor = Color.red;

    private GridMap map;
    private SimplePathfinder pathfinder;
    private UITileEntry[,] tileObjects;

    void OnEnable()
    {
        CreateMap();
        CreateTileType();
        RenderMap();
    }

    void CreateMap()
    {
        map = new GridMap(mapWidth, mapHeight);
        pathfinder = new SimplePathfinder();
    }
    void CreateTileType()
    {
        
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                map.GetTile(x, y).Type = TileType.Enemy;
            }
        }
        map.GetTile(0, 0).Type = TileType.Player;
        map.GetTile(mapWidth, mapHeight).Type = TileType.Boss;
    }
    void CreateOccupant(Tile tile)
    {
        if (tile.Type == TileType.Player)
        {
            // Tạo occupant cho player
            //var player = 
        }
        else if (tile.Type == TileType.Enemy)
        {
            // Tạo occupant cho enemy
        }
        else if (tile.Type == TileType.Boss)
        {
            // Tạo occupant cho boss
        }
    }
    void RenderMap()
    {
        tileObjects = new UITileEntry[mapWidth, mapHeight];

        float tileSize = 100f;

        // offset để căn giữa map
        float offsetX = -(mapWidth * tileSize) / 2f + tileSize / 2f;
        float offsetY = -(mapHeight * tileSize) / 2f + tileSize / 2f;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Tile tile = map.GetTile(x, y);
                Vector3 worldPos = new Vector3(x * tileSize + offsetX, y * tileSize + offsetY, 0);

                UITileEntry tileObj = Instantiate(tilePrefab, containter);

                tileObj.name = $"Tile_{x}_{y}";
                tileObj.transform.localPosition = worldPos;
                tileObj.transform.localScale = Vector3.one;

                var rect = tileObj.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(tileSize, tileSize);

                tileObj.SetData(null, unwalkableColor);
                tileObjects[x, y] = tileObj;
            }
        }
    }


    // Test pathfinding - gọi từ editor hoặc code khác
    public void TestPathfinding(Vector2Int startPos, Vector2Int goalPos)
    {
        Tile start = map.GetTile(startPos);
        Tile goal = map.GetTile(goalPos);

        if (start == null || goal == null)
        {
            Debug.Log("Invalid positions");
            return;
        }

        List<Tile> path = pathfinder.FindPath(map, start, goal);

        if (path != null)
        {
            Debug.Log($"Found path with {path.Count} steps");
            //HighlightPath(path);
        }
        else
        {
            Debug.Log("No path found");
        }
    }

    //void HighlightPath(List<Tile> path)
    //{
    //    // Reset màu cũ
    //    RenderMap();

    //    // Highlight path
    //    foreach (Tile tile in path)
    //    {
    //        GameObject tileObj = tileObjects[tile.Position.x, tile.Position.y];
    //        Renderer renderer = tileObj.GetComponent<Renderer>();
    //        if (renderer != null)
    //        {
    //            renderer.material.color = Color.green;
    //        }
    //    }
    //}
}