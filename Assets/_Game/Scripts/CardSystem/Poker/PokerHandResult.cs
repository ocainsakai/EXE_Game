using System.Collections.Generic;

namespace CardSystem.PokerSystem
{
    public struct PokerHandResult
    {
        public PokerHandType HandType;
        public List<CardMask> BestCards;

        public override string ToString() => HandType.ToString();
    }

}