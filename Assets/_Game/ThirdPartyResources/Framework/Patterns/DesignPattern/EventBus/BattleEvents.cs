public static class BattleEvents
{
    public class End { }
    public class Win { }
    public class Lose { }
    public class TurnChanged
    {
        public int TurnNumber;
        public TurnChanged(int turn) { TurnNumber = turn; }
    }
}
