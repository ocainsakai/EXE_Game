using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class CardsContext : MonoBehaviour
{
    public List<Card> hand = new List<Card>();
    public List<Card> deck = new List<Card>();
    public List<Card> discardPile = new List<Card>();
    public List<Card> played = new List<Card>();
    public List<Card> scored = new List<Card>();
    public ReactiveCollection<Card> selected = new ReactiveCollection<Card>();


    public void ResetHand()
    {
        selected.Clear();
        hand.ForEach(x => x.Unselect());
    }
    public List<Card> GetSelected()
    {
        var cards = hand.FindAll(x => selected.Contains(x));
        ResetHand();
        return cards;
    }
    public List<Card> TakeSelect()
    {
        var list = GetSelected();
        hand.RemoveAll(x => list.Contains(x));
        return list;
    }
    public void SortByRank()
    {
        hand = hand.OrderBy(x => x.Data.Rank).ThenBy(x => x.Data.Suit).ToList();
    }
    public void SortBySuit()
    {
        hand = hand.OrderBy(x => x.Data.Suit).ThenBy(x => x.Data.Rank).ToList();
    }
}
