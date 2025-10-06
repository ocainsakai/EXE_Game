using CardSystem;
using System.Linq;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    [System.Serializable]
    public class MapData
    {
        public string MapID;
        public EnemyData[] enemyData;
        public BossData[] bossData;
    }

    // --- Deck ---
    public DeckData[] deckData;

    // --- Card ---
    public CardData[] cardsData;

    // --- Map ---
    public MapData[] maps;         // danh sách map
    public MapData currentMap;     // map đang chơi

    // --- Player ---
    public PlayerData[] playerData;

    // --- Singleton ---
    public static GameInstance Singleton;
    private bool isInitialized;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
            isInitialized = true;
            SetRandomCurrentMap();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsInitialized() => isInitialized;

    // =====================
    // Deck
    // =====================
    public DeckData GetDeckData(int id)
    {
        return deckData.FirstOrDefault(deck => deck.DeckID == id);
    }

    public DeckData GetDeckData()
    {
        int selectedId = PlayerSave.GetSelectedDeck();
        var character = deckData.FirstOrDefault(c => c.DeckID == selectedId);
        if (character == null)
            Debug.LogWarning($"CharacterData with ID '{selectedId}' not found.");
        return character;
    }

    // =====================
    // Card
    // =====================
    public CardData GetCardData(CardMask cardMask)
    {
        var card = cardsData.FirstOrDefault(c => c.Mask.Equals(cardMask));
        if (card == null)
            Debug.LogError($"CardData with ID {cardMask} not found");
        return card;
    }

    // =====================
    // Map + Enemy + Boss
    // =====================

    public void SetCurrentMap(string mapID)
    {
        currentMap = maps.FirstOrDefault(m => m.MapID == mapID);
        if (currentMap == null)
            Debug.LogError($"Map with ID {mapID} not found!");
    }

    public void SetRandomCurrentMap()
    {
        if (maps == null || maps.Length == 0)
        {
            Debug.LogError("No maps available in GameInstance!");
            currentMap = null;
            return;
        }

        currentMap = maps[Random.Range(0, maps.Length)];
        Debug.Log($"Random Map selected: {currentMap.MapID}");
    }

    public EnemyData GetEnemyData(string id)
    {
        if (currentMap == null)
        {
            Debug.LogError("Current map is not set!");
            return null;
        }
        return currentMap.enemyData.FirstOrDefault(e => e.EnemyID == id);
    }

    public EnemyData GetRandomEnemy()
    {
        if (currentMap == null || currentMap.enemyData == null || currentMap.enemyData.Length == 0)
            return null;
        return currentMap.enemyData[Random.Range(0, currentMap.enemyData.Length)];
    }

    public BossData GetBoss(string occupantID)
    {
        if (currentMap == null)
        {
            Debug.LogError("Current map is not set!");
            return null;
        }
        return currentMap.bossData.FirstOrDefault(x => x.EnemyID == occupantID);
    }
}
