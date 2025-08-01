public interface ICountable
{
    /// <summary>
    /// Gets the count of the item.
    /// </summary>
    int Count { get; }
    /// <summary>
    /// Increases the count of the item by a specified amount.
    /// </summary>
    /// <param name="amount">The amount to increase the count by.</param>
    void IncreaseCount(int amount);
    /// <summary>
    /// Decreases the count of the item by a specified amount.
    /// </summary>
    /// <param name="amount">The amount to decrease the count by.</param>
    void DecreaseCount(int amount);
}