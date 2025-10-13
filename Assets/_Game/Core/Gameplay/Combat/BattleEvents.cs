using CardSystem.PokerSystem;
using UnityEngine.Events;
/// <summary>
/// Events để UI subscribe
/// </summary>
[System.Serializable]
public class BattleEvents
{
    [System.Serializable]
    public class DamageEvent : UnityEvent<int, string> { } // damage, target name

    [System.Serializable]
    public class HandPlayedEvent : UnityEvent<PokerHandType, int> { } // hand type, damage

    [System.Serializable]
    public class BattleEndEvent : UnityEvent<bool> { } // isVictory

    [System.Serializable]
    public class EnergyChangedEvent : UnityEvent<int, int> { } // current, max

    // Core events
    public UnityEvent OnBattleStart = new();
    public DamageEvent OnPlayerDamaged = new();
    public DamageEvent OnEnemyDamaged = new();
    public HandPlayedEvent OnHandPlayed = new();
    public UnityEvent OnEnemyTurn = new();
    public UnityEvent<int> OnRoundStart = new(); // Mỗi round mới (sau enemy turn)
    public BattleEndEvent OnBattleEnd = new();
    public UnityEvent OnStateChanged = new();

    // Energy events
    public EnergyChangedEvent OnEnergyChanged = new();
    public UnityEvent OnEnergyDepleted = new(); // Hết energy

    // ==================== TRIGGERS ====================

    public void TriggerBattleStart()
    {
        OnBattleStart?.Invoke();
    }

    public void TriggerPlayerDamaged(int damage, string source = "Enemy")
    {
        OnPlayerDamaged?.Invoke(damage, source);
    }

    public void TriggerEnemyDamaged(int damage, string source = "Player")
    {
        OnEnemyDamaged?.Invoke(damage, source);
    }

    public void TriggerHandPlayed(PokerHandType handType, int damage)
    {
        OnHandPlayed?.Invoke(handType, damage);
    }

    public void TriggerEnemyTurn()
    {
        OnEnemyTurn?.Invoke();
    }

    public void TriggerRoundStart(int roundNumber)
    {
        OnRoundStart?.Invoke(roundNumber);
    }

    public void TriggerBattleEnd(bool isVictory)
    {
        OnBattleEnd?.Invoke(isVictory);
    }

    public void TriggerStateChanged()
    {
        OnStateChanged?.Invoke();
    }

    public void TriggerEnergyChanged(int current, int max)
    {
        OnEnergyChanged?.Invoke(current, max);
    }

    public void TriggerEnergyDepleted()
    {
        OnEnergyDepleted?.Invoke();
    }
}