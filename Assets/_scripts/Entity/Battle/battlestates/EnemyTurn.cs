using Ain;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
public class EnemyTurn : IState
{
    private BattleManager _battleManager;
    private EnemyManager enemyManager;
    public EnemyTurn(BattleManager battleManager, EnemyManager enemyManager)
    {
        _battleManager = battleManager;
        this.enemyManager = enemyManager;
    }

    public void OnEnter()
    {
        Debug.Log("EnemyTurn: OnEnter");
        _ = HandleAttack();        
    }

    public void OnExit()
    {
        Debug.Log("EnemyTurn: OnExit");
    }

    public void Tick()
    {
    }
    public async UniTask HandleAttack()
    {
        var sequence = DOTween.Sequence();
        var enemies = enemyManager.GetEnemiesAlive();
        var player = PlayerController.Instance;
        foreach (var enemy in enemies)
        {
            
            if (player.IsDead) continue;

            enemy.Count();

            await UniTask.Delay(1000);
        }
        Debug.Log("da");
        _battleManager.CheckWinLose();
    }
}
