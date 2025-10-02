using CardSystem;
using System.Linq;
using UnityEngine;


[DefaultExecutionOrder(-1)]
public class GameInstance : MonoBehaviour
{

    public DeckData[] deckDatas;
    public CardData[] cardsData;

    public EnemyData[] enemyDatas;
    public BossData[] bossDatas;

    public PlayerData[] playerData;

    public static GameInstance Singleton;
    private bool isInitialized;

    /// <summary>
    /// Initializes the GameInstance as a singleton.
    /// </summary>
    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
            isInitialized = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool IsInitialized()
    {
        return isInitialized;
    }
    public DeckData GetDeckData(int id)
    {
        foreach (var deck in deckDatas)
        {
            if (deck.DeckID == id)
            {
                return deck;
            }
        }
        Debug.LogError($"DeckData with ID {id} not found");
        return null;
    }

    //public float GetMult
    public DeckData GetDeckData()
    {
        int selectedId = PlayerSave.GetSelectedDeck();
        var character = deckDatas.FirstOrDefault(c => c.DeckID == selectedId);
        if (character == null)
            Debug.LogWarning($"CharacterData with ID '{selectedId}' not found.");
        return character;
    }

    public CardData GetCardData(CardMask cardMask)
    {
        foreach (var card in cardsData)
        {
            if (card.Mask.Equals(cardMask))
            {
                return card;
            }
        }
        Debug.LogError($"CardData with ID {cardMask} not found");
        return null;

    }

    public EnemyData GetEnemyData(string id)
    {
        foreach (var enemy in enemyDatas)
        {
            if (enemy.EnemyID == id)
            {
                return enemy;
            }
        }
        Debug.LogError($"EnemyData with ID {id} not found");
        return null;
    }
    public EnemyData GetRandomEnemy()
    {
        return enemyDatas[Random.Range(0, enemyDatas.Length)];
    }

    public EnemyData GetBoss(string occupantID)
    {
        return bossDatas.FirstOrDefault(x => x.EnemyID == occupantID);
    }
}
