using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public DeckData deckData;
    public CardFactory cardFactory;

    private List<Card> deck = new List<Card>();

    private void Awake()
    {
        cardFactory = GetComponent<CardFactory>();
    }

    private void Start()
    {
        InitializeDeck();
    }
    private void InitializeDeck()
    {
        foreach (var item in deckData.startingCards)
        {
            var card = cardFactory.CreateCard(item, deckData.CardBack);
            card.gameObject.SetActive(true);
            deck.Add(card);
        }
        deck = deck.OrderBy(x => Random.value).ToList();
    }
}
