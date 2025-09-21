/// <summary>
/// A factory interface for creating dungeon layouts.
/// This interface is responsible for generating the layout of a dungeon, including its floors and corridors.
/// </summary>
public interface IDungeonLayoutFactory
{
    /// <summary>
    /// Creates a new dungeon layout.
    /// </summary>
    /// <returns>A new instance of <see cref="DungeonLayout"/>.</returns>
    DungeonLayout CreateDungeonLayout();
}