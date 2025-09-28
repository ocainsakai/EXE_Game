using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Events;
// 4. MonoBehaviour để sử dụng trong Unity
public class MapManager : MonoBehaviour
{
    [Header("Map Settings")]
    public int mapWidth = 10;
    public int mapHeight = 10;

    [Header("Rendering")]
    public Transform containter;
    public UITileEntry tilePrefab;
    public Sprite playerIcon;
    public Color walkableColor = Color.white;
    public Color unwalkableColor = Color.red;

    public UnityEvent<Tile> OnTileSelected;


    private GridMap map;
    private UITileEntry[,] tileObjects;


    private Vector2Int playerPosition;
    void Start()
    {
        CreateMap();
        CreateTileType();
        CreateOccupants();
        UpdatePlayerTile(new Vector2Int(0, 0));
        RenderMap();
    }

    private void CreateOccupants()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                var tile = map.GetTile(x, y);
                CreateOccupant(tile);
            }
        }
    }

    void CreateMap()
    {
        map = new GridMap(mapWidth, mapHeight);
    }
    void CreateTileType()
    {
        
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                var tile = map.GetTile(x, y);
                tile.Type = TileType.Enemy;
            }
        }
        map.GetTile(mapWidth-1, mapHeight-1).Type = TileType.Boss;
    }
    void CreateOccupant(Tile tile)
    {
        if (tile.Type == TileType.Enemy)
        {
            var enemy = GameInstance.Singleton?.GetRandomEnemy();
            if (enemy == null)
            {
                Debug.LogError("No enemy found in GameInstance!");
                return;
            }
            tile.OccupantID = enemy.EnemyID;
            tile.Icon = enemy.Icon;
        }
        else if (tile.Type == TileType.Boss)
        {
            var enemy = GameInstance.Singleton?.bossDatas?.FirstOrDefault();
            if (enemy == null)
            {
                Debug.LogError("No boss data found in GameInstance!");
                return;
            }
            tile.OccupantID = enemy.BossID;
            tile.Icon = enemy.Icon;
        }
    }

    public void RenderMap()
    {
        if (map == null)
        {
            Debug.LogError("Map is null, cannot render!");
            return;
        }

        if (tilePrefab == null)
        {
            Debug.LogError("TilePrefab is not assigned!");
            return;
        }

        if (containter == null)
        {
            Debug.LogError("Container is not assigned!");
            return;
        }
        foreach(Transform child in containter)
        {
            DestroyImmediate(child.gameObject);
        }
        tileObjects = new UITileEntry[mapWidth, mapHeight];
        UITileEntry.OnTileMapClicked += OnTileMapClickHandler;
        float tileSize = 100f;

        // offset để căn giữa map
        float offsetX = -(mapWidth * tileSize) / 2f + tileSize / 2f;
        float offsetY = -(mapHeight * tileSize) / 2f + tileSize / 2f;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Tile tile = map.GetTile(x, y);
                if (tile == null)
                {
                    Debug.LogWarning($"Tile at ({x},{y}) is null, skipping.");
                    continue;
                }

                Vector3 worldPos = new Vector3(x * tileSize + offsetX, y * tileSize + offsetY, 0);

                UITileEntry tileObj = Instantiate(tilePrefab, containter);
                if (tileObj == null)
                {
                    Debug.LogError($"Failed to instantiate Tile prefab at ({x},{y})");
                    continue;
                }

                tileObj.name = $"Tile_{x}_{y}";
                tileObj.transform.localPosition = worldPos;
                tileObj.transform.localScale = Vector3.one;

                var rect = tileObj.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.sizeDelta = new Vector2(tileSize, tileSize);
                }
                else
                {
                    Debug.LogWarning($"Tile prefab at ({x},{y}) has no RectTransform!");
                }

                // Nếu icon null thì log cảnh báo nhưng vẫn cho chạy
                if (tile.Icon == null)
                {
                    Debug.LogWarning($"Tile at ({x},{y}) has no Icon assigned!");
                }

                tileObj.SetData(
                    new Vector2Int(x, y),
                    tile.Icon,
                    tile.IsWalkable ? walkableColor : unwalkableColor
                );

                tileObjects[x, y] = tileObj;
            }
        }
    }

    void UpdatePlayerTile(Vector2Int position)
    {
        map.ResetWalkable();
        playerPosition = position;
        var playerTile = map.GetTile(playerPosition.x, playerPosition.y);
        playerTile.Icon = playerIcon;
        playerTile.Type = TileType.Player;
        var walkableTiles = map.GetNeighbors(playerTile);
        foreach (var tileObj in walkableTiles)
        {
            if (tileObj.Type == TileType.Nothing) continue;
            tileObj.IsWalkable = true;
        }
        RenderMap();
    }
    void ClearTile(Vector2Int position)
    {
        var tile = map.GetTile(position.x, position.y);
        tile.Icon = null;
        tile.IsWalkable = false;
        tile.Type = TileType.Nothing;

    }
    void OnTileMapClickHandler(Vector2Int position)
    {
        Debug.Log($"Tile clicked at: {position}");

        var tile = map.GetTile(position);
        if (tile.Type == TileType.Player || tile.Type == TileType.Nothing) return;
        _currentTile = tile;
        OnTileSelected?.Invoke(tile);
    }


    private Tile _currentTile;
    public void OnBattleEnter()
    {

    }
    public void OnBattleWin()
    {
        ClearTile(playerPosition);
        UpdatePlayerTile(_currentTile.Position);
    }
    private void OnDestroy()
    {
        UITileEntry.OnTileMapClicked -= OnTileMapClickHandler;
    }
}