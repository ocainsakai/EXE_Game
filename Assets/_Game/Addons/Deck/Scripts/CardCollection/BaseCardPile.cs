using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseCardPile : MonoBehaviour, ICardPile
{
    protected readonly List<Card> cards = new List<Card>();
    public virtual void Add(Card card) { 
        if (card == null) return;
        cards.Add(card); 
    }
    public virtual void AddRange(IEnumerable<Card> newCards)
    {
        if (newCards == null || !newCards.Any()) return;
        cards.AddRange(newCards);
    }

    public virtual Card RemoveTop()
    {
        if (cards.Count == 0) return null;
        Card top = cards[0];
        cards.RemoveAt(0);
        return top;
    }
    public virtual bool RemoveCard(Card card)
    {
        return cards.Remove(card);
    }
    public virtual List<Card> RemoveAll()
    {
        var removed = new List<Card>(cards);
        cards.Clear();
        
        return removed;
    }

    public int Count => cards.Count;
    public IReadOnlyList<Card> Cards => cards.AsReadOnly();
    protected void SortCards(Comparison<Card> comparison)
    {
        cards.Sort(comparison);
    }

}

