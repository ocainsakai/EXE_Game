using System;
using System.Linq;
using _Game.Addons.Deck.Scripts;
using _Game.Core.Gameplay;
using CardSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace _Game.Core
{
    [DefaultExecutionOrder(-1)]
    public class GameInstance : MonoBehaviour
    {
        // --- Deck ---
        public DeckData[] deckData;

        // --- Card ---
        public CardData[] cardsData;

        // --- Map ---
        public MapData[] maps;         // danh sách map
        [HideInInspector]
        public MapData currentMap;     // map đang chơi
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

        //public float GetMult
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
            currentMap = maps.FirstOrDefault(m => m.mapID == mapID);
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
            Debug.Log($"Random Map selected: {currentMap.mapID}");
        }

        public EnemyData GetEnemyData(string id)
        {
            if (currentMap == null)
            {
                Debug.LogError("Current map is not set!");
                return null;
            }
            return currentMap.EnemyData.FirstOrDefault(e => e.enemyID == id);
        }

        public EnemyData GetRandomEnemy()
        {
            if (currentMap == null || currentMap.EnemyData == null || currentMap.EnemyData.Length == 0)
                return null;
            return currentMap.EnemyData[Random.Range(0, currentMap.EnemyData.Length)];
        }

        public BossData GetBoss(string occupantID)
        {
            if (currentMap == null)
            {
                Debug.LogError("Current map is not set!");
                return null;
            }
            return currentMap.BossData.FirstOrDefault(x => x.enemyID == occupantID);
        }

        public EnemyData GetEnemyDataByID(string id)
        {
            // Kiểm tra an toàn
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            // 'maps' là mảng (array) chứa TẤT CẢ MapData
            // mà GameInstance của bạn quản lý.
            if (maps == null || maps.Length == 0)
            {
                Debug.LogError("GameInstance.maps chưa được gán hoặc bị rỗng!");
                return null;
            }

            // Lặp qua TẤT CẢ các map có trong GameInstance
            foreach (var map in maps)
            {
                if (map == null) continue;

                // 1. Tìm trong danh sách EnemyData của map này
                if (map.EnemyData != null)
                {
                    foreach (var enemy in map.EnemyData)
                    {
                        if (enemy != null && enemy.enemyID == id)
                            return enemy;
                    }
                }

                // 2. Tìm trong danh sách BossData của map này
                if (map.BossData != null)
                {
                    foreach (var boss in map.BossData)
                    {
                        // Giả định BossData cũng là một dạng EnemyData
                        if (boss != null && boss.enemyID == id)
                            return boss;
                    }
                }
            }

            // Không tìm thấy ở bất cứ đâu
            Debug.LogWarning($"GetEnemyDataByID: Không tìm thấy EnemyData cho ID: {id}");
            return null;
        }

    }
}
