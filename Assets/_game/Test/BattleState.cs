
using Game.Service;

namespace Game
{
    public abstract class BattleState : BaseState
    {
        protected readonly BattleController controller;
        protected readonly PlayerController playerController;
        protected readonly EnemyManager enemyManager;
        public BattleState(BattleController controller)
        {
            this.controller = controller;
            playerController = controller.playerController;
            enemyManager = controller.enemyManager;
        }
    }
}