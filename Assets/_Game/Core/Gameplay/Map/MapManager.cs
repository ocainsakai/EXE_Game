using UnityEngine;
using System.Linq;
using System;
using System.Collections;
using _Game.Core;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

// --- Thêm hai lớp này để lưu trữ dữ liệu ---

[System.Serializable]
public class SavedTileData
{
    public TileType Type;
    public string OccupantID;
    // Chúng ta không cần lưu IsWalkable (vì nó được tính toán lại)
    // Chúng ta không thể lưu Icon (sẽ được tải lại từ OccupantID)
}

[System.Serializable]
public class SavedMapData
{
    public int mapWidth;
    public int mapHeight;
    public Vector2Int playerPosition;

    // JsonUtility không hỗ trợ mảng 2D, vì vậy chúng ta dùng mảng 1D
    // và tự tính toán chỉ số (index)
    public SavedTileData[] tiles;
}


public class MapManager : MonoBehaviour
{
    [Header("Map Settings")]
    public int mapWidth = 5;
    public int mapHeight = 5;

    [Header("Rendering")] 
    [SerializeField] private GameObject UIMap;
    public RectTransform containter;
    public UITileEntry tilePrefab;
    public Sprite playerIcon;
    public Color walkableColor = Color.white;
    public Color unwalkableColor = Color.black;

    public UnityEvent<Tile> onTileSelected;
    public UnityEvent<EnemyData> onTilePlay;

    private GridMap map;
    private UITileEntry[,] tileObjects;
    private Vector2Int playerPosition;

    private Tile _currentTile;

    // Khóa (key) để lưu/tải dữ liệu từ PlayerPrefs
    public const string SAVE_KEY = "SavedMapData";

    void Awake()
    {
        // OPTIMIZED: Đăng ký event chỉ một lần duy nhất.
        UITileEntry.OnTileMapClicked += OnTileMapClickHandler;
    }

    void Start()
    {
        // Thử tải map đã lưu
        if (!LoadMap())
        {
            // Nếu không có, tạo map mới
            StartCoroutine(CreateNewMap()); 
        }
    }
    private void OnDestroy()
    {
        UITileEntry.OnTileMapClicked -= OnTileMapClickHandler;
    }

    public IEnumerator CreateNewMap()
    {
        yield return null;
        Debug.Log("Đang tạo bản đồ mới và xóa save cũ...");

        // 1. Xóa save game cũ
        DeleteSavedMap();

        // 2. Lấy map data mới

        var currentMap = GameInstance.Singleton?.currentMap;
        if (currentMap == null)
        {
            Debug.LogWarning("Không có map nào được set trong GameInstance. Sẽ chọn 1 map ngẫu nhiên.");
            GameInstance.Singleton.SetRandomCurrentMap();
            currentMap = GameInstance.Singleton?.currentMap;
        }

        // 3. Tạo dữ liệu logic
        CreateAndPopulateMap();

        // 4. Render UI (hàm này đã bao gồm xóa UI cũ)
        RenderMapFirstTime();

        // 5. Đặt vị trí player ban đầu
        UpdatePlayerTile(new Vector2Int(0, 0), true);
        OnTileMapClickHandler(playerPosition);
    }

    /// <summary>
    /// Lưu trạng thái hiện tại của bản đồ.
    /// </summary>
    public void SaveMap()
    {
        // 1. Tạo đối tượng data để lưu
        SavedMapData savedData = new SavedMapData();
        savedData.mapWidth = mapWidth;
        savedData.mapHeight = mapHeight;
        savedData.playerPosition = playerPosition;
        savedData.tiles = new SavedTileData[mapWidth * mapHeight];

        // 2. Chuyển đổi từ GridMap -> SavedMapData
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Tile tile = map.GetTile(x, y);
                SavedTileData tileData = new SavedTileData();
                tileData.Type = tile.Type;
                tileData.OccupantID = tile.OccupantID;
                
                // Chuyển từ 2D (x, y) sang 1D (index)
                savedData.tiles[y * mapWidth + x] = tileData;
            }
        }

        // 3. Serialize thành JSON và lưu vào PlayerPrefs
        string jsonData = JsonUtility.ToJson(savedData);
        PlayerPrefs.SetString(SAVE_KEY, jsonData);
        PlayerPrefs.Save();
        Debug.Log("Map đã được lưu!");
    }

    /// <summary>
    /// Tải bản đồ từ dữ liệu đã lưu.
    /// </summary>
    /// <returns>Trả về true nếu tải thành công, false nếu không có dữ liệu.</returns>
    public bool LoadMap()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY))
        {
            Debug.Log("Không tìm thấy map đã lưu.");
            return false;
        }

        try
        {
            // 1. Tải JSON và Deserialize
            string jsonData = PlayerPrefs.GetString(SAVE_KEY);
            SavedMapData savedData = JsonUtility.FromJson<SavedMapData>(jsonData);

            // 2. Thiết lập lại map
            mapWidth = savedData.mapWidth;
            mapHeight = savedData.mapHeight;
            map = new GridMap(mapWidth, mapHeight);

            // 3. Populate dữ liệu map
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    Tile tile = map.GetTile(x, y);
                    // Chuyển từ 1D (index) sang 2D (x, y)
                    SavedTileData data = savedData.tiles[y * mapWidth + x];

                    tile.Type = data.Type;
                    tile.OccupantID = data.OccupantID;

                    // Tái tạo lại Icon từ OccupantID
                    RestoreTileIcon(tile);
                }
            }

            // 4. Render map
            RenderMapFirstTime();

            // 5. Đặt player về vị trí đã lưu
            UpdatePlayerTile(savedData.playerPosition, true);

            Debug.Log("Map đã được tải từ save!");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Lỗi khi tải map: {e.Message}. Sẽ tạo map mới.");
            PlayerPrefs.DeleteKey(SAVE_KEY); // Xóa file save bị lỗi
            return false;
        }
    }

    /// <summary>
    /// Xóa dữ liệu map đã lưu.
    /// </summary>
    private void DeleteSavedMap()
    {
        DeleteMapSave(); // Sửa lại để gọi hàm static bên dưới
    }

    /// <summary>
    /// Hàm static public để xóa save từ bất kỳ script nào khác.
    /// </summary>
    public static void DeleteMapSave()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            PlayerPrefs.DeleteKey(SAVE_KEY);
            PlayerPrefs.Save();
            Debug.Log("Đã xóa dữ liệu map đã lưu (từ static).");
        }
    }

    /// <summary>
    /// Hàm hỗ trợ: Tải lại Icon cho ô dựa trên OccupantID.
    /// </summary>
    void RestoreTileIcon(Tile tile)
    {
        if (string.IsNullOrEmpty(tile.OccupantID))
        {
            tile.Icon = null;
            return;
        }

        // --- !! QUAN TRỌNG !! ---
        // Bạn CẦN phải thêm hàm GetEnemyDataByID(string id) vào GameInstance.
        // Hàm này phải có khả năng trả về EnemyData (hoặc BossData)
        // dựa trên ID của nó.
        var enemyData = GameInstance.Singleton.GetEnemyDataByID(tile.OccupantID);

        if (enemyData != null)
        {
            tile.Icon = enemyData.icon;
        }
        else if (tile.Type != TileType.Player && tile.Type != TileType.Nothing)
        {
            // Chỉ cảnh báo nếu nó *nên* có data (là Enemy/Boss)
            Debug.LogWarning($"Không tìm thấy EnemyData cho OccupantID: {tile.OccupantID}");
            tile.Icon = null; // Không tìm thấy
        }
    }


    // --- Các hàm gốc của bạn (Không thay đổi) ---

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
                    tile.OccupantID = enemy.enemyID;
                    tile.Icon = enemy.icon;
                }
                break;

            case TileType.Boss:
                var boss = GameInstance.Singleton?.currentMap?.BossData?.FirstOrDefault();
                if (boss != null)
                {
                    tile.OccupantID = boss.enemyID;
                    tile.Icon = boss.icon;
                }
                break;
        }
    }

    void RenderMapFirstTime()
    {
        if (tilePrefab == null || containter == null)
        {
            Debug.LogError("TilePrefab or Container is not assigned!");
            return;
        }

        foreach (Transform child in containter)
        {
            Destroy(child.gameObject);
        }

        tileObjects = new UITileEntry[mapWidth, mapHeight];
        
        float tileSize =( (Mathf.Min(containter.rect.width, containter.rect.height) - 10f) / mapWidth) - 10f; 
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

    void UpdateTileVisuals(int x, int y)
    {
        if (x < 0 || x >= mapWidth || y < 0 || y >= mapHeight) return;

        Tile tileData = map.GetTile(x, y);
        UITileEntry tileUI = tileObjects[x, y];

        if (tileData == null || tileUI == null) return;

        Color tileColor = tileData.IsWalkable ? walkableColor : unwalkableColor;
        tileUI.SetData(new Vector2Int(x, y), tileData.Icon, tileColor);
    }

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
        playerTile.IsWalkable = false;

        // 4. Set các ô xung quanh là walkable (trong data)
        var walkableTiles = map.GetNeighbors(playerTile);
        foreach (var tile in walkableTiles)
        {
            if (tile.Type == TileType.Enemy || tile.Type == TileType.Boss)
            {
                tile.IsWalkable = true;
            }
        }

        // 5. Cập nhật lại hình ảnh cho TẤT CẢ các ô
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
        
        // --- QUAN TRỌNG KHI LƯU ---
        // Khi người chơi rời đi, ô đó không còn Occupant
        tile.OccupantID = null; 

        UpdateTileVisuals(position.x, position.y);
    }

    void OnTileMapClickHandler(Vector2Int position)
    {
        var tile = map.GetTile(position);
        if (tile.Type == TileType.Player || tile.Type == TileType.Nothing) return;
        _currentTile = tile;
        onTileSelected?.Invoke(tile);
    }
    public void OnEnterBattle()
    {
        // Khi vào trận đấu, chúng ta nên lưu map
        // phòng trường hợp người chơi thoát game giữa chừng
        SaveMap(); 
        
        // ... (các logic khác của bạn)
    }

    public void OnBattleWin()
    {
        UpdatePlayerTile(_currentTile.Position);
        UIMap.SetActive(true);
        
        // Khi thắng, lưu lại vị trí mới của người chơi
        SaveMap();
    }

    public void SaveAndReturnToMainMenu()
    {
        // 1. Gọi hàm SaveMap() chúng ta đã tạo
        Debug.Log("Đang lưu map trước khi về menu...");
        SaveMap();

        // 2. Tải Scene Main Menu
        UnitySceneManager.LoadScene("MainMenu");
    }


}