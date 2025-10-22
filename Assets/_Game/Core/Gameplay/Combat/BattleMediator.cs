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
public class BattleMediator : MonoBehaviour
{
    [Header("Energy Settings")]
    [SerializeField] private int startEnergy = 3;
    [SerializeField] private int maxEnergy = 3;
    [SerializeField] private int energyRegenPerRound = 1;
    [SerializeField] private int energyCostPlay = 1;
    [SerializeField] private int energyCostDiscard = 1;
    
    [SerializeField] private UISliderBarHelper energyBarHelper;

    // State
    public Energy energy;
    public void StartBattle(PlayerData player, EnemyData enemy)
    {
        energy = new(startEnergy, maxEnergy);
        energy.onValueChanged.AddListener(energyBarHelper.SetValue);
    }

    public bool CanAffordPlayHand(int count)
    {
        int cost = count * energyCostPlay;
        if (energy.CurrentValue < cost)
        {
            return false;
        }
        return true;
    }

    public bool CanAffordDiscard()
    {
        if (energy.CurrentValue < energyCostDiscard)
        {
            return false;
        }
        return true;
    }

    public void UseEnergyPlay()
    {
        energy.TryConsume(energyCostPlay);
    }
    public void UseEnergyDiscard()
    {
        energy.TryConsume(energyCostDiscard);

    }
    public void RegenEnergy()
    {
        energy.Add(energyRegenPerRound);
    }

    public void Attack(IHealth entityHealth, float damage)
    {
        entityHealth.TakeDame(damage);
    }
}