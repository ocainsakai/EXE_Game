using System.Collections.Generic;

/// <summary>
/// Represents the outcome of executing an action in a turn-based battle.
/// </summary>
public interface IBattleResult
{
    /// <summary>
    /// The actor who performed the action.
    /// </summary>
    IBattleActor Source { get; }

    /// <summary>
    /// Target(s) affected by the action.
    /// </summary>
    IEnumerable<IBattleActor> Targets { get; }

    /// <summary>
    /// Type of result (damage, heal, buff, debuff, win, lose…).
    /// </summary>
    string ResultType { get; }

    /// <summary>
    /// Numerical value of the result (e.g. damage amount, heal amount).
    /// </summary>
    int Value { get; }

    /// <summary>
    /// Whether this result ends the battle (e.g. victory, defeat).
    /// </summary>
    bool EndsBattle { get; }
}
