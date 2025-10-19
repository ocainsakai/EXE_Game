using CardSystem.PokerSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Addons.Deck.Scripts.Card;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Room : MonoBehaviour
{
    private List<CardRuntime> _cards = new List<CardRuntime>();

    public UnityAction<List<CardRuntime>> OnCardsChanged;
    public UnityAction<CardRuntime> OnCardAdd;
    public UnityAction<CardRuntime> OnCardDiscard;
    public UnityEvent<PokerHandResult> onPokerHandResult;
    public Action OnDiscardComplete;

    [FormerlySerializedAs("RoomSize")] public int roomSize = 8;

    [FormerlySerializedAs("CanSelectCardChanged")] public UnityEvent<bool> canSelectCardChanged;
    private bool _canSelectCard = true;
    public bool CanSelectCard
    {
        get => _canSelectCard;
        set
        {
            _canSelectCard = value;
            canSelectCardChanged?.Invoke(value);
        }
    }

    private bool currentSort = true;

    public int SelectCount => SelectedCards?.Count ?? 0;
    public List<CardRuntime> SelectedCards => _cards?.Where(x => x.IsSelected).ToList();
    public IReadOnlyList<CardRuntime> Cards => _cards; // Return copy để tránh modify từ bên ngoài
    
    private void OnDisable()
    {
        // Unsubscribe events trước khi clear
        foreach (var card in _cards)
        {
            card.SelectedChanged -= UpdateSelected;
        }
        _cards.Clear();
    }

   
    public void Add(CardRuntime cardRuntime)
    {
        Debug.Log("add to room");
        if (cardRuntime == null || _cards.Contains(cardRuntime)) return;

        _cards.Add(cardRuntime);
        cardRuntime.SelectedChanged += UpdateSelected;
        UpdateSelected();   
        Debug.Log("added to room on event"); 
        OnCardAdd?.Invoke(cardRuntime);
    }

    public void UpdateUI()
    {
        OnCardsChanged?.Invoke(_cards); 
    }

    public void Discards()
    {
        var cardsToDiscard = SelectedCards;

        // Cách kiểm tra an toàn và gọn gàng nhất
        if (cardsToDiscard == null || !cardsToDiscard.Any())
        {
            Debug.LogWarning("No cards selected to discard!");
            return;
        }

        _cards.RemoveAll(x => cardsToDiscard.Contains(x));
        foreach (var card in cardsToDiscard)
        {
            card.SelectedChanged -= UpdateSelected;
            OnCardDiscard?.Invoke(card);
        }

        UpdateUI();
        OnDiscardComplete?.Invoke();
    }

    public void Sort()
    {
        currentSort = !currentSort;
        Sort(currentSort);
        UpdateUI();
    }

    public void CurrentSort()
    {
        Sort(currentSort);
        UpdateUI();
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

    private void SortCards(Comparison<CardRuntime> comparison)
    {
        _cards.Sort(comparison);
    }

  
    private void UpdateSelected()
    {        
        CanSelectCard = SelectCount < 5;

        if (SelectCount > 0)
        {
            var pokerHand = PokerEvaluator.Evaluate(SelectedCards.Select(x => x.Mask).ToList());
            onPokerHandResult?.Invoke(pokerHand);
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

    public void Remove(CardRuntime card)
    {
    }

    public void Clear()
    {
    }
}