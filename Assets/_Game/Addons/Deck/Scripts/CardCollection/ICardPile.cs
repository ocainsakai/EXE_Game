using System.Collections.Generic;

public interface ICardPile
{
    void Add(Card card);
    void AddRange(IEnumerable<Card> cards);
    Card RemoveTop();
    bool RemoveCard(Card card);
    List<Card> RemoveAll();
    int Count { get; }
    IReadOnlyList<Card> Cards { get; }
}
