public class TwoPairType : PokerTypeBase
{
    public override PokerHandType Type => PokerHandType.TwoPair;

    public override bool IsMatch(int[] ranks, int[] suits)
    {
        return HasTwoPair(ranks);
    }
}
