using CardSystem.PokerSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityUtils;

public class Room : MonoBehaviour
{
    [SerializeField] private BaseCardPile discard;
    [SerializeField] private DeckManager deck;
    [SerializeField] private MultTable multTable;
    [SerializeField] private UIPokerMult _uIPokerMult;

    private List<Card> _cards = new List<Card>();
    private List<Card> runtimeDeck = new List<Card>();

    public UnityEvent<List<Card>> OnCardsChanged;
    public UnityEvent<List<Card>> OnCardAdd;
    public UnityEvent<List<Card>> OnCardDiscard;
    public Action OnDiscardComplete;

    public int RoomSize = 8;

    public UnityEvent<bool> CanSelectCardChanged;
    private bool _canSelectCard = true;
    public bool CanSelectCard
    {
        get => _canSelectCard;
        set
        {
            if (_canSelectCard != value)
            {
                _canSelectCard = value;
                CanSelectCardChanged?.Invoke(value);
            }
        }
    }

    private bool sortToggle = true;

    public List<Card> SelectedCards => _cards.Where(x => x.IsSeleced).ToList();
    public List<Card> Cards => new List<Card>(_cards); // Return copy để tránh modify từ bên ngoài

    private void OnEnable()
    {
        runtimeDeck = deck.OriginCards.ToList();
        ShuffleDeck();
        Draw();
    }

    private void OnDisable()
    {
        // Unsubscribe events trước khi clear
        foreach (var card in _cards)
        {
            card.SelectedChanged -= UpdateSelected;
        }

        var allCards = new List<Card>(_cards);
        _cards.Clear();
        runtimeDeck.Clear();

        OnCardDiscard?.Invoke(allCards);
    }

    public void Add(Card card)
    {
        if (card == null || _cards.Contains(card)) return;

        _cards.Add(card);
        card.SelectedChanged += UpdateSelected;
        card.IsSeleced = false;
    }

    public void Draw()
    {
        int cardsNeeded = RoomSize - _cards.Count;
        if (cardsNeeded <= 0) return;

        var cardsToDraw = runtimeDeck.Take(cardsNeeded).ToList();

        foreach (var card in cardsToDraw)
        {
            Add(card);
        }

        runtimeDeck.RemoveRange(0, cardsToDraw.Count);
        Debug.Log($"Deck remaining: {runtimeDeck.Count}");

        Sort(sortToggle);

        OnCardAdd?.Invoke(cardsToDraw);
        OnCardsChanged?.Invoke(_cards);

        UpdateSelected();
    }

    public void Discards()
    {
        var cardsToDiscard = SelectedCards;

        if (cardsToDiscard.Count == 0)
        {
            Debug.LogWarning("No cards selected to discard!");
            return;
        }

        // Unsubscribe events
        foreach (var card in cardsToDiscard)
        {
            card.SelectedChanged -= UpdateSelected;
        }

        _cards.RemoveAll(x => cardsToDiscard.Contains(x));

        OnCardDiscard?.Invoke(cardsToDiscard);
        OnCardsChanged?.Invoke(_cards);

        Draw();

        OnDiscardComplete?.Invoke();
    }

    public void Sort()
    {
        sortToggle = !sortToggle;
        Sort(sortToggle);
        OnCardsChanged?.Invoke(_cards);
    }

    private void Sort(bool isBySuit)
    {
        if (isBySuit)
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
            if (rankCompare != 0) return rankCompare;
            return a.Suit.CompareTo(b.Suit);
        });
    }

    public void SortBySuit()
    {
        SortCards((a, b) =>
        {
            int suitCompare = a.Suit.CompareTo(b.Suit);
            if (suitCompare != 0) return suitCompare;
            return a.Rank.CompareTo(b.Rank);
        });
    }

    protected void SortCards(Comparison<Card> comparison)
    {
        _cards.Sort(comparison);
    }

    public void ShuffleDeck()
    {
        for (int i = runtimeDeck.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            (runtimeDeck[i], runtimeDeck[randomIndex]) = (runtimeDeck[randomIndex], runtimeDeck[i]);
        }
    }

    private void UpdateSelected()
    {
        int selectedCount = SelectedCards.Count;
        CanSelectCard = selectedCount < 5;

        Debug.Log($"Selected: {selectedCount}/5, CanSelect: {CanSelectCard}");

        if (selectedCount > 0)
        {
            var pokerHand = PokerEvaluator.Evaluate(SelectedCards.Select(x => x.Mask).ToList());
            UpdateUIPoker(pokerHand);
        }
        else
        {
            // Clear poker UI khi không có card nào được chọn
            if (_uIPokerMult != null)
            {
                _uIPokerMult.SetPokerMult(PokerHandType.HighCard, 0);
            }
        }
    }

    private void UpdateUIPoker(PokerHandResult pokerHand)
    {
        if (_uIPokerMult != null)
        {
            float mult = multTable.GetMult(pokerHand.HandType);
            _uIPokerMult.SetPokerMult(pokerHand.HandType, mult);
        }
    }
}