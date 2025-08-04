using Ain;

public class EnemyTurn : IState
{
    private BattleManager battleManager;

    public EnemyTurn(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    public void OnEnter()
    {
        battleManager.PlayerTurn();
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
    }
}
