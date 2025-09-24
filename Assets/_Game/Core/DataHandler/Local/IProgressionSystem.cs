public interface IProgressionSystem
{
    // Initialize player state for new run
    PlayerData CreateNewPlayer();

    // Update player stats (HP, Gold, Deck, etc.)
    void UpdatePlayerData(PlayerData data);

    // Save run state
    void SaveProgress(string saveId, PlayerData player);

    // Load run state
    PlayerData LoadProgress(string saveId);

    // Reset run (Game Over / Restart)
    void ResetProgress();
}
