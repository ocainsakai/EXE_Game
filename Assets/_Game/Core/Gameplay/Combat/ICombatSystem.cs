using CardSystem;
using System.Collections.Generic;

public interface ICombatSystem
{
    // Start combat with a specific enemy
    void StartCombat(PlayerData player, EnemyData enemy);

    // Play a hand of cards (Poker hand)
    CombatResult PlayHand(List<CardData> selectedCards);

    // Apply enemy turn (AI action)
    void EnemyTurn();

    // Check if combat is finished
    bool IsCombatOver();

    // Get combat log (for debug/UI)
    List<string> GetCombatLog();
}
