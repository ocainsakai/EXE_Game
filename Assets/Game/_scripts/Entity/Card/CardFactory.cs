using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CardFactory : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private Transform _cardPoolParent;
    [SerializeField] private int _defaultPoolSize = 20;
    [SerializeField] private int _maxPoolSize = 100;

    private Dictionary<CardSDData, ObjectPool<Card>> _cardPools;
    private Dictionary<Card, CardSDData> _cardToDataMap;

    protected void Awake()
    {
        
        InitializePools();
    }

    private void InitializePools()
    {
        _cardPools = new Dictionary<CardSDData, ObjectPool<Card>>();
        _cardToDataMap = new Dictionary<Card, CardSDData>();
    }

    public Card GetCard(CardSDData data)
    {
        if (!_cardPools.ContainsKey(data))
        {
            CreateNewPool(data);
        }

        return _cardPools[data].Get();
    }

    public List<Card> GetCards(IEnumerable<CardSDData> cardDataList)
    {
        List<Card> cards = new List<Card>();
        foreach (var data in cardDataList)
        {
            cards.Add(GetCard(data));
        }
        return cards;
    }

    private void CreateNewPool(CardSDData data)
    {
        _cardPools[data] = new ObjectPool<Card>(
            createFunc: () => CreateCardInstance(data),
            actionOnGet: card => OnCardGet(card),
            actionOnRelease: card => OnCardRelease(card),
            actionOnDestroy: card => Destroy(card.gameObject),
            collectionCheck: true,
            defaultCapacity: _defaultPoolSize,
            maxSize: _maxPoolSize
        );
    }

    private Card CreateCardInstance(CardSDData data)
    {
        var cardObject = Instantiate(_cardPrefab, _cardPoolParent);
        var card = cardObject.GetComponent<Card>();
        card.dataHolder.Data.Value = data;
        _cardToDataMap[card] = data;
        return card;
    }

    private void OnCardGet(Card card)
    {
        card.gameObject.SetActive(true);
        card.ResetState();
    }

    private void OnCardRelease(Card card)
    {
        card.gameObject.SetActive(false);
        card.transform.SetParent(_cardPoolParent);
        card.transform.localPosition = Vector3.zero;
    }

    public void ReturnCard(Card card)
    {
        if (card == null || !_cardToDataMap.TryGetValue(card, out var data))
        {
            Debug.LogWarning("Attempted to return an unknown card to pool");
            Destroy(card?.gameObject);
            return;
        }

        _cardPools[data].Release(card);
    }

    public void ReturnCards(IEnumerable<Card> cards)
    {
        foreach (var card in cards)
        {
            ReturnCard(card);
        }
    }

    private void OnDestroy()
    {
        _cardPools.Clear();
        _cardToDataMap.Clear();
    }
}