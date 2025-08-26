using System;
using System.Linq;

public class RoyalFlushType : PokerTypeBase
{
    public override PokerHandType Type => PokerHandType.RoyalFlush;

    public override bool IsMatch(int[] ranks, int[] suits)
    {
        Array.Sort(ranks);
        return HasFlush(suits) && ranks.SequenceEqual(new[] { 10, 11, 12, 13, 14 });
    }
}