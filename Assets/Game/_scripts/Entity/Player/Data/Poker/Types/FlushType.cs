public class FlushType : PokerTypeBase
{
    public override PokerHandType Type => PokerHandType.Flush;

    public override bool IsMatch(int[] ranks, int[] suits)
    {
        return HasFlush(suits) && !HasStraight(ranks);
    }
}
