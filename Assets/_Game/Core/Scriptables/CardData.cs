
namespace CardSystem
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    [CreateAssetMenu(fileName = "CardData", menuName = "Cards/New Card")]
    public class CardData : ScriptableObject
    {
        public SerializableGuid CardID = SerializableGuid.NewGuid();

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
        public static void SortByRank(this List<CardData> list)
        {
            list.Sort((a, b) =>
            {
                int suitCompare = a.Rank.CompareTo(b.Rank);
                if (suitCompare != 0) return suitCompare;
                return a.Suit.CompareTo(b.Suit);
            });
        }
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
    }

}