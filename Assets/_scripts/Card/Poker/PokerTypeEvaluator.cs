using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokerTypeEvaluator : MonoBehaviour
{
    public PokerTypeDatabase database;

    public (PokerTypeData data, List<Card> cards) Evaluate(IEnumerable<Card> cards)
    {
        var result = GetBestHand(cards);
        var matchedCards = cards.Where(x => result.MatchedCards.Contains(x.Data)).ToList();
        //PokerType pokerType = GetBestHand(cards).;
        return (database.GetData(result.HandType), matchedCards);
    }
    public PokerHandMatch GetBestHand(IEnumerable<Card> cards)
    {
        if (cards == null || cards.Count() == 0)
        {
            return PokerHandMatch.None;
        }
        var allPossible = GetAllPossibleHands(cards);
        if (allPossible.Any())
        {
            return allPossible.OrderByDescending(x => x.HandType).First();
        }
        var max = new List<CardData>() { cards.Max().Data };
        return new PokerHandMatch(PokerType.HighCard, max);
    }
    public IEnumerable<PokerHandMatch> GetAllPossibleHands(IEnumerable<Card> cards)
    {
        int count = cards.Count();
        HashSet<PokerHandMatch> foundHands = new HashSet<PokerHandMatch>();
        var cardDatas = cards.Select(c => c.Data).ToList();

        if (count >= 5)
        {

            foundHands.Add(HasFlushFive(cardDatas));
            foundHands.Add(HasFlushHouse(cardDatas));
            foundHands.Add(HasFiveOfAKind(cardDatas));
            foundHands.Add(HasRoyalFlush(cardDatas));
            foundHands.Add(HasStraightFlush(cardDatas));
            foundHands.Add(HasFullHouse(cardDatas));
            foundHands.Add(HasFlush(cardDatas));
            foundHands.Add(HasStraight(cardDatas));
        }
        if (count >= 4)
        {
            foundHands.Add(HasFourOfAKind(cardDatas));
            foundHands.Add(HasTwoPair(cardDatas));

        }
        if (count >= 3)
        {

            foundHands.Add(HasThreeOfAKind(cardDatas));
        }
        if (count >= 2)
        {

            foundHands.Add(HasOnlyPair(cardDatas));
        }
        var max = new List<CardData>() { cards.OrderByDescending(x => x.Data.RankValue).First().Data };
        var high = new PokerHandMatch(PokerType.HighCard, max);
        foundHands.Add(high);
        return foundHands;
    }
    private List<IGrouping<CardRank, CardData>> GetRankGroups(IEnumerable<CardData> cards)
    {
        return cards.GroupBy(c => c.Rank).OrderByDescending(g => g.Count()).ToList();
    }

    // Utility function to get CardCardSuit groups, useful for flush checks
    private List<IGrouping<CardSuit, CardData>> GetCardSuitGroups(IEnumerable<CardData> cards)
    {
        return cards.GroupBy(c => c.Suit).ToList();
    }

    public  PokerHandMatch HasPair(IEnumerable<CardData> cards)
    {
        var groups = GetRankGroups(cards);
        var pairGroup = groups.FirstOrDefault(g => g.Count() == 2);
        if (pairGroup != null)
        {
            return new PokerHandMatch(PokerType.Pair, pairGroup.ToList());
        }
        return PokerHandMatch.None;
    }

    public  PokerHandMatch HasOnlyPair( IEnumerable<CardData> cards)
    {
        var groups = GetRankGroups(cards);
        var pairs = groups.Where(g => g.Count() == 2).ToList();
        if (pairs.Count == 1)
        {
            return new PokerHandMatch(PokerType.Pair, pairs.First().ToList());
        }
        return PokerHandMatch.None;
    }


    public  PokerHandMatch HasTwoPair( IEnumerable<CardData> cards)
    {
        var groups = GetRankGroups(cards);
        var pairs = groups.Where(g => g.Count() == 2).OrderByDescending(g => g.Key).Take(2).ToList();
        if (pairs.Count == 2)
        {
            return new PokerHandMatch(PokerType.TwoPair, pairs.SelectMany(g => g).ToList());
        }
        return PokerHandMatch.None;
    }


    public  PokerHandMatch HasThreeOfAKind( IEnumerable<CardData> cards)
    {
        var groups = GetRankGroups(cards);
        var threeOfAKindGroup = groups.FirstOrDefault(g => g.Count() == 3);
        if (threeOfAKindGroup != null)
        {
            return new PokerHandMatch(PokerType.ThreeOfAKind, threeOfAKindGroup.ToList());
        }
        return PokerHandMatch.None;
    }


    public  PokerHandMatch HasStraight( IEnumerable<CardData> cards)
    {
        var distinctRanks = cards.Select(c => (int)c.Rank).Distinct().OrderBy(r => r).ToList();

        if (distinctRanks.Contains((int)CardRank.Ace))
        {
            distinctRanks.Insert(0, 1);
        }

        for (int i = 0; i <= distinctRanks.Count - 5; i++)
        {
            var potentialStraightRanks = distinctRanks.Skip(i).Take(5).ToList();
            bool isConsecutive = true;
            for (int j = 0; j < 4; j++)
            {
                if (potentialStraightRanks[j] + 1 != potentialStraightRanks[j + 1])
                {
                    isConsecutive = false;
                    break;
                }
            }

            if (isConsecutive)
            {

                var matchedCards = cards.Where(c => potentialStraightRanks.Contains((int)c.Rank)).ToList();

                return new PokerHandMatch(PokerType.Straight, matchedCards.Take(5).ToList());
            }
        }
        return PokerHandMatch.None;
    }


    public  PokerHandMatch HasFlush( IEnumerable<CardData> cards)
    {
        var CardSuitGroups = GetCardSuitGroups(cards);
        var flushGroup = CardSuitGroups.FirstOrDefault(g => g.Count() >= 5);
        if (flushGroup != null)
        {
            return new PokerHandMatch(PokerType.Flush, flushGroup.ToList());
        }
        return PokerHandMatch.None;
    }


    public  PokerHandMatch HasFullHouse( IEnumerable<CardData> cards)
    {
        var groups = GetRankGroups(cards);
        var threeOfAKindGroup = groups.FirstOrDefault(g => g.Count() == 3);
        var pairGroup = groups.FirstOrDefault(g => g.Count() == 2);

        if (threeOfAKindGroup != null && pairGroup != null)
        {
            var matchedCards = threeOfAKindGroup.ToList();
            matchedCards.AddRange(pairGroup.ToList());
            return new PokerHandMatch(PokerType.FullHouse, matchedCards);
        }
        return PokerHandMatch.None;
    }


    public  PokerHandMatch HasFourOfAKind( IEnumerable<CardData> cards)
    {
        var groups = GetRankGroups(cards);
        var fourOfAKindGroup = groups.FirstOrDefault(g => g.Count() == 4);
        if (fourOfAKindGroup != null)
        {
            return new PokerHandMatch(PokerType.FourOfAKind, fourOfAKindGroup.ToList());
        }
        return PokerHandMatch.None;
    }


    public  PokerHandMatch HasFiveOfAKind( IEnumerable<CardData> cards)
    {
        var groups = GetRankGroups(cards);
        var fiveOfAKindGroup = groups.FirstOrDefault(g => g.Count() == 5);
        if (fiveOfAKindGroup != null)
        {
            return new PokerHandMatch(PokerType.FiveOfAKind, fiveOfAKindGroup.ToList());
        }
        return PokerHandMatch.None;
    }


    public  PokerHandMatch HasStraightFlush( IEnumerable<CardData> cards)
    {
        var straightMatch = HasStraight(cards);
        var flushMatch = HasFlush(cards);
        if (straightMatch != PokerHandMatch.None && flushMatch != PokerHandMatch.None)
        {
            var list = straightMatch.MatchedCards.Intersect(flushMatch.MatchedCards);
            return new PokerHandMatch(PokerType.StraightFlush, list);
        }
        return PokerHandMatch.None;
    }

    public  PokerHandMatch HasRoyalFlush( IEnumerable<CardData> cards)
    {
        var CardSuitGroups = GetCardSuitGroups(cards);
        foreach (var CardSuitGroup in CardSuitGroups.Where(g => g.Count() >= 5))
        {
            var ranksInGroup = CardSuitGroup.Select(c => (int)c.Rank).Distinct().ToList();
            if (ranksInGroup.Contains((int)CardRank.Ten) &&
                ranksInGroup.Contains((int)CardRank.Jack) &&
                ranksInGroup.Contains((int)CardRank.Queen) &&
                ranksInGroup.Contains((int)CardRank.King) &&
                ranksInGroup.Contains((int)CardRank.Ace))
            {
                var royalFlushCards = CardSuitGroup.Where(c =>
                    c.Rank == CardRank.Ten || c.Rank == CardRank.Jack || c.Rank == CardRank.Queen || c.Rank == CardRank.King || c.Rank == CardRank.Ace
                ).ToList();
                return new PokerHandMatch(PokerType.StraightFlush, royalFlushCards);
            }
        }
        return PokerHandMatch.None;
    }



    public  PokerHandMatch HasFlushHouse( IEnumerable<CardData> cards)
    {
        var fullHouseMatch = HasFullHouse(cards);
        var flushMatch = HasFlush(cards);
        if (fullHouseMatch != PokerHandMatch.None && flushMatch != PokerHandMatch.None)
        {
            var matchedCards = fullHouseMatch.MatchedCards.Intersect(flushMatch.MatchedCards);
            return new PokerHandMatch(PokerType.FlushFive, matchedCards);
        }
        return PokerHandMatch.None;
    }


    public PokerHandMatch HasFlushFive( IEnumerable<CardData> cards)
    {
        var fiveOfAKindMatch = HasFiveOfAKind(cards);
        var flushMatch = HasFlush(cards);
        if (fiveOfAKindMatch != PokerHandMatch.None && flushMatch != PokerHandMatch.None)
        {
            var matchedCards = fiveOfAKindMatch.MatchedCards.Intersect(flushMatch.MatchedCards);
            return new PokerHandMatch(PokerType.FlushFive, matchedCards);
        }
        return PokerHandMatch.None;
    }
}


public class PokerHandMatch
{
    public PokerType HandType { get; }
    public List<CardData> MatchedCards { get; }

    public PokerHandMatch(PokerType type, IEnumerable<CardData> cards)
    {
        HandType = type;
        MatchedCards = cards?.ToList() ?? null;
    }
    public static PokerHandMatch None = new PokerHandMatch(PokerType.PokerType, null);
    public override string ToString()
    {
        return $"{HandType}: [{string.Join(", ", MatchedCards.Select(c => c.ToString()))}]";
    }
}

