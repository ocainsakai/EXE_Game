using CardSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : BaseCardPile
{
    [SerializeField] UICardManager cardManager;
    [SerializeField] private Transform cardContainer;
    [SerializeField] private Transform deckCover;

    private void Awake()
    {
        // ✅ Load toàn bộ CardData từ Resources/Cards
        var allCards = Resources.LoadAll<CardData>("Cards");
        if (allCards == null || allCards.Length == 0)
        {
            Debug.LogError("Không tìm thấy CardData trong Resources/Cards");
            return;
        }

        CreateCards(allCards);
        ShuffleDeck();
    }

    public Card CreateCard(CardData data)
    {
        Card card = new(data);
        cards.Add(card);

        // tạo UI
        if (cardManager != null)
            cardManager.Add(card, cardContainer);

        return card;
    }

    public void CreateCards(IEnumerable<CardData> cardsData)
    {
        foreach (var cardData in cardsData)
        {
            CreateCard(cardData);
        }
    }

    public void ShuffleDeck()
    {
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (cards[i], cards[randomIndex]) = (cards[randomIndex], cards[i]);
        }
    }
}
