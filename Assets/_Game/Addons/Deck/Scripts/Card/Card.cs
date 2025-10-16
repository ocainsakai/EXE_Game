using CardSystem;
using Google.Impl;
using System;
using System.Collections;
using UnityEngine;

public class Card
{
    public static Func<Card, IEnumerator> OnActive;

    //public static bool CanSelect;
    private CardData _cardData;
    public CardData CardData;
    public static bool CanSelect;
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
    public Action RequestSelectedChanged;

    private bool isSelecting;
    public bool IsSelected
    {
        get => isSelecting;
        set
        {
            if (value == isSelecting) return;
            if (!CanSelect && !isSelecting) return;
            isSelecting = value;
            SelectedChanged?.Invoke();
        }
    }


    public Card(CardData data)
    {
        _cardData = data;
        isSelecting = false;
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

    public IEnumerator Active()
    {
        if (OnActive != null)
        {
            // Lấy toàn bộ delegate trong event (nếu có nhiều listener)
            foreach (var d in OnActive.GetInvocationList())
            {
                var func = (Func<Card, IEnumerator>)d;
                yield return func(this); // chạy lần lượt từng listener
            }
        }

        Debug.Log("playing..."+ Name);
        yield return new WaitForSeconds(1);
    }

    public void ChangedState()
    {
        Debug.Log($"{this} + ChangedState ");

        RequestSelectedChanged?.Invoke();
        IsSelected = !IsSelected;
    }
}
