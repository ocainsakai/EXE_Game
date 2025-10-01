using UnityEngine;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour
{
    [SerializeField] UIGameplay uiGameplay;

    public UnityEvent OnBattleStart;
    public UnityEvent OnBattleWin;

    private EnemyData currentEnemy;

    public void OnEnterTheBattleWithTile(Tile tile)
    {
        if (tile.Type == TileType.Enemy)
        {
            currentEnemy = GameInstance.Singleton.GetEnemyData(tile.OccupantID);
        }
        else if (tile.Type == TileType.Boss)
        {
            currentEnemy = GameInstance.Singleton.GetBoss(tile.OccupantID);
        }

        if (currentEnemy == null)
        {
            Debug.LogError($"Enemy data not found for ID: {tile.OccupantID}");
            return;
        }
        StartBattle(currentEnemy);
    }

    public void StartBattle(EnemyData enemy)
    {
        currentEnemy = enemy;
        Debug.Log($"Starting battle with {enemy.Name}");
        
        uiGameplay.ShowBattle(enemy);

        OnBattleStart?.Invoke();

    }
    public void WinTheBattle()
    {
        OnBattleWin?.Invoke();
    }
}
