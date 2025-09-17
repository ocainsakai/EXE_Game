using System.Collections.Generic;

/// <summary>
/// Represents an action that an actor can perform during their turn.
/// </summary>
public interface IBattleAction
{
    /// <summary>
    /// Identifier or name of the action.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Actor who performs this action.
    /// </summary>
    IBattleActor Source { get; }

    /// <summary>
    /// Target(s) of the action (could be empty for self-actions).
    /// </summary>
    IEnumerable<IBattleActor> Targets { get; }

    /// <summary>
    /// Check if the action is valid (enough resources, valid targets…).
    /// </summary>
    bool IsValid();

    /// <summary>
    /// Execute the action and produce a battle result.
    /// </summary>
    IBattleResult Execute();
}
