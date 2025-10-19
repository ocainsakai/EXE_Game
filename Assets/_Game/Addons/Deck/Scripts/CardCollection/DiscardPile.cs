using System.Collections.Generic;
using _Game.Addons.Deck.Scripts;
using _Game.Addons.Deck.Scripts.Card;
using DG.Tweening;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    [SerializeField] UICardFactory _cardManager;
    [SerializeField] private Transform discard;
    
    private List<CardRuntime> discardPile = new List<CardRuntime>();    
    
    public void Add(CardRuntime cardRuntime)
    {
        discardPile.Add(cardRuntime);   
    }

    public void AddRange(List<CardRuntime> cardsToDiscard)
    {
        discardPile.AddRange(cardsToDiscard);
    }
    public void Clear()
    {
        discardPile.Clear();      
    }

    public List<CardRuntime> TakeAllCards()
    {
        var list = new List<CardRuntime>(discardPile);
        Clear();
        return list; 
    }
}
