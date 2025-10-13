using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Hệ thống battle với Energy System
/// </summary>
public class BattleSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MultTable multTable;
    [SerializeField] private Slider energyBar;
    [SerializeField] private Enemy enemy;
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

        Events.OnEnergyChanged.AddListener((curent, max) =>
        {
            Debug.Log($"{curent}/{max}");
            energyBar.maxValue = max;
            energyBar.value = curent;
        });
        StartBattle(null, enemy.Data);
    }

    private void Start()
    {
        // test
    }
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
   

    public bool CanPlayHand(List<Card> selectedCards)
    {
        int cost = selectedCards.Count * energyCostPlay;
        if (_state.CurrentEnergy < cost)
        {
            return false;
        }
        return true;
    }

    public bool CanDiscard()
    {
        if (_state.CurrentEnergy < energyCostDiscard)
        {
            return false;
        }
        return true;
    }

    public void UseEnergyPlay()
    {
        _state.TryUseEnergy(energyCostPlay);
        Events.TriggerEnergyChanged(_state.CurrentEnergy, _state.MaxEnergy);

    }
    public void UseEnergyDiscard()
    {
        //Debug.Log($"energyCostDiscard: {energyCostDiscard}");
        _state.TryUseEnergy(energyCostDiscard);
        Events.TriggerEnergyChanged(_state.CurrentEnergy, _state.MaxEnergy);

    }
    public void RegenEnergy()
    {
        _state.RestoreEnergy(energyRegenPerRound);
        Events.TriggerEnergyChanged(_state.CurrentEnergy, _state.MaxEnergy);

    }
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