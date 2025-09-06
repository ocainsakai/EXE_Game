using UnityEngine;

namespace Map
{
    public class BattleResult
    {
        public bool IsWin { get; private set; }
        public int GoldReward { get; private set; }
        public int ExpReward { get; private set; }
        //public Vector2Int MapPosition { get; private set; }

        public BattleResult(bool isWin, int gold, int exp)
        {
            IsWin = isWin;
            GoldReward = gold;
            ExpReward = exp;
            //MapPosition = position;
        }
    }

}