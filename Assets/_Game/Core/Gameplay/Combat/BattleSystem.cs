using System.Collections.Generic;
using _Game.Addons.Deck.Scripts;
using _Game.Addons.Deck.Scripts.Card;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
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


    // State
    private BattleState _state;
    // Properties
    public BattleState State => _state;

    [FormerlySerializedAs("OnEnergyChanged")] public UnityEvent<int, int> onEnergyChanged;

    private void Awake()
    {
        if (multTable == null)
        {
            Debug.LogError("[BattleSystem] MultTable is not assigned!");
        }
    }

    public void StartBattle(PlayerData player, EnemyData enemy)
    {
        _state = new BattleState(player, enemy, startEnergy, maxEnergy, energyRegenPerRound);

        Debug.Log($"[BattleSystem] Battle started: Player vs {enemy.name} (HP: {enemy.hp}, Energy: {startEnergy})");

        onEnergyChanged?.Invoke(_state.CurrentEnergy, _state.MaxEnergy);
    }

    // ==================== PLAYER ACTIONS ====================

    /// <summary>
    /// Player chơi 1 hand (cost 1 energy)
    /// </summary>
   

    public bool CanPlayHand(List<CardRuntime> selectedCards)
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
        onEnergyChanged?.Invoke(_state.CurrentEnergy, _state.MaxEnergy);

    }
    public void UseEnergyDiscard()
    {
        _state.TryUseEnergy(energyCostDiscard);
        onEnergyChanged?.Invoke(_state.CurrentEnergy, _state.MaxEnergy);

    }
    public void RegenEnergy()
    {
        _state.RestoreEnergy(energyRegenPerRound);
        onEnergyChanged?.Invoke(_state.CurrentEnergy, _state.MaxEnergy);

    }
    private void EndBattle()
    {
        bool isVictory = _state.IsPlayerVictory;

        Debug.Log($"[BattleSystem] Battle ended - {(isVictory ? "VICTORY" : "DEFEAT")}");
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