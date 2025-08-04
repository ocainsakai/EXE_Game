using Ain;
using System.Collections.Generic;
public class BattleStateMachine : StateMachine<BattleState,IState>
{
    public BattleStateMachine(BattleManager battleManager)
    {
        States = new Dictionary<BattleState, IState>
        {
            { BattleState.BattleStart,  new BattleStart(battleManager) },
            { BattleState.PlayerTurn, new PlayerTurn(battleManager, battleManager.playerController) },
            { BattleState.EnemyTurn, new EnemyTurn(battleManager, battleManager.enemyManager) },
            { BattleState.WinBattle,    new BattleEnd(battleManager, true) },
            { BattleState.LoseBattle, new BattleEnd(battleManager, false) },
        };
    }
}
