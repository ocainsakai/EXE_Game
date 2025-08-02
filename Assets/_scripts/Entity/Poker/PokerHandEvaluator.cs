using System.Collections.Generic;

public class PokerHandEvaluator
{
    private readonly List<PokerTypeBase> _pokerTypes;

    public PokerHandEvaluator()
    {
        _pokerTypes = new List<PokerTypeBase>
        {
            new RoyalFlushType(),
            new StraightFlushType(),
            new FourOfAKindType(),
            new FullHouseType(),
            new FlushType(),
            new StraightType(),
            new ThreeOfAKindType(),
            new TwoPairType(),
            new OnePairType(),
            new HighCardType()
        };
    }

    public PokerHandType EvaluateHand(int[] ranks, int[] suits)
    {
        foreach (var pokerType in _pokerTypes)
        {
            if (pokerType.IsMatch(ranks, suits))
            {
                return pokerType.Type;
            }
        }
        return PokerHandType.HighCard;
    }
}