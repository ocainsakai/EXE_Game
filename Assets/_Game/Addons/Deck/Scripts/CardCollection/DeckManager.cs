using CardSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : BaseCardPile
{
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform cardContainer;
    [SerializeField] private Transform deckCover;
    private void Awake()
    { 
        var deckData = GameInstance.Singleton.GetDeckData(PlayerSave.GetSelectedDeck());
        deckCover.GetComponent<Image>().sprite = deckData.DeckCover;
        CreateCards(deckData.Cards);
        ShuffleDeck();
    }
    public Card CreateCard(CardData data)
    {
        Card card = new(data);
        cards.Add(card);

        // create UI
        UICardManager.Singleton.Add(card, cardContainer);
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
        // Fisher–Yates shuffle (ổn định, nhanh hơn OrderBy)
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (cards[i], cards[randomIndex]) = (cards[randomIndex], cards[i]);
        }
    }
}
