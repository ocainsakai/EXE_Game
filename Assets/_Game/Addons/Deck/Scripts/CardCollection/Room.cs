using CardSystem.PokerSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityUtils;

public class Room : MonoBehaviour
{
    [SerializeField] BaseCardPile discard;
    [SerializeField] DeckManager deck;
    [SerializeField] UIPokerMult _uIPokerMult;

    private List<Card> _cards = new List<Card>();
    private List<Card> runtimeDeck = new();

    public UnityEvent<List<Card>> OnCardsChanged;
    public Action OnDiscardComplete;
    public int RoomSize = 8;


    public UnityEvent<bool> CanSelectCardChanged;
    private bool _canSelectCard;
    public bool CanSelectCard
    {
        get => _canSelectCard;
        set {
            _canSelectCard = value;
            CanSelectCardChanged?.Invoke(value);
        }
    }
    private bool sortToggle = true;
    public List<Card> SetectedCards => _cards.Where(x => x.IsSeleced).ToList();
    public void Add(Card card)
    {
        _cards.Add(card);
        card.SelectedChanged += UpdateSelected;
        card.IsSeleced = false;
    }
    private void UpdateSelected()
    {
        CanSelectCard = _cards.Where(x => x.IsSeleced).Count() < 5;
        Debug.Log(CanSelectCard);
        var pokerHand = PokerEvaluator.Evaluate(SetectedCards.Select(x => x.Mask).ToList());

        if (_uIPokerMult != null)
        {
            _uIPokerMult.SetPokerMult(pokerHand.HandType);
        }
    }
    public void OnEnable()
    {
        runtimeDeck = deck.OriginCards.ToList();
        ShuffleDeck();
        Draw();
    }
    public void Draw()
    {
        var cards = runtimeDeck.Take(RoomSize - _cards.Count);   
        cards.ForEach(x => Add(x));
        Sort(sortToggle);
        OnCardsChanged?.Invoke(_cards);
        UpdateSelected();

    }
    public void OnDisable()
    {
        runtimeDeck.Clear();
        _cards.Clear();
        OnCardsChanged.Invoke(_cards);
    }
    public void Discards()
    {
        var cardsToDiscard = _cards.Where(x => x.IsSeleced).ToList();
        OnCardsChanged?.Invoke(_cards);
        Draw();
    }
    public void Sort()
    {
        sortToggle = !sortToggle;
        Sort(sortToggle);

        OnCardsChanged?.Invoke(_cards);
    }
    public void ShuffleDeck()
    {
        for (int i = runtimeDeck.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            (runtimeDeck[i], runtimeDeck[randomIndex]) = (runtimeDeck[randomIndex], runtimeDeck[i]);
        }
    }
    private void Sort(bool isRank)
    {
        if (isRank)
        {
            SortBySuit();
        }
        else
        {
            SortByRank();
        }
    }
    public void SortByRank()
    {
        SortCards((a, b) =>
        {
            int rankCompare = a.Rank.CompareTo(b.Rank);
            return rankCompare != 0 ? rankCompare : a.Rank.CompareTo(b.Rank);
        });
    }
    public void SortBySuit()
    {
        SortCards((a, b) =>
        {   
            int rankCompare = a.Suit.CompareTo(b.Suit);
            return rankCompare != 0 ? rankCompare : a.Suit.CompareTo(b.Suit);
        });

    }
    protected void SortCards(Comparison<Card> comparison)
    {
        _cards.Sort(comparison);
    }
}
