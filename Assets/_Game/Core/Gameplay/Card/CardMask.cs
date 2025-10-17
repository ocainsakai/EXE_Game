namespace CardSystem
{
    public struct CardMask
    {
        private readonly int value;

        private const int SuitMask = 0b11;      // 2 bit cho suit
        private const int RankMask = 0b11111;   // 5 bit cho rank
        private const int SuitShift = 0;
        private const int RankShift = 2;

        public CardMask(int rank, int suit)
        {
            value = (rank & RankMask) << RankShift | (suit & SuitMask) << SuitShift;
        }
        public CardMask(CardRank rank, CardSuit suit)
        {
            value = (((int)rank & RankMask) << RankShift) | (((int)suit & SuitMask) << SuitShift);
        }
        public int RawValue => value;

        public int Rank => value >> RankShift & RankMask;
        public int Suit => value >> SuitShift & SuitMask;
        public CardRank ERank => (CardRank)((value >> RankShift) & RankMask);
        public CardSuit ESuit => (CardSuit)((value >> SuitShift) & SuitMask);
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