using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] UiBattle uiBattle;
    private EnemyData currentEnemy;

    public void OnEnterTheBattleWithTile(Tile tile)
    {
        if (tile.Type == TileType.Enemy || tile.Type == TileType.Boss)
        {
            var enemy = GameInstance.Singleton.GetEnemyData(tile.OccupantID);
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
        // Initialize battle UI, player stats, enemy stats, etc.


        // open ui
        //uiBattle.Open(enemy);
    }
}
