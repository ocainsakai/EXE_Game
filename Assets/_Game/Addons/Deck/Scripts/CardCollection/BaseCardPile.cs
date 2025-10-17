using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Addons.Deck.Scripts;
using _Game.Addons.Deck.Scripts.Card;
using UnityEngine;

public abstract class BaseCardPile : MonoBehaviour, ICardPile
{
    protected readonly List<CardRuntime> cards = new List<CardRuntime>();
    public virtual void Add(CardRuntime cardRuntime) { 
        if (cardRuntime == null) return;
        cards.Add(cardRuntime); 
    }
    public virtual void AddRange(IEnumerable<CardRuntime> newCards)
    {
        if (newCards == null || !newCards.Any()) return;
        cards.AddRange(newCards);
    }

    public virtual CardRuntime RemoveTop()
    {
        if (cards.Count == 0) return null;
        CardRuntime top = cards[0];
        cards.RemoveAt(0);
        return top;
    }
    public virtual bool RemoveCard(CardRuntime cardRuntime)
    {
        return cards.Remove(cardRuntime);
    }
    public virtual List<CardRuntime> RemoveAll()
    {
        var removed = new List<CardRuntime>(cards);
        cards.Clear();
        
        return removed;
    }

    public int Count => cards.Count;
    public IReadOnlyList<CardRuntime> Cards => cards.AsReadOnly();
    protected void SortCards(Comparison<CardRuntime> comparison)
    {
        cards.Sort(comparison);
    }

}

