using CardSystem.PokerSystem;
using UnityEngine.Events;
using UnityEngine.Serialization;

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
    [FormerlySerializedAs("OnBattleStart")] public UnityEvent onBattleStart = new();
    [FormerlySerializedAs("OnPlayerDamaged")] public DamageEvent onPlayerDamaged = new();
    [FormerlySerializedAs("OnEnemyDamaged")] public DamageEvent onEnemyDamaged = new();
    [FormerlySerializedAs("OnHandPlayed")] public HandPlayedEvent onHandPlayed = new();
    [FormerlySerializedAs("OnEnemyTurn")] public UnityEvent onEnemyTurn = new();
    [FormerlySerializedAs("OnRoundStart")] public UnityEvent<int> onRoundStart = new(); // Mỗi round mới (sau enemy turn)
    [FormerlySerializedAs("OnBattleEnd")] public BattleEndEvent onBattleEnd = new();
    [FormerlySerializedAs("OnStateChanged")] public UnityEvent onStateChanged = new();

    // Energy events
    [FormerlySerializedAs("OnEnergyChanged")] public EnergyChangedEvent onEnergyChanged = new();
    [FormerlySerializedAs("OnEnergyDepleted")] public UnityEvent onEnergyDepleted = new(); // Hết energy

    // ==================== TRIGGERS ====================

    public void TriggerBattleStart()
    {
        onBattleStart?.Invoke();
    }

    public void TriggerPlayerDamaged(int damage, string source = "Enemy")
    {
        onPlayerDamaged?.Invoke(damage, source);
    }

    public void TriggerEnemyDamaged(int damage, string source = "Player")
    {
        onEnemyDamaged?.Invoke(damage, source);
    }

    public void TriggerHandPlayed(PokerHandType handType, int damage)
    {
        onHandPlayed?.Invoke(handType, damage);
    }

    public void TriggerEnemyTurn()
    {
        onEnemyTurn?.Invoke();
    }

    public void TriggerRoundStart(int roundNumber)
    {
        onRoundStart?.Invoke(roundNumber);
    }

    public void TriggerBattleEnd(bool isVictory)
    {
        onBattleEnd?.Invoke(isVictory);
    }

    public void TriggerStateChanged()
    {
        onStateChanged?.Invoke();
    }

    public void TriggerEnergyChanged(int current, int max)
    {
        onEnergyChanged?.Invoke(current, max);
    }

    public void TriggerEnergyDepleted()
    {
        onEnergyDepleted?.Invoke();
    }
}