namespace CardSystem
{
    public struct CardMask
    {
        private readonly int value;

        private const int SUIT_MASK = 0b11;      // 2 bit cho suit
        private const int RANK_MASK = 0b11111;   // 5 bit cho rank
        private const int SUIT_SHIFT = 0;
        private const int RANK_SHIFT = 2;

        public CardMask(int rank, int suit)
        {
            value = (rank & RANK_MASK) << RANK_SHIFT | (suit & SUIT_MASK) << SUIT_SHIFT;
        }
        public CardMask(CardRank rank, CardSuit suit)
        {
            value = (((int)rank & RANK_MASK) << RANK_SHIFT) | (((int)suit & SUIT_MASK) << SUIT_SHIFT);
        }
        public int RawValue => value;

        public int Rank => value >> RANK_SHIFT & RANK_MASK;
        public int Suit => value >> SUIT_SHIFT & SUIT_MASK;
        public CardRank ERank => (CardRank)((value >> RANK_SHIFT) & RANK_MASK);
        public CardSuit ESuit => (CardSuit)((value >> SUIT_SHIFT) & SUIT_MASK);
        public override string ToString()
        {
            return $"{GetRankName(Rank)} of {GetSuitName(Suit)}";
        }

        private static string GetSuitName(int suit) =>
            suit switch
            {
                0 => "Hearts",
                1 => "Diamonds",
                2 => "Clubs",
                3 => "Spades",
                _ => "Unknown Suit"
            };

        private static string GetRankName(int rank) =>
            rank switch
            {
                2 => "Two",
                3 => "Three",
                4 => "Four",
                5 => "Five",
                6 => "Six",
                7 => "Seven",
                8 => "Eight",
                9 => "Nine",
                10 => "Ten",
                11 => "Jack",
                12 => "Queen",
                13 => "King",
                14 => "Ace",
                _ => "Unknown Rank"
            };
    }
}