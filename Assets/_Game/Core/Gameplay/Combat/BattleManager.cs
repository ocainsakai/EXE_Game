using UnityEngine;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour
{
    [SerializeField] UIGameplay uiGameplay;

    public UnityEvent OnBattleStart;

    private EnemyData currentEnemy;

    public void OnEnterTheBattleWithTile(Tile tile)
    {
        if (tile.Type == TileType.Enemy || tile.Type == TileType.Boss)
        {
            var enemy = GameInstance.Singleton.GetEnemyData(tile.OccupantID);
            if (enemy == null) 
            {
                Debug.LogError($"Enemy data not found for ID: {tile.OccupantID}");
                return;
            }
            StartBattle(enemy);
        }
        else
        {
            Debug.LogWarning("No enemy on this tile to battle.");
        }
    }
    public void StartBattle(EnemyData enemy)
    {
        currentEnemy = enemy;
        Debug.Log($"Starting battle with {enemy.Name}");
        
        uiGameplay.ShowBattle(enemy);

        OnBattleStart?.Invoke();

    }
}
