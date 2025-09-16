using System.Collections.Generic;
/// <summary>
/// Generic battle manager interface for any turn-based game.
/// </summary>
public interface ITurnBasedBattleManager<TActor, TAction, TResult>
{
    /// <summary>
    /// Initialize the battle with given actors (players, enemies, etc).
    /// </summary>
    void InitializeBattle(IEnumerable<TActor> actors);

    /// <summary>
    /// Start the battle loop.
    /// </summary>
    void StartBattle();

    /// <summary>
    /// Start a new turn for the current actor.
    /// </summary>
    TActor StartTurn();

    /// <summary>
    /// Execute an action by the current actor.
    /// </summary>
    TResult ExecuteAction(TAction action);

    /// <summary>
    /// End the current turn and move to the next.
    /// </summary>
    void EndTurn();

    /// <summary>
    /// Check if the battle is finished.
    /// </summary>
    bool IsBattleOver();

    /// <summary>
    /// Get the result of the battle (win/lose/draw or custom type).
    /// </summary>
    TResult GetBattleResult();

    /// <summary>
    /// End the battle manually (e.g., surrender).
    /// </summary>
    void EndBattle();
}
