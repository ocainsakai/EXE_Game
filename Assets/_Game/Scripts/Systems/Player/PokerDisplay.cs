
using CardSystem.PokerSystem;
using UnityEngine;

public class PokerDisplay : MonoBehaviour
{
    [SerializeField] private TextDisplay pokerType;
    [SerializeField] private TextDisplay pokerMult;

    public void UpdatePokerMult(int obj)
    {
        string content = "MULT: "+ obj.ToString();
        pokerMult.UpdateContent(content);
    }

    public void UpdatePokerType(PokerHandType type)
    {
        string content = type switch
        {
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
            _ => "POKER Type",
        };
        pokerType.UpdateContent(content.ToUpper());
    }
}
