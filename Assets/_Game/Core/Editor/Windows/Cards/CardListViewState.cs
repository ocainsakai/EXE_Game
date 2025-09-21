using UnityEngine;

namespace CardSystem
{
    [System.Serializable]
    public class CardListViewState
    {
        public Vector2 scrollPos;
        public CardViewMode cardViewMode = CardViewMode.OneLine;
        public SortBy sortBy = SortBy.None;
        public bool sortAscending = true;
        public int thumbSize = 64;
        public bool showArt = true;

        // Filter
        public string searchTerm = "";
        public bool useFilter = false;
        public CardRankFilter filterRank = CardRankFilter.All;
        public CardSuitFilter filterSuit = CardSuitFilter.All;
    }
}