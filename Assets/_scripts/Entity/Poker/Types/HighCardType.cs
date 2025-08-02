

public class HighCardType : PokerTypeBase
{
    public override PokerHandType Type => PokerHandType.HighCard;

    public override bool IsMatch(int[] ranks, int[] suits)
    {
        return true;
    }
}
