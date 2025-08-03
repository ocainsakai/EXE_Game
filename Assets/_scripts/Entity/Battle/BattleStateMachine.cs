using Ain;

public class BattleStateMachine : StateMachine<IState>
{
    public BattleStart battleStart;
    public BattleEnd battleEnd;
    public DrawPhase drawPhase;
    public EnemyTurn enemyTurn;
    public PlayerTurn playerTurn;
    private BattleManager _battleManager;

    public void Initialize(BattleManager battleManager)
    {
        _battleManager = battleManager;
        battleStart = new BattleStart(this, _battleManager.deckList);
    }

}
