public class OnePairType : PokerTypeBase
{
    public override PokerHandType Type => PokerHandType.OnePair;

    public override bool IsMatch(int[] ranks, int[] suits)
    {
        return HasPair(ranks) && !HasTwoPair(ranks);
    }
}
