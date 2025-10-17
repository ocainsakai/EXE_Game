using System.Collections.Generic;
using System.Linq;
using _Game.Addons.Deck.Scripts;
using _Game.Addons.Deck.Scripts.Card;

public static class CardCollectionExtensions
{
    public static int GetAceCount(this IEnumerable<CardRuntime> collection)
    {
        return collection.Count(c => c.Rank == CardSystem.CardRank.Ace);
    }

    // Đếm theo rank cụ thể
    public static int GetRankCount(this IEnumerable<CardRuntime> collection, CardSystem.CardRank rank)
    {
        return collection.Count(c => c.Rank == rank);
    }

    // Đếm theo suit cụ thể
    public static int GetSuitCount(this IEnumerable<CardRuntime> collection, CardSystem.CardSuit suit)
    {
        return collection.Count(c => c.Suit == suit);
    }

    // Đếm toàn bộ rank → dictionary (Rank -> count)
    public static Dictionary<CardSystem.CardRank, int> GetAllRankCounts(this IEnumerable<CardRuntime> collection)
    {
        return collection
            .GroupBy(c => c.Rank)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    // Đếm toàn bộ suit → dictionary (Suit -> count)
    public static Dictionary<CardSystem.CardSuit, int> GetAllSuitCounts(this IEnumerable<CardRuntime> collection)
    {
        return collection
            .GroupBy(c => c.Suit)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    // Đếm Face cards (J, Q, K)
    public static int GetFaceCardCount(this IEnumerable<CardRuntime> collection)
    {
        return collection.Count(c =>
            c.Rank == CardSystem.CardRank.Jack ||
            c.Rank == CardSystem.CardRank.Queen ||
            c.Rank == CardSystem.CardRank.King);
    }

    // Đếm Number cards (2 -> 10)
    public static int GetNumberCardCount(this IEnumerable<CardRuntime> collection)
    {
        return collection.Count(c =>
            c.Rank >= CardSystem.CardRank.Two &&
            c.Rank <= CardSystem.CardRank.Ten);
    }
}
