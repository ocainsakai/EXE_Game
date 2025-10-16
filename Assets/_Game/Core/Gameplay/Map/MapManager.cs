using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Events;
public class MapManager : MonoBehaviour
{
    [Header("Map Settings")]
    public int mapWidth = 5;
    public int mapHeight = 5;

    [Header("Rendering")]
    public Transform containter;
    public UITileEntry tilePrefab;
    public Sprite playerIcon;
    public Color walkableColor = Color.white;
    public Color unwalkableColor = Color.black;

    public UnityEvent<Tile> OnTileSelected;

    private GridMap map;
    private UITileEntry[,] tileObjects;
    private Vector2Int playerPosition;

    private Tile _currentTile;
    void Start()
    {
        GameInstance.Singleton.SetRandomCurrentMap();
        var currentMap = GameInstance.Singleton?.currentMap;
        if (currentMap == null)
        {
            Debug.LogError("No current map set in GameInstance");
            return;
        }

        // OPTIMIZED: Đăng ký event chỉ một lần duy nhất.
        UITileEntry.OnTileMapClicked += OnTileMapClickHandler;

        CreateAndPopulateMap();
        RenderMapFirstTime();

        UpdatePlayerTile(new Vector2Int(0, 0), true); // isFirstTime = true
    }

    // OPTIMIZED: Gộp các hàm khởi tạo map vào một chỗ cho gọn.
    void CreateAndPopulateMap()
    {
        map = new GridMap(mapWidth, mapHeight);
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                var tile = map.GetTile(x, y);

                // Gán loại Tile
                if (x == mapWidth - 1 && y == mapHeight - 1)
                {
                    tile.Type = TileType.Boss;
                }
                else
                {
                    tile.Type = TileType.Enemy;
                }

                // Gán Occupant tương ứng
                CreateOccupantForTile(tile);
            }
        }
    }

    void CreateOccupantForTile(Tile tile)
    {
        switch (tile.Type)
        {
            case TileType.Enemy:
                var enemy = GameInstance.Singleton?.GetRandomEnemy();
                if (enemy != null)
                {
                    tile.OccupantID = enemy.EnemyID;
                    tile.Icon = enemy.Icon;
                }
                break;

            case TileType.Boss:
                var boss = GameInstance.Singleton?.currentMap?.bossData?.FirstOrDefault();
                if (boss != null)
                {
                    tile.OccupantID = boss.EnemyID;
                    tile.Icon = boss.Icon;
                }
                break;
        }
    }

    // OPTIMIZED: Đổi tên để rõ ràng đây là lần render đầu tiên, tạo ra các GameObjects.
    void RenderMapFirstTime()
    {
        if (tilePrefab == null || containter == null)
        {
            Debug.LogError("TilePrefab or Container is not assigned!");
            return;
        }

        // OPTIMIZED: Sử dụng Destroy thay vì DestroyImmediate.
        foreach (Transform child in containter)
        {
            Destroy(child.gameObject);
        }

        tileObjects = new UITileEntry[mapWidth, mapHeight];
        float tileSize = 150f;
        float offsetX = -(mapWidth * tileSize) / 2f + tileSize / 2f;
        float offsetY = -(mapHeight * tileSize) / 2f + tileSize / 2f;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                UITileEntry tileObj = Instantiate(tilePrefab, containter);
                tileObj.name = $"Tile_{x}_{y}";
                tileObj.transform.localPosition = new Vector3(x * tileSize + offsetX, y * tileSize + offsetY, 0);
                tileObj.transform.localScale = Vector3.one;

                var rect = tileObj.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.sizeDelta = new Vector2(tileSize, tileSize);
                }

                tileObjects[x, y] = tileObj;
                UpdateTileVisuals(x, y); // Cập nhật hình ảnh ban đầu
            }
        }
    }

    // OPTIMIZED: Hàm mới chỉ để cập nhật hình ảnh của một ô cụ thể.
    void UpdateTileVisuals(int x, int y)
    {
        if (x < 0 || x >= mapWidth || y < 0 || y >= mapHeight) return;

        Tile tileData = map.GetTile(x, y);
        UITileEntry tileUI = tileObjects[x, y];

        if (tileData == null || tileUI == null) return;

        Color tileColor = tileData.IsWalkable ? walkableColor : unwalkableColor;
        tileUI.SetData(new Vector2Int(x, y), tileData.Icon, tileColor);
    }

    // OPTIMIZED: Sửa lại để không render toàn bộ map.
    void UpdatePlayerTile(Vector2Int newPosition, bool isFirstTime = false)
    {
        // 1. Dọn dẹp vị trí cũ của người chơi (nếu không phải lần đầu)
        if (!isFirstTime)
        {
            ClearTile(playerPosition);
        }

        // 2. Reset trạng thái walkable của cả bản đồ trong data
        map.ResetWalkable();

        // 3. Cập nhật vị trí mới và dữ liệu cho ô người chơi
        playerPosition = newPosition;
        var playerTile = map.GetTile(playerPosition.x, playerPosition.y);
        playerTile.Icon = playerIcon;
        playerTile.Type = TileType.Player;
        playerTile.IsWalkable = false; // Người chơi không thể đi vào ô của chính mình

        // 4. Set các ô xung quanh là walkable (trong data)
        var walkableTiles = map.GetNeighbors(playerTile);
        foreach (var tile in walkableTiles)
        {
            if (tile.Type == TileType.Enemy || tile.Type == TileType.Boss)
            {
                tile.IsWalkable = true;
            }
        }

        // 5. Cập nhật lại hình ảnh cho TẤT CẢ các ô (chỉ cập nhật data, không tạo lại object)
        // Đây là cách đơn giản nhất. Cách tối ưu hơn nữa là chỉ cập nhật những ô bị thay đổi.
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                UpdateTileVisuals(x, y);
            }
        }
    }

    void ClearTile(Vector2Int position)
    {
        var tile = map.GetTile(position.x, position.y);
        tile.Icon = null;
        tile.IsWalkable = false;
        tile.Type = TileType.Nothing;

        // Cập nhật hình ảnh của ô vừa bị xóa
        UpdateTileVisuals(position.x, position.y);
    }

    void OnTileMapClickHandler(Vector2Int position)
    {
        var tile = map.GetTile(position);
        if (tile.Type == TileType.Player || tile.Type == TileType.Nothing) return;
        _currentTile = tile;
        OnTileSelected?.Invoke(tile);
    }
    public void OnEnterBattle()
    {
        UpdatePlayerTile(_currentTile.Position);
    }

    public void OnBattleWin()
    {
        UpdatePlayerTile(_currentTile.Position);
    }

    private void OnDestroy()
    {
        UITileEntry.OnTileMapClicked -= OnTileMapClickHandler;
    }
}