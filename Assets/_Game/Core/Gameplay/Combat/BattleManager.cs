using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manager tổng thể cho battle flow - kết nối Map và Battle System
/// </summary>
public class BattleManager : MonoBehaviour
{
    [SerializeField] private BattleSystem battleSystem;
    [SerializeField] private Enemy enemy;
    [SerializeField] private PlayerActionController playerActionController;
    [SerializeField] private PlayerStatComponent playerStatComponent;

    public UnityEvent OnBattleStart;
    public UnityEvent OnBattleEnd;
    public UnityEvent OnBattleWin;
    public UnityEvent OnBattleLose;
    public void BattleStart(EnemyData enemyData)
    {
        
        var playerData = GameInstance.Singleton.PlayerData;
        battleSystem.StartBattle(playerData, enemyData);
        playerActionController.gameObject.SetActive(true);
        this.enemy.SetData(enemyData);

        OnBattleStart?.Invoke();
    }
    public void CheckCondition(object sender)
    {
        if (playerStatComponent.HP <= 0)
        {
            Debug.Log($"You lose");
            // lose resolve
            OnBattleEnd?.Invoke();
            OnBattleLose?.Invoke();
            return;
        }
        if (enemy.HP <=0)
        {
            Debug.Log($"You win");
            // win resolve
            OnBattleEnd?.Invoke();
            OnBattleWin?.Invoke();
            return;
        }
        if (sender != null && (sender is Enemy))
        {
            Debug.Log($"You start your turn");
            playerActionController.PlayerStartTurn();
            return;
        }
        if (sender != null && sender is PlayerActionController)
        {
            Debug.Log($"You start enemy turn");
            enemy.CountToAction();
            return;
        }
    }
    public void AttackPlayer(Enemy enemy)
    {
        playerStatComponent.HP -= enemy.Data.Atk;
    }
}