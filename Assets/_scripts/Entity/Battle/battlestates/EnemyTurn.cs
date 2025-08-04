using Ain;

public class EnemyTurn : IState
{
    private BattleManager battleManager;
    private EnemyManager enemyManager;
    public EnemyTurn(BattleManager battleManager, EnemyManager enemyManager)
    {
        this.battleManager = battleManager;
        this.enemyManager = enemyManager;
    }

    public void OnEnter()
    {
        foreach (var enemy in enemyManager.enemyList.enemies)
        {
            // Start the enemy's turn
            enemy.Count();
        }
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
    }
}
