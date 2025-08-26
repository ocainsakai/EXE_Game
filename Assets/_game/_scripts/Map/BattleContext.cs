using System;

namespace Map
{
    public class BattleContext
    {
        public EnemyData enemyData;
        public BattleContext(EnemyData enemyData)
        {
            this.enemyData = enemyData;
        }
    }
}