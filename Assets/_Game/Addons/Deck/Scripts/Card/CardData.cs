using System.Collections.Generic;
using System.Linq;
using CardSystem;
using UnityEngine;

namespace _Game.Addons.Deck.Scripts
{
    [CreateAssetMenu(fileName = "CardData", menuName = "Cards/New Card")]
    public class CardData : ScriptableObject
    {
        public int CardID;

        [Header("Card Identity")]
        public CardRank Rank;
        public CardSuit Suit;

        [Header("Card Info")]
        public int Cost;
        public string Name;
        [TextArea] public string Description;
        public Sprite Art;

        public CardMask Mask => new CardMask(Rank, Suit);
    }

    public static class CardSorter {
        // Thứ tự rank A → K → Q → J → 10 → 9 → ... → 2
        private static readonly CardRank[] RankOrder = new[]
        {
            CardRank.Ace, CardRank.King, CardRank.Queen, CardRank.Jack,
            CardRank.Ten, CardRank.Nine, CardRank.Eight, CardRank.Seven,
            CardRank.Six, CardRank.Five, CardRank.Four, CardRank.Three, CardRank.Two
        };

        // Thứ tự suit: mỗi hàng 1 chất
        private static readonly CardSuit[] SuitOrder = new[]
        {
            CardSuit.Hearts,   // hàng 1
            CardSuit.Diamonds, // hàng 2
            CardSuit.Clubs,    // hàng 3
            CardSuit.Spades    // hàng 4
        };

        // Sắp xếp mặc định (theo Rank trước, rồi Suit)
        public static void SortByRank(this List<CardData> list)
        {
            list.Sort((a, b) =>
            {
                int rankCompare = a.Rank.CompareTo(b.Rank);
                if (rankCompare != 0) return rankCompare;
                return a.Suit.CompareTo(b.Suit);
            });
        }

        // Sắp xếp mặc định (theo Suit trước, rồi Rank)
        public static void SortBySuit(this List<CardData> list)
        {
            list.Sort((a, b) =>
            {
                int suitCompare = a.Suit.CompareTo(b.Suit);
                if (suitCompare != 0) return suitCompare;
                return a.Rank.CompareTo(b.Rank);
            });
        }

        public static IEnumerable<CardData> SortByRank(this IEnumerable<CardData> list)
        {
            return list.OrderBy(c => c.Rank).ThenBy(c => c.Suit);
        }

        public static IEnumerable<CardData> SortBySuit(this IEnumerable<CardData> list)
        {
            return list.OrderBy(c => c.Suit).ThenBy(c => c.Rank);
        }

        // ⭐ Dùng cho hiển thị kiểu Balatro: 4 hàng × 13 cột
        public static IEnumerable<CardData> SortForDisplay(this IEnumerable<CardData> list)
        {
            return list
                .OrderBy(c => System.Array.IndexOf(SuitOrder, c.Suit))
                .ThenBy(c => System.Array.IndexOf(RankOrder, c.Rank));
        }
    }
}