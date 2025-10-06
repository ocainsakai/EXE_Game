using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manager tổng thể cho battle flow - kết nối Map và Battle System
/// </summary>
public class BattleManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BattleSystem battleSystem;
    //[SerializeField] private UIGameplay uiGameplay;
    [SerializeField] private MapManager mapManager;

    [Header("Events")]
    public UnityEvent OnBattleStart;
    public UnityEvent OnBattleWin;
    public UnityEvent OnBattleLose;

    private Tile currentTile;
    private EnemyData currentEnemy;

    private void OnEnable()
    {
        // Subscribe to battle system events
        if (battleSystem != null)
        {
            battleSystem.Events.OnBattleEnd.AddListener(OnBattleEnded);
        }
    }

    private void OnDisable()
    {
        if (battleSystem != null)
        {
            battleSystem.Events.OnBattleEnd.RemoveListener(OnBattleEnded);
        }
    }

    /// <summary>
    /// Start battle khi click vào tile trên map
    /// </summary>
    public void OnEnterTheBattleWithTile(Tile tile)
    {
        if (tile == null)
        {
            Debug.LogError("[BattleManager] Tile is null!");
            return;
        }

        currentTile = tile;

        // Get enemy data based on tile type
        if (tile.Type == TileType.Enemy)
        {
            currentEnemy = GameInstance.Singleton?.GetEnemyData(tile.OccupantID);
        }
        else if (tile.Type == TileType.Boss)
        {
            currentEnemy = GameInstance.Singleton?.GetBoss(tile.OccupantID);
        }
        else
        {
            Debug.LogWarning($"[BattleManager] Tile type {tile.Type} is not a battle tile!");
            return;
        }

        if (currentEnemy == null)
        {
            Debug.LogError($"[BattleManager] Enemy data not found for ID: {tile.OccupantID}");
            return;
        }

        StartBattle(currentEnemy);
    }

    /// <summary>
    /// Start battle with specific enemy
    /// </summary>
    public void StartBattle(EnemyData enemy)
    {
        if (enemy == null)
        {
            Debug.LogError("[BattleManager] Cannot start battle - enemy is null!");
            return;
        }

        currentEnemy = enemy;

        Debug.Log($"[BattleManager] Starting battle with {enemy.Name}");

        // Get player data
        PlayerData playerData = GetPlayerData();

        // Show battle UI
        //if (uiGameplay != null)
        //{
        //    uiGameplay.ShowBattle(enemy);
        //}

        // Start battle in system
        if (battleSystem != null)
        {
            battleSystem.StartBattle(playerData, enemy);
        }

        OnBattleStart?.Invoke();
    }

    /// <summary>
    /// Handle battle end
    /// </summary>
    private void OnBattleEnded(bool isVictory)
    {
        Debug.Log($"[BattleManager] Battle ended - {(isVictory ? "Victory" : "Defeat")}");

        if (isVictory)
        {
            HandleVictory();
        }
        else
        {
            HandleDefeat();
        }
    }

    /// <summary>
    /// Handle victory
    /// </summary>
    private void HandleVictory()
    {
        // Give rewards
        if (currentEnemy != null)
        {
            int reward = currentEnemy.reward;
            Debug.Log($"[BattleManager] Victory! Gained {reward} gold");

            // TODO: Add gold to player
            // PlayerData.Gold += reward;
        }

        // Update map
        if (mapManager != null && currentTile != null)
        {
            mapManager.OnBattleWin();
        }

        OnBattleWin?.Invoke();

        // Hide battle UI
        //if (uiGameplay != null)
        //{
        //    // Will be handled by continue button
        //}
    }

    /// <summary>
    /// Handle defeat
    /// </summary>
    private void HandleDefeat()
    {
        Debug.Log("[BattleManager] Defeat - Game Over");

        OnBattleLose?.Invoke();

        // TODO: Show game over screen or retry option
    }

    /// <summary>
    /// Get player data (from GameInstance or default)
    /// </summary>
    private PlayerData GetPlayerData()
    {
        // TODO: Get from GameInstance when implemented
        PlayerData playerData = ScriptableObject.CreateInstance<PlayerData>();
        return playerData;
    }

    // ==================== PUBLIC API ====================

    /// <summary>
    /// Check if currently in battle
    /// </summary>
    public bool IsInBattle()
    {
        return battleSystem != null &&
               battleSystem.State != null &&
               !battleSystem.State.IsBattleOver;
    }

    /// <summary>
    /// Get current battle state
    /// </summary>
    public BattleState GetBattleState()
    {
        return battleSystem?.State;
    }

    // ==================== DEBUG ====================

    [ContextMenu("Debug - Start Test Battle")]
    private void DebugStartTestBattle()
    {
        // Create test enemy
        var testEnemy = ScriptableObject.CreateInstance<EnemyData>();
        testEnemy.Name = "Test Enemy";
        testEnemy.HP = 50;
        testEnemy.Atk = 10;
        testEnemy.reward = 100;

        StartBattle(testEnemy);
    }
}