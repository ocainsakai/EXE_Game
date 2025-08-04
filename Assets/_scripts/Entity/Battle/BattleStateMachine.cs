using Ain;

public class BattleStateMachine : StateMachine<IState>
{
    public BattleStart battleStart;
    public BattleEnd winBattle;
    public BattleEnd loseBattle;
    public DrawPhase drawPhase;
    public EnemyTurn enemyTurn;
    public PlayerTurn playerTurn;
    //private BattleManager _battleManager;

    public void Initialize(BattleManager battleManager, PlayerController playerController)
    {
        battleStart = new BattleStart(battleManager, battleManager.deckList);
        playerTurn = new PlayerTurn(battleManager, playerController);
        enemyTurn = new EnemyTurn(battleManager);
        winBattle = new BattleEnd(battleManager, true);
        loseBattle = new BattleEnd(battleManager, false);
    }

}
