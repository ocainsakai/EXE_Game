using System.Collections.Generic;
using UnityEngine;

public class CardFactory : MonoBehaviour
{
    [SerializeField] private CardFactorySetting cardFactorySetting;
    [SerializeField] private Transform _spawnParent;
    private Queue<Card> _cardPool = new Queue<Card>();

    private void Awake()
    {
        InitializePool();
    }
    private void InitializePool()
    {
        for (int i = 0; i < cardFactorySetting._poolSize; i++)
        {
            Card newCard = Instantiate(cardFactorySetting._cardPrefab,
                                        _spawnParent);
            newCard.gameObject.SetActive(false);
            _cardPool.Enqueue(newCard);
        }
    }
    public Card CreateCard(CardData data, Sprite CardBack)
    {
        Card cardInstance;

        if (_cardPool.Count > 0)
        {
            cardInstance = _cardPool.Dequeue();
        }
        else
        {
            cardInstance = Instantiate(cardFactorySetting._cardPrefab, _spawnParent);
        }
        cardInstance.name = data.CardName;
        cardInstance.Initialize(data, CardBack);
        return cardInstance;
    }
}
