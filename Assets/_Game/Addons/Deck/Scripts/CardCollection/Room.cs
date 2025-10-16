using CardSystem.PokerSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Room : MonoBehaviour
{
    private List<Card> _cards = new List<Card>();

    public UnityEvent<List<Card>> OnCardsChanged;
    public UnityEvent<Card> OnCardAdd;
    public UnityEvent<Card> OnCardDiscard;
    public UnityEvent<PokerHandResult> OnPokerHandResult;
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

    private bool currentSort = true;

    public List<Card> SelectedCards => _cards.Where(x => x.IsSelected).ToList();
    public List<Card> Cards => new List<Card>(_cards); // Return copy để tránh modify từ bên ngoài

    public List<Card> GetSelectCards => Cards.Where(x => x.IsSelected).ToList();

    private void OnDisable()
    {
        // Unsubscribe events trước khi clear
        foreach (var card in _cards)
        {
            card.RequestSelectedChanged -= UpdateSelected;
            OnCardDiscard?.Invoke(card);
        }
        _cards.Clear();
    }

   
    public void Add(Card card)
    {
        //Debug.Log(card.ToString());
        if (card == null || _cards.Contains(card)) return;

        _cards.Add(card);
        //card.SelectedChanged += UpdateSelected;
        card.RequestSelectedChanged += UpdateSelected;
        UpdateSelected();
        OnCardAdd?.Invoke(card);
    }


    public void Discards()
    {
        var cardsToDiscard = SelectedCards;

        if (cardsToDiscard.Count == 0)
        {
            Debug.LogWarning("No cards selected to discard!");
            return;
        }

        _cards.RemoveAll(x => cardsToDiscard.Contains(x));
        foreach (var card in cardsToDiscard)
        {
            card.RequestSelectedChanged -= UpdateSelected;
            OnCardDiscard?.Invoke(card);
        }

        OnCardsChanged?.Invoke(_cards);
        OnDiscardComplete?.Invoke();
    }

    public void Sort()
    {
        currentSort = !currentSort;
        Sort(currentSort);
        OnCardsChanged?.Invoke(_cards);
    }

    public void CurrentSort()
    {
        Sort(currentSort);
        OnCardsChanged?.Invoke(_cards);

    }
    public void Sort(bool isBySuit)
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

  
    private void UpdateSelected()
    {
        //Debug.Log($"{gameObject} + {gameObject.name} + UpdateSelected ");

        int selectedCount = SelectedCards.Count;
        Card.CanSelect = selectedCount < 5;

        if (selectedCount > 0)
        {
            var pokerHand = PokerEvaluator.Evaluate(SelectedCards.Select(x => x.Mask).ToList());
            OnPokerHandResult?.Invoke(pokerHand);
        }
    }
    public void UnselectAll()
    {
        foreach (var card in _cards)
        {
            card.IsSelected = false;
        }
        OnCardsChanged?.Invoke(_cards);

    }
}