using CardSystem;
using CardSystem.PokerSystem;
using System.Collections.Generic;
using System.Linq;
using _Game.Addons.Deck.Scripts;
using _Game.Addons.Deck.Scripts.Card;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// Quản lý việc chọn bài trong hand và preview poker hand
/// </summary>
public class UIHandSelector : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int maxSelectedCards = 5;
    [SerializeField] private float selectedYOffset = 20f;
    [SerializeField] private Color selectedColor = Color.yellow;
    [SerializeField] private Color normalColor = Color.white;

    [Header("Preview")]
    [SerializeField] private TextMeshProUGUI handTypePreviewText;
    [SerializeField] private TextMeshProUGUI damagePreviewText;
    [SerializeField] private MultTable multTable;

    [FormerlySerializedAs("OnSelectionChanged")] [Header("Events")]
    public UnityEvent onSelectionChanged;

    // State
    private Dictionary<CardRuntime, CardEntry> _cardToEntry = new Dictionary<CardRuntime, CardEntry>();
    private HashSet<CardRuntime> _selectedCards = new HashSet<CardRuntime>();

    public IReadOnlyList<CardRuntime> SelectedCards => _selectedCards.ToList();
    public int SelectedCount => _selectedCards.Count;

    private void Start()
    {
        UpdatePreview();
    }

    // ==================== CARD REGISTRATION ====================

    /// <summary>
    /// Register card entry để có thể select
    /// </summary>
    public void RegisterCardEntry(CardEntry cardEntry)
    {
        if (cardEntry == null || cardEntry.CardRuntime == null)
            return;

        if (_cardToEntry.ContainsKey(cardEntry.CardRuntime))
        {
            Debug.LogWarning($"Card {cardEntry.CardRuntime.Mask} already registered!");
            return;
        }

        _cardToEntry[cardEntry.CardRuntime] = cardEntry;

        // Subscribe to click event
        cardEntry.OnCardClicked += () => OnCardClicked(cardEntry);
    }

    /// <summary>
    /// Unregister card entry
    /// </summary>
    public void UnregisterCardEntry(CardEntry cardEntry)
    {
        if (cardEntry == null || cardEntry.CardRuntime == null)
            return;

        _cardToEntry.Remove(cardEntry.CardRuntime);
        _selectedCards.Remove(cardEntry.CardRuntime);

        UpdatePreview();
    }

    /// <summary>
    /// Clear all registrations (khi reset hand)
    /// </summary>
    public void ClearAllRegistrations()
    {
        _cardToEntry.Clear();
        _selectedCards.Clear();
        UpdatePreview();
    }

    // ==================== SELECTION ====================

    /// <summary>
    /// Handle card click
    /// </summary>
    private void OnCardClicked(CardEntry cardEntry)
    {
        if (cardEntry == null || cardEntry.CardRuntime == null)
            return;

        CardRuntime cardRuntime = cardEntry.CardRuntime;

        if (_selectedCards.Contains(cardRuntime))
        {
            DeselectCard(cardRuntime);
        }
        else
        {
            SelectCard(cardRuntime);
        }
    }

    private void SelectCard(CardRuntime cardRuntime)
    {
        // Check limit
        if (_selectedCards.Count >= maxSelectedCards)
        {
            Debug.Log($"Maximum {maxSelectedCards} cards can be selected!");
            return;
        }

        _selectedCards.Add(cardRuntime);

        if (_cardToEntry.TryGetValue(cardRuntime, out var entry))
        {
            UpdateCardVisual(entry, true);
        }

        UpdatePreview();
        onSelectionChanged?.Invoke();
    }

    private void DeselectCard(CardRuntime cardRuntime)
    {
        _selectedCards.Remove(cardRuntime);

        if (_cardToEntry.TryGetValue(cardRuntime, out var entry))
        {
            UpdateCardVisual(entry, false);
        }

        UpdatePreview();
        onSelectionChanged?.Invoke();
    }

    /// <summary>
    /// Clear all selected cards
    /// </summary>
    public void ClearSelection()
    {
        var cardsToDeselect = new List<CardRuntime>(_selectedCards);

        foreach (var card in cardsToDeselect)
        {
            if (_cardToEntry.TryGetValue(card, out var entry))
            {
                UpdateCardVisual(entry, false);
            }
        }

        _selectedCards.Clear();
        UpdatePreview();
        onSelectionChanged?.Invoke();
    }

    /// <summary>
    /// Select specific cards (programmatically)
    /// </summary>
    public void SelectCards(List<CardRuntime> cards)
    {
        ClearSelection();

        foreach (var card in cards)
        {
            if (_cardToEntry.ContainsKey(card))
            {
                SelectCard(card);
            }
        }
    }

    // ==================== VISUAL UPDATE ====================

    /// <summary>
    /// Update visual state của card khi select/deselect
    /// </summary>
    private void UpdateCardVisual(CardEntry cardEntry, bool isSelected)
    {
        // Move card up/down
        RectTransform rect = cardEntry.GetComponent<RectTransform>();
        if (rect != null)
        {
            Vector3 pos = rect.anchoredPosition;
            pos.y = isSelected ? selectedYOffset : 0;
            rect.anchoredPosition = pos;
        }

        // Change color or outline
        var canvasGroup = cardEntry.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = isSelected ? 1f : 0.8f;
        }

        // TODO: Add outline, glow effect, etc.
    }

    // ==================== PREVIEW ====================

    /// <summary>
    /// Update poker hand preview
    /// </summary>
    private void UpdatePreview()
    {
        if (_selectedCards.Count == 0)
        {
            if (handTypePreviewText != null)
                handTypePreviewText.text = "Select cards to play";

            if (damagePreviewText != null)
                damagePreviewText.text = "";

            return;
        }

        // Evaluate hand
        var cardMasks = _selectedCards.Select(c => c.Mask).ToList();
        var handResult = PokerEvaluator.Evaluate(cardMasks);

        // Calculate potential damage
        int baseDamage = CalculateBaseDamage(_selectedCards.ToList());
        float multiplier = multTable != null ? multTable.GetMult(handResult.HandType) : 1f;
        int totalDamage = Mathf.RoundToInt(baseDamage * multiplier);

        // Update UI
        if (handTypePreviewText != null)
        {
            string handName = GetHandDisplayName(handResult.HandType);
            handTypePreviewText.text = $"<color=yellow>{handName}</color>";
        }

        if (damagePreviewText != null)
        {
            damagePreviewText.text = $"Damage: <color=red>{totalDamage}</color> ({baseDamage} × {multiplier}x)";
        }
    }

    private int CalculateBaseDamage(List<CardRuntime> cards)
    {
        int total = 0;
        foreach (var card in cards)
        {
            total += GetCardChipValue(card);
        }
        return total;
    }

    private int GetCardChipValue(CardRuntime cardRuntime)
    {
        switch (cardRuntime.Mask.ERank)
        {
            case CardRank.Ace:
                return 11;
            case CardRank.King:
            case CardRank.Queen:
            case CardRank.Jack:
                return 10;
            default:
                return (int)cardRuntime.Mask.ERank;
        }
    }

    private string GetHandDisplayName(PokerHandType handType)
    {
        return handType switch
        {
            PokerHandType.RoyalFlush => "Royal Flush",
            PokerHandType.StraightFlush => "Straight Flush",
            PokerHandType.FourOfAKind => "Four of a Kind",
            PokerHandType.FullHouse => "Full House",
            PokerHandType.Flush => "Flush",
            PokerHandType.Straight => "Straight",
            PokerHandType.ThreeOfAKind => "Three of a Kind",
            PokerHandType.TwoPair => "Two Pair",
            PokerHandType.OnePair => "One Pair",
            PokerHandType.HighCard => "High Card",
            _ => "None"
        };
    }

    // ==================== QUERIES ====================

    public bool IsCardSelected(CardRuntime cardRuntime)
    {
        return _selectedCards.Contains(cardRuntime);
    }

    public bool CanSelectMore()
    {
        return _selectedCards.Count < maxSelectedCards;
    }

    public bool HasSelection()
    {
        return _selectedCards.Count > 0;
    }

    // ==================== DEBUG ====================

    [ContextMenu("Debug - Select All")]
    private void DebugSelectAll()
    {
        var allCards = _cardToEntry.Keys.Take(maxSelectedCards).ToList();
        SelectCards(allCards);
    }

    [ContextMenu("Debug - Clear Selection")]
    private void DebugClearSelection()
    {
        ClearSelection();
    }
}