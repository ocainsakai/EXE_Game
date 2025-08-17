using System;
using System.Linq;

public class StraightFlushType : PokerTypeBase
{
    public override PokerHandType Type => PokerHandType.StraightFlush;

    public override bool IsMatch(int[] ranks, int[] suits)
    {
        return HasStraight(ranks) && HasFlush(suits) && !IsRoyalFlush(ranks);
    }

    private bool IsRoyalFlush(int[] ranks)
    {
        Array.Sort(ranks);
        return ranks.SequenceEqual(new[] { 10, 11, 12, 13, 14 });
    }
}
