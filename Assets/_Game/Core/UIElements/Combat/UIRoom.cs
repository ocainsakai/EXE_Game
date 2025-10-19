// UIRoom.cs - Refactored (Event-Driven)
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using _Game.Addons.Deck.Scripts.Card;
using UnityEngine;

public class UIRoom : MonoBehaviour
{
    [SerializeField] private UICardFactory cardFactory;
    [SerializeField] private Room room; // << QUAN TRỌNG: Phải được gán trong Inspector

    private List<CardEntry> _cardEntries = new List<CardEntry>();

    private void Start()
    {
        if (room == null)
        {
            Debug.LogError("Room reference is not set in UIRoom!", this);
            return;
        }
        
        // Đăng ký lắng nghe các sự kiện từ Room
        room.OnCardAdd += (HandleCardAdded);
        room.OnCardDiscard += (HandleCardDiscarded);
        room.OnCardsChanged += (HandleCardsChanged);
        room.canSelectCardChanged.AddListener(UpdateInteract);

        // Đồng bộ trạng thái ban đầu
        HandleCardsChanged(room.Cards.ToList());
    }

    private void OnDestroy()
    {
        if (room != null)
        {
            room.OnCardAdd -= (HandleCardAdded);
            room.OnCardDiscard -= (HandleCardDiscarded);
            room.OnCardsChanged -= (HandleCardsChanged);
            room.canSelectCardChanged.RemoveListener(UpdateInteract);
        }
    }

    private void HandleCardAdded(CardRuntime cardRuntime)
    {
        Debug.Log("Card added");    
        if (_cardEntries.Any(entry => entry.CardID == cardRuntime.CardID)) return;
        Debug.Log("Card added after discarding");
        var entry = cardFactory.GetOrCreateCard(cardRuntime);
        entry.SetRoom(room);
        entry.transform.SetParent(transform, false);
        _cardEntries.Add(entry);
        RepositionCards();
    }

    private void HandleCardDiscarded(CardRuntime cardRuntime)
    {
        var entryToRemove = _cardEntries.FirstOrDefault(e => e.CardID == cardRuntime.CardID);
        if (entryToRemove != null)
        {
            _cardEntries.Remove(entryToRemove);
            cardFactory.ReturnToPool(entryToRemove);
            RepositionCards();
        }
    }
    
    private void HandleCardsChanged(List<CardRuntime> newCardOrder)
    {
        if (newCardOrder == null) return;
        var entryLookup = _cardEntries.ToDictionary(entry => entry.CardID);
        _cardEntries = newCardOrder
            .Where(card => entryLookup.ContainsKey(card.CardID))
            .Select(card => entryLookup[card.CardID])
            .ToList();
        RepositionCards();
    }

    private void UpdateInteract(bool canSelect)
    {
        foreach (CardEntry entry in _cardEntries)
        {
            bool isInteractable = canSelect || entry.CardRuntime.IsSelected;
            entry.SetInteractable(isInteractable);
        }
    }

    private void RepositionCards()
    {
        if (_cardEntries.Count == 0) return;

        float totalWidth = 1000f;
        float spacing = _cardEntries.Count > 1 ? totalWidth / (_cardEntries.Count - 1) : 0f;
        float startX = -totalWidth / 2f;

        for (int i = 0; i < _cardEntries.Count; i++)
        {
            _cardEntries[i].transform.SetSiblingIndex(i);
            _cardEntries[i].transform.DOLocalMoveX(startX + i * spacing, 0.3f).SetEase(Ease.OutQuad);
        }
    }
}