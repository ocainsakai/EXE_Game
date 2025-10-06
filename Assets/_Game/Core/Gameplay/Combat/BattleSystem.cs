using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Hệ thống battle với Energy System
/// </summary>
public class BattleSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MultTable multTable;

    [Header("Energy Settings")]
    [SerializeField] private int startEnergy = 3;
    [SerializeField] private int maxEnergy = 3;
    [SerializeField] private int energyRegenPerRound = 1;
    [SerializeField] private int energyCostPlay = 1;
    [SerializeField] private int energyCostDiscard = 1;

    [Header("Timing")]
    [SerializeField] private float enemyAttackDelay = 1f;
    [SerializeField] private float actionDelay = 0.3f;

    // State
    private BattleState _state;
    private DamageCalculator _damageCalculator;

    // Events
    public BattleEvents Events { get; private set; } = new BattleEvents();

    // Properties
    public BattleState State => _state;
    public bool IsProcessing { get; private set; }

    private void Awake()
    {
        if (multTable == null)
        {
            Debug.LogError("[BattleSystem] MultTable is not assigned!");
        }

        _damageCalculator = new DamageCalculator(multTable);
    }

    // ==================== BATTLE LIFECYCLE ====================

    /// <summary>
    /// Bắt đầu battle mới
    /// </summary>
    public void StartBattle(PlayerData player, EnemyData enemy)
    {
        _state = new BattleState(player, enemy, startEnergy, maxEnergy, energyRegenPerRound);

        Debug.Log($"[BattleSystem] Battle started: Player vs {enemy.Name} (HP: {enemy.HP}, Energy: {startEnergy})");

        Events.TriggerBattleStart();
        Events.TriggerEnergyChanged(_state.CurrentEnergy, _state.MaxEnergy);
        Events.TriggerStateChanged();
    }

    // ==================== PLAYER ACTIONS ====================

    /// <summary>
    /// Player chơi 1 hand (cost 1 energy)
    /// </summary>
    public void PlayHand(List<Card> selectedCards)
    {
        // Validation
        if (!CanPlayHand(selectedCards, out string errorMsg))
        {
            Debug.LogWarning($"[BattleSystem] Cannot play hand: {errorMsg}");
            return;
        }

        StartCoroutine(PlayHandRoutine(selectedCards));
    }

    private bool CanPlayHand(List<Card> selectedCards, out string errorMsg)
    {
        errorMsg = null;

        if (IsProcessing)
        {
            errorMsg = "Already processing an action";
            return false;
        }

        if (_state.IsBattleOver)
        {
            errorMsg = "Battle is already over";
            return false;
        }

        if (!_state.IsPlayerTurn)
        {
            errorMsg = "Not player's turn";
            return false;
        }

        if (!_state.CanUseEnergy(energyCostPlay))
        {
            errorMsg = $"Not enough energy (need {energyCostPlay}, have {_state.CurrentEnergy})";
            Events.TriggerEnergyDepleted();
            return false;
        }

        if (selectedCards == null || selectedCards.Count == 0)
        {
            errorMsg = "No cards selected";
            return false;
        }

        return true;
    }

    private IEnumerator PlayHandRoutine(List<Card> selectedCards)
    {
        IsProcessing = true;

        // 1. Use energy
        _state.TryUseEnergy(energyCostPlay);
        Events.TriggerEnergyChanged(_state.CurrentEnergy, _state.MaxEnergy);

        // 2. Tính damage
        int damage = _damageCalculator.Calculate(selectedCards, out var handResult);
        Events.TriggerHandPlayed(handResult.HandType, damage);

        yield return new WaitForSeconds(actionDelay);

        // 3. Damage enemy
        _state.DamageEnemy(damage);
        Events.TriggerEnemyDamaged(damage, "Player");
        Events.TriggerStateChanged();

        yield return new WaitForSeconds(0.5f);

        // 4. Check win
        if (_state.IsBattleOver)
        {
            EndBattle();
            IsProcessing = false;
            yield break;
        }

        // 5. Nếu hết energy → auto end turn
        if (_state.CurrentEnergy == 0)
        {
            Debug.Log("[BattleSystem] Energy depleted - Auto ending turn");
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(EnemyTurnRoutine());
        }

        // 6. Check lose after enemy turn
        if (_state.IsBattleOver)
        {
            EndBattle();
        }

        IsProcessing = false;
    }

    /// <summary>
    /// Discard cards (cost 1 energy) - TODO: Implement card draw
    /// </summary>
    public void DiscardCards(List<Card> cards)
    {
        if (!CanDiscard(out string errorMsg))
        {
            Debug.LogWarning($"[BattleSystem] Cannot discard: {errorMsg}");
            return;
        }

        StartCoroutine(DiscardRoutine(cards));
    }

    private bool CanDiscard(out string errorMsg)
    {
        errorMsg = null;

        if (IsProcessing)
        {
            errorMsg = "Already processing an action";
            return false;
        }

        if (!_state.IsPlayerTurn)
        {
            errorMsg = "Not player's turn";
            return false;
        }

        if (!_state.CanUseEnergy(energyCostDiscard))
        {
            errorMsg = $"Not enough energy (need {energyCostDiscard}, have {_state.CurrentEnergy})";
            Events.TriggerEnergyDepleted();
            return false;
        }

        return true;
    }

    private IEnumerator DiscardRoutine(List<Card> cards)
    {
        IsProcessing = true;

        // Use energy
        _state.TryUseEnergy(energyCostDiscard);
        Events.TriggerEnergyChanged(_state.CurrentEnergy, _state.MaxEnergy);

        Debug.Log($"[BattleSystem] Discarded {cards.Count} cards");

        // TODO: Draw new cards from deck
        Events.TriggerStateChanged();

        yield return new WaitForSeconds(actionDelay);

        // Auto end turn if no energy
        if (_state.CurrentEnergy == 0)
        {
            yield return StartCoroutine(EnemyTurnRoutine());
        }

        IsProcessing = false;
    }

    /// <summary>
    /// Manual end turn (skip remaining energy)
    /// </summary>
    public void EndTurn()
    {
        if (IsProcessing || !_state.IsPlayerTurn)
            return;

        StartCoroutine(EnemyTurnRoutine());
    }

    // ==================== ENEMY TURN ====================

    /// <summary>
    /// Enemy attack turn
    /// </summary>
    private IEnumerator EnemyTurnRoutine()
    {
        Debug.Log("[BattleSystem] Enemy turn starts");

        _state.EndPlayerTurn();
        Events.TriggerEnemyTurn();

        yield return new WaitForSeconds(enemyAttackDelay);

        // Enemy attack
        int enemyDamage = _state.Enemy.Atk;
        _state.DamagePlayer(enemyDamage);

        Events.TriggerPlayerDamaged(enemyDamage, _state.Enemy.Name);
        Events.TriggerStateChanged();

        yield return new WaitForSeconds(0.5f);

        // Check lose
        if (_state.IsBattleOver)
        {
            yield break;
        }

        // Start new round - regenerate energy
        _state.StartNewRound();
        Events.TriggerRoundStart(_state.RoundNumber);
        Events.TriggerEnergyChanged(_state.CurrentEnergy, _state.MaxEnergy);
        Events.TriggerStateChanged();

        Debug.Log($"[BattleSystem] Round {_state.RoundNumber} - Energy restored to {_state.CurrentEnergy}");
    }

    // ==================== BATTLE END ====================

    /// <summary>
    /// Kết thúc battle
    /// </summary>
    private void EndBattle()
    {
        bool isVictory = _state.IsPlayerVictory;

        Debug.Log($"[BattleSystem] Battle ended - {(isVictory ? "VICTORY" : "DEFEAT")}");

        Events.TriggerBattleEnd(isVictory);
    }

    /// <summary>
    /// Force end battle (debug/cheat)
    /// </summary>
    public void ForceBattleEnd(bool isVictory)
    {
        if (_state != null && !_state.IsBattleOver)
        {
            _state.ForceEnd(isVictory);
            EndBattle();
        }
    }

    // ==================== QUERIES ====================

    public bool CanAffordPlay() => _state != null && _state.CanUseEnergy(energyCostPlay);
    public bool CanAffordDiscard() => _state != null && _state.CanUseEnergy(energyCostDiscard);
}