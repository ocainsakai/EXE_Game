
using CardSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class DeckManager : MonoBehaviour
{
    [SerializeField] private CardFactory cardFactory;
    private List<Card> deck = new List<Card>();

    private void Awake()
    {
        if (cardFactory == null)
            cardFactory = GetComponent<CardFactory>();

        var deckData = GameInstance.Singleton.GetDeckData(PlayerSave.GetSelectedDeck());
        deckData.Cards.ForEach(card => CreateCard(card));
    }
    public void CreateCards(IEnumerable<CardData> cards)
    {
        Card.discardPile = cardFactory.discardPile.position;
        foreach (CardData card in cards)
        {
            CreateCard(card);
        }
    }
    public void CreateCard(CardData card)
    {
       deck.Add(cardFactory.CreateCard(card));
    }
    public List<Card> DrawCards(int count)
    {
        count = Mathf.Min(count, deck.Count);
        var cards = new List<Card>();
        for (int i = 0; i < count; i++)
        {
            cards.Add(DrawCard());
        }
        return cards;
    }
    public Card DrawCard()
    {
        var card = deck.FirstOrDefault();
        if (card != null) 
            deck.Remove(card);
        return card;
    }
    public void ShuffeDeck()
    {
        deck = deck.OrderBy(x => Random.value).ToList();
    }
}
