using System.Collections.Generic;
using System.Linq;
using _Game.Addons.Deck.Scripts;
using _Game.Addons.Deck.Scripts.Card;
using UnityEngine;

public static class CardPileExtensions
{
    public static void MoveCardTo(this ICardPile source, ICardPile target, CardRuntime cardRuntime)
    {
        if (source.Cards.Contains(cardRuntime))
        {
            source.RemoveCard(cardRuntime);
            target.Add(cardRuntime);
        }
    }
    public static IEnumerable<CardRuntime> DrawFrom(this ICardPile sources, ICardPile target, int amount)
    {
        var drawnCards = new List<CardRuntime>();
        if (amount > target.Count) return drawnCards;
        for (int i = 0; i < amount; i++)
        {
            var card = target.RemoveTop();
            Debug.Log($"Draw card {card.Name}");
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
