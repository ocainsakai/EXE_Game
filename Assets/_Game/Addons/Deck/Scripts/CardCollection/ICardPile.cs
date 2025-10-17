using System.Collections.Generic;
using _Game.Addons.Deck.Scripts;
using _Game.Addons.Deck.Scripts.Card;

public interface ICardPile
{
    void Add(CardRuntime cardRuntime);
    void AddRange(IEnumerable<CardRuntime> cards);
    CardRuntime RemoveTop();
    bool RemoveCard(CardRuntime cardRuntime);
    List<CardRuntime> RemoveAll();
    int Count { get; }
    IReadOnlyList<CardRuntime> Cards { get; }
}
