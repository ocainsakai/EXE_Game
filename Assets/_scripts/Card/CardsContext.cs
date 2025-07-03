using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class CardsContext : MonoBehaviour
{
    public List<Card> hand = new List<Card>();
    public List<Card> deck = new List<Card>();
    public List<Card> discardPile = new List<Card>();
    public ReactiveCollection<Card> selected = new ReactiveCollection<Card>();
    public ReactiveCollection<Card> played = new ReactiveCollection<Card>();
    public ReactiveCollection<Card> scored = new ReactiveCollection<Card>();

    public void SortByRank()
    {
        hand = hand.OrderBy(x => x.Data.Rank).ThenBy(x => x.Data.Suit).ToList();
    }
    public void SortBySuit()
    {
        hand = hand.OrderBy(x => x.Data.Suit).ThenBy(x => x.Data.Rank).ToList();
    }
}
