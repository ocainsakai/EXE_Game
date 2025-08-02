
// CombatSimulationManager.cs
using Ain;

public class CombatSimulationManager : Singleton<CombatSimulationManager>
{
    public EnemyData SelectedEnemy { get; private set; }
    //public PlayerData PlayerData { get; private set; }

    public void SetSelectedEnemy(EnemyData enemy) => SelectedEnemy = enemy;
    //public void SetPlayerData(PlayerData player) => PlayerData = player;
}