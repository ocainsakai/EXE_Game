using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardFactory : MonoBehaviour
{
    [Header("Card Prefab Reference")]
    public AssetReference cardPrefabRef;
    public AssetLabelReference cardDatas;

    private GameObject cardPrefab; // Cache prefab đã load

    // Load prefab 1 lần duy nhất
    private async Task LoadCardPrefab()
    {
        if (cardPrefab != null) return;

        var handle = cardPrefabRef.LoadAssetAsync<GameObject>();
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            cardPrefab = handle.Result;
        }
        else
        {
            Debug.LogError("Failed to load card prefab from Addressables");
        }
    }

    // Spawn 1 card
    public GameObject SpawnCardPrf(CardSDData data)
    {
        if (cardPrefab == null)
        {
            Debug.LogError("Card prefab is not loaded yet!");
            return null;
        }

        var cardGO = Instantiate(cardPrefab, transform);
        cardGO.name = "Card";
        cardGO.GetComponent<CardDataHolder>().Data.Value = data;
        return cardGO;
    }

    // Load toàn bộ card data
    public async Task<List<CardSDData>> LoadAllCardsFromLabel()
    {
        List<CardSDData> loadedCards = new List<CardSDData>();

        var handle = Addressables.LoadAssetsAsync<CardSDData>(
            cardDatas,
            card => loadedCards.Add(card)
        );

        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"Loaded {loadedCards.Count} cards from label {cardDatas.labelString}");
        }
        else
        {
            Debug.LogError($"Failed to load cards from label {cardDatas.labelString}");
        }

        return loadedCards;
    }

    // Load và spawn toàn bộ card
    private async void Awake()
    {
        await LoadCardPrefab();

        var cards = await LoadAllCardsFromLabel();

        foreach (var card in cards)
        {
            SpawnCardPrf(card);
        }
    }
}
