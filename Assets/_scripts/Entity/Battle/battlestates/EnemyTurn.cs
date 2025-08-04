using Ain;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
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
        Debug.Log("EnemyTurn: OnEnter");
        var enimies = enemyManager.GetEnemiesAlive();
        foreach (var enemy in enimies)
        {
            enemy.Count();
        }

        enemyManager.EndTurn();
        battleManager.PlayerTurn();
    }

    public void OnExit()
    {
        Debug.Log("EnemyTurn: OnExit");
       
    }

    public void Tick()
    {
    }
}
