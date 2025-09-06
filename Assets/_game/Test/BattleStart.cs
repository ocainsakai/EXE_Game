namespace Game
{
    public class BattleStart : BattleState
    {
        protected EnemyData enemy;
        protected PlayerConfig playerConfig;
        public BattleStart(BattleController controller, EnemyData enemyData, PlayerConfig playerConfig) : base(controller)
        {
            this.enemy = enemyData;
            this.playerConfig = playerConfig;
        }

        public override void OnEnter()
        {
            //UIManager.Instance.CloseAll();


            playerController.LoadPlayerConfig(playerConfig);
            playerController.BuidDeck();

            enemyManager.LoadEnemy(enemy);

        }
        public override void OnExit()
        {
        }
    }
}