public class FourOfAKindType : PokerTypeBase
{
    public override PokerHandType Type => PokerHandType.FourOfAKind;

    public override bool IsMatch(int[] ranks, int[] suits)
    {
        return HasFourOfAKind(ranks);
    }
}
