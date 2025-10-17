using UnityEngine;

/// <summary>
/// Lưu trạng thái hiện tại của battle với Energy System
/// </summary>
public class BattleState
{
    public PlayerData Player { get; private set; }
    public EnemyData Enemy { get; private set; }

    public int PlayerHp { get; private set; }
    public int PlayerMaxHp { get; private set; }
    public int EnemyHp { get; private set; }
    public int EnemyMaxHp { get; private set; }

    // Energy System
    public int CurrentEnergy { get; private set; }
    public int MaxEnergy { get; private set; }
    public int EnergyRegenPerRound { get; private set; }

    public int RoundNumber { get; private set; }
    public bool IsPlayerTurn { get; private set; }
    public bool IsBattleOver { get; private set; }
    public bool IsPlayerVictory { get; private set; }

    public BattleState(
        PlayerData player,
        EnemyData enemy,
        int startEnergy = 3,
        int maxEnergy = 3,
        int energyRegen = 1)
    {
        Player = player;
        Enemy = enemy;

        // TODO: Get từ PlayerData khi có
        PlayerMaxHp = 100;
        PlayerHp = PlayerMaxHp;

        EnemyMaxHp = enemy.hp;
        EnemyHp = EnemyMaxHp;

        // Energy setup
        MaxEnergy = maxEnergy;
        CurrentEnergy = startEnergy;
        EnergyRegenPerRound = energyRegen;

        RoundNumber = 1;
        IsPlayerTurn = true;
        IsBattleOver = false;
    }

    // ==================== DAMAGE ====================

    public void DamageEnemy(int damage)
    {
        EnemyHp = Mathf.Max(0, EnemyHp - damage);
        CheckBattleEnd();
    }

    public void DamagePlayer(int damage)
    {
        PlayerHp = Mathf.Max(0, PlayerHp - damage);
        CheckBattleEnd();
    }

    // ==================== ENERGY ====================

    public bool CanUseEnergy(int amount = 1)
    {
        return CurrentEnergy >= amount;
    }

    public bool TryUseEnergy(int amount = 1)
    {
        if (!CanUseEnergy(amount))
            return false;

        CurrentEnergy = Mathf.Max(0, CurrentEnergy - amount);
        return true;
    }

    public void RestoreEnergy(int amount)
    {
        CurrentEnergy = Mathf.Min(MaxEnergy, CurrentEnergy + amount);
    }

    public void SetEnergyToMax()
    {
        CurrentEnergy = MaxEnergy;
    }

    // ==================== TURN MANAGEMENT ====================

    public void EndPlayerTurn()
    {
        IsPlayerTurn = false;
    }

    public void StartNewRound()
    {
        RoundNumber++;
        IsPlayerTurn = true;

        // Regenerate energy each round
        RestoreEnergy(EnergyRegenPerRound);
    }

    // ==================== BATTLE END ====================

    private void CheckBattleEnd()
    {
        if (PlayerHp <= 0)
        {
            IsBattleOver = true;
            IsPlayerVictory = false;
        }
        else if (EnemyHp <= 0)
        {
            IsBattleOver = true;
            IsPlayerVictory = true;
        }
    }

    public void ForceEnd(bool victory)
    {
        IsBattleOver = true;
        IsPlayerVictory = victory;
    }

    // ==================== HELPERS ====================

    public float GetPlayerHpPercent() => (float)PlayerHp / PlayerMaxHp;
    public float GetEnemyHpPercent() => (float)EnemyHp / EnemyMaxHp;
    public float GetEnergyPercent() => (float)CurrentEnergy / MaxEnergy;
}

