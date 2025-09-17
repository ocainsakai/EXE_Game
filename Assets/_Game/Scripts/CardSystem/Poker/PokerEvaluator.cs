
using System.Collections.Generic;
using System.Linq;

namespace CardSystem.PokerSystem
{
    public static class PokerEvaluator
    {
        private static int GetRankMask(IEnumerable<CardMask> cards)
        {
            int mask = 0;
            foreach (var c in cards)
                mask |= 1 << c.Rank - 2; // bit 0 = rank 2, bit 12 = Ace
            return mask;
        }

        private static Dictionary<CardRank, int> CountByRank(IEnumerable<CardMask> cards)
        {
            return cards.GroupBy(c => c.ERank)
                        .ToDictionary(g =>  g.Key, g => g.Count());
        }

        private static Dictionary<CardSuit, List<CardMask>> GroupBySuit(IEnumerable<CardMask> cards)
        {
            return cards.GroupBy(c => c.ESuit)
                        .ToDictionary(g => g.Key, g => g.ToList());
        }

        private static List<CardMask> GetFlush(IEnumerable<CardMask> cards)
        {
            var suits = GroupBySuit(cards);
            foreach (var s in suits)
                if (s.Value.Count >= 5)
                    return s.Value.OrderByDescending(c => c.Rank).Take(5).ToList();
            return null;
        }

        // Check Straight
        private static List<CardMask> GetStraight(IEnumerable<CardMask> cards)
        {
            int mask = GetRankMask(cards);

            // Wheel straight (A-2-3-4-5)
            if ((mask & 0b1000000001111) == 0b1000000001111)
            {
                return cards.Where(c => c.ERank == CardRank.Ace || c.Rank >= 2 && c.Rank <= 5)
                            .OrderByDescending(c => c.ERank == CardRank.Ace ? 1 : c.Rank)
                            .Take(5).ToList();
            }

            for (int i = 12; i >= 4; i--)
            {
                int pattern = 0b11111 << i - 4;
                if ((mask & pattern) == pattern)
                {
                    int highRank = i + 2; // rank cao nh?t
                    return cards.Where(c => c.Rank <= highRank && c.Rank > highRank - 5)
                                .OrderByDescending(c => c.Rank)
                                .Take(5).ToList();
                }
            }
            return null;
        }

        private static List<CardMask> GetStraightFlush(IEnumerable<CardMask> cards)
        {
            var suits = GroupBySuit(cards);
            foreach (var s in suits)
            {
                if (s.Value.Count >= 5)
                {
                    var straight = GetStraight(s.Value);
                    if (straight != null) return straight;
                }
            }
            return null;
        }

        // Ðánh giá hand
        public static PokerHandResult Evaluate(IEnumerable<CardMask> cards)
        {
            if (cards == null || cards.Count() == 0)
            {
                return new PokerHandResult
                {
                    HandType = PokerHandType.None,
                    BestCards = null
                };
            }
            var hand = cards.ToList();
            var counts = CountByRank(hand);

            // Check Straight Flush
            var straightFlush = GetStraightFlush(hand);
            if (straightFlush != null)
            {
                if (straightFlush.Max(c => c.ERank) == CardRank.Ace)
                    return new PokerHandResult { HandType = PokerHandType.RoyalFlush, BestCards = straightFlush };
                return new PokerHandResult { HandType = PokerHandType.StraightFlush, BestCards = straightFlush };
            }

            // Four of a Kind
            if (counts.Any(c => c.Value == 4))
            {
                var four = counts.First(c => c.Value == 4).Key;
                var best = hand.Where(c => c.ERank == four).ToList();
                best.Add(hand.Where(c => c.ERank != four).OrderByDescending(c => c.Rank).First());
                return new PokerHandResult { HandType = PokerHandType.FourOfAKind, BestCards = best };
            }

            // Full House
            if (counts.Any(c => c.Value == 3) && counts.Any(c => c.Value >= 2 && c.Key != counts.First(x => x.Value == 3).Key))
            {
                var three = counts.Where(c => c.Value == 3).OrderByDescending(c => c.Key).First().Key;
                var pair = counts.Where(c => c.Value >= 2 && c.Key != three).OrderByDescending(c => c.Key).First().Key;

                var best = hand.Where(c => c.ERank == three).Take(3).ToList();
                best.AddRange(hand.Where(c => c.ERank == pair).Take(2));
                return new PokerHandResult { HandType = PokerHandType.FullHouse, BestCards = best };
            }

            // Flush
            var flush = GetFlush(hand);
            if (flush != null)
                return new PokerHandResult { HandType = PokerHandType.Flush, BestCards = flush };

            // Straight
            var straight = GetStraight(hand);
            if (straight != null)
                return new PokerHandResult { HandType = PokerHandType.Straight, BestCards = straight };

            // Three of a Kind
            if (counts.Any(c => c.Value == 3))
            {
                var three = counts.Where(c => c.Value == 3).OrderByDescending(c => c.Key).First().Key;
                var best = hand.Where(c => c.ERank == three).Take(3).ToList();
                best.AddRange(hand.Where(c => c.ERank != three).OrderByDescending(c => c.Rank).Take(2));
                return new PokerHandResult { HandType = PokerHandType.ThreeOfAKind, BestCards = best };
            }

            // Two Pair
            var pairs = counts.Where(c => c.Value == 2).OrderByDescending(c => c.Key).ToList();
            if (pairs.Count >= 2)
            {
                var best = hand.Where(c => c.ERank == pairs[0].Key).Take(2).ToList();
                best.AddRange(hand.Where(c => c.ERank == pairs[1].Key).Take(2));
                best.Add(hand.Where(c => c.ERank != pairs[0].Key && c.ERank != pairs[1].Key)
                             .OrderByDescending(c => c.Rank).First());
                return new PokerHandResult { HandType = PokerHandType.TwoPair, BestCards = best };
            }

            // One Pair
            if (pairs.Count == 1)
            {
                var pair = pairs[0].Key;
                var best = hand.Where(c => c.ERank == pair).Take(2).ToList();
                best.AddRange(hand.Where(c => c.ERank != pair).OrderByDescending(c => c.Rank).Take(3));
                return new PokerHandResult { HandType = PokerHandType.OnePair, BestCards = best };
            }

            // High Card
            return new PokerHandResult
            {
                HandType = PokerHandType.HighCard,
                BestCards = hand.OrderByDescending(c => c.Rank).Take(5).ToList()
            };
        }
    }

}