using Ain;
using System;
using TMPro;
using UniRx;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pokerType;
    [SerializeField] private TextMeshProUGUI pokerMult;
    [SerializeField] private PokerData HUDData;


    private void Awake()
    {
        HUDData.Reset();
        HUDData.PokerType.Subscribe(UpdatePokerType).AddTo(this);
        HUDData.PokerMult.Subscribe(UpdatePokerMult).AddTo(this);

    }


    private void UpdatePokerMult(int obj)
    {
        pokerMult.text = obj.ToString();
    }

    private void UpdatePokerType(PokerHandType type)
    {
        pokerType.text = type switch
        {
            PokerHandType.None => "Poker Type",
            PokerHandType.HighCard => "High Card",
            PokerHandType.OnePair => "One Pair",
            PokerHandType.TwoPair => "Two Pair",
            PokerHandType.ThreeOfAKind => "Three of a Kind",
            PokerHandType.Straight => "Straight",
            PokerHandType.Flush => "Flush",
            PokerHandType.FullHouse => "Full House",
            PokerHandType.FourOfAKind => "Four of a Kind",
            PokerHandType.StraightFlush => "Straight Flush",
            PokerHandType.RoyalFlush => "Royal Flush",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
