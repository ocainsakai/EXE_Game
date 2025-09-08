
using Game.Service;

namespace Game
{
    public abstract class BattleState : BaseState
    {
        protected readonly BattleController controller;
        protected readonly BattlePlayer playerController;
        protected readonly BattleEnemy enemyManager;
        public BattleState(BattleController controller)
        {
            this.controller = controller;
            playerController = controller.playerController;
            enemyManager = controller.enemyManager;
        }
    }
}