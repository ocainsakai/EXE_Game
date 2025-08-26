public class StraightType : PokerTypeBase
{
    public override PokerHandType Type => PokerHandType.Straight;

    public override bool IsMatch(int[] ranks, int[] suits)
    {
        return HasStraight(ranks) && !HasFlush(suits);
    }
}
