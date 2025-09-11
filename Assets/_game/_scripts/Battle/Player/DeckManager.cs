
using CardSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class DeckManager : MonoBehaviour
{
    [SerializeField] private CardFactory cardFactory;
    private List<CardController> deck = new List<CardController>();
    public void CreateCards(IEnumerable<CardData> cards)
    {
        foreach (CardData card in cards)
        {
            CreateCard(card);
        }
    }
    public void CreateCard(CardData card)
    {
       deck.Add(cardFactory.CreateCard(card));
    }
    public List<CardController> DrawCards(int count)
    {
        count = Mathf.Min(count, deck.Count);
        var cards = new List<CardController>();
        for (int i = 0; i < count; i++)
        {
            cards.Add(DrawCard());
        }
        return cards;
    }
    public CardController DrawCard()
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
