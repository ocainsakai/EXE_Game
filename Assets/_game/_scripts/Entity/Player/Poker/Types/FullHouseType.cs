public class FullHouseType : PokerTypeBase
{
    public override PokerHandType Type => PokerHandType.FullHouse;

    public override bool IsMatch(int[] ranks, int[] suits)
    {
        return HasFullHouse(ranks);
    }
}
