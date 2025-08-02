using System;
using System.Collections.Generic;
using System.Linq;

public abstract partial class PokerTypeBase
{
    public abstract PokerHandType Type { get; }
    public abstract bool IsMatch(int[] ranks, int[] suits);

    protected bool ValidateInput(int[] ranks, int[] suits = null)
    {
        if (ranks == null || ranks.Length < 5)
            return false;

        if (suits != null && suits.Length != ranks.Length)
            return false;

        return true;
    }

    protected bool HasFlush(int[] suits)
    {
        if (!ValidateInput(suits)) return false;
        return suits.Distinct().Count() == 1;
    }

    protected bool HasStraight(int[] ranks)
    {
        if (!ValidateInput(ranks)) return false;

        Array.Sort(ranks);

        // Wheel (A-2-3-4-5)
        if (ranks.SequenceEqual(new[] { 2, 3, 4, 5, 14 }))
            return true;

        for (int i = 1; i < ranks.Length; i++)
        {
            if (ranks[i] != ranks[i - 1] + 1)
                return false;
        }

        return true;
    }

    protected Dictionary<int, int> CountRanks(int[] ranks)
    {
        if (!ValidateInput(ranks)) return new Dictionary<int, int>();
        return ranks.GroupBy(r => r)
                   .ToDictionary(g => g.Key, g => g.Count());
    }

    protected bool HasPair(int[] ranks)
    {
        var rankCounts = CountRanks(ranks);
        return rankCounts.Any(kvp => kvp.Value == 2);
    }

    protected bool HasTwoPair(int[] ranks)
    {
        var rankCounts = CountRanks(ranks);
        return rankCounts.Count(kvp => kvp.Value == 2) >= 2;
    }

    protected bool HasThreeOfAKind(int[] ranks)
    {
        var rankCounts = CountRanks(ranks);
        return rankCounts.Any(kvp => kvp.Value == 3);
    }

    protected bool HasFourOfAKind(int[] ranks)
    {
        var rankCounts = CountRanks(ranks);
        return rankCounts.Any(kvp => kvp.Value == 4);
    }

    protected bool HasFullHouse(int[] ranks)
    {
        var rankCounts = CountRanks(ranks);
        return rankCounts.ContainsValue(3) && rankCounts.ContainsValue(2);
    }
}