using UnityEngine;

[CreateAssetMenu(fileName = "NewSDCardData", menuName = "Scriptable Objects/SD Card", order = 2)]
public class CardSDData : CardData
{
    public int Suit;
    public int Rank;
    public Sprite Art;

    public string GetSuit()
    {
        switch (Suit)
        {
            case 0: return "Hearts";
            case 1: return "Diamonds";
            case 2: return "Clubs";
            case 3: return "Spades";
            default: return "Unknown Suit";
        }
    }
    public string GetRank()
    {
        switch (Rank)
        {
            case 2: return "Two";
            case 3: return "Three";
            case 4: return "Four";
            case 5: return "Five";
            case 6: return "Six";
            case 7: return "Seven";
            case 8: return "Eight";
            case 9: return "Nine";
            case 10: return "Ten";
            case 11: return "Jack";
            case 12: return "Queen";
            case 13: return "King";
            case 14: return "Ace";
            default: return "Unknown Rank";
        }
    }
}