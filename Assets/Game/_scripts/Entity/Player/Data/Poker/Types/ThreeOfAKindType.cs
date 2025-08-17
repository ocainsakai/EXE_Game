public class ThreeOfAKindType : PokerTypeBase
{
    public override PokerHandType Type => PokerHandType.ThreeOfAKind;

    public override bool IsMatch(int[] ranks, int[] suits)
    {
        return HasThreeOfAKind(ranks) && !HasFullHouse(ranks);
    }
}
