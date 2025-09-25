using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CardPileExtensions
{
    public static void MoveCardTo(this ICardPile source, ICardPile target, Card card)
    {
        if (source.Cards.Contains(card))
        {
            source.RemoveCard(card);
            target.Add(card);
        }
    }
    public static IEnumerable<Card> DrawFrom(this ICardPile sources, ICardPile target, int amount)
    {
        var drawnCards = new List<Card>();
        if (amount > target.Count) return drawnCards;
        for (int i = 0; i < amount; i++)
        {
            var card = target.RemoveTop();
            Debug.Log($"Draw card {card.CardData.Name}");
            sources.Add(card);
            drawnCards.Add(card);
        }
        return drawnCards;
    }
    public static void MoveAllTo(this ICardPile source, ICardPile target)
    {
        var all = source.RemoveAll();
        target.AddRange(all);
    }
}
