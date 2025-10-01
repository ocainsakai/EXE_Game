using CardSystem;
using System;
using UnityEngine;

public class Card
{
    public static bool CanSelect;
    private CardData _cardData;
    public int CardDataID => _cardData.CardID;
    public SerializableGuid CardID;

    [Header("Card Identity")]
    public CardRank Rank => _cardData.Rank;
    public CardSuit Suit => _cardData.Suit;

    [Header("Card Info")]
    public int Cost => _cardData.Cost;
    public string Name => _cardData.Name;
    [TextArea] public string Description;
    public Sprite Art => _cardData.Art;

    public CardMask Mask => new CardMask(Rank, Suit);

    public Action SelectedChanged;

    private bool isSelecting;
    public bool IsSeleced
    {
        get => isSelecting;
        set
        {
            if (value != isSelecting)
            {
                isSelecting = value;
                SelectedChanged?.Invoke();
            }
        }
    }

    public Card(CardData data)
    {
        _cardData = data;
        CardID = SerializableGuid.NewGuid();
    }

    public override bool Equals(object obj)
    {
        return obj is Card other && other.CardID == CardID;
    }

    public override int GetHashCode()
    {
        return CardID.GetHashCode();
    }

}
