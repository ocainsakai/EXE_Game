using System.Collections.Generic;

/// <summary>
/// Represents an actor (unit, player, enemy) that can participate in a turn-based battle.
/// </summary>
public interface IBattleActor
{
    /// <summary>
    /// Unique identifier for the actor.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Display name (for UI/logging).
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Whether the actor is still alive/active in battle.
    /// </summary>
    bool IsAlive { get; }

    /// <summary>
    /// Called when the actor takes a turn.
    /// Should return a list of possible actions (for AI or UI).
    /// </summary>
    IEnumerable<IBattleAction> GetAvailableActions();

    /// <summary>
    /// Apply the result of an action (damage, heal, buff…).
    /// </summary>
    void ApplyResult(IBattleResult result);
}
