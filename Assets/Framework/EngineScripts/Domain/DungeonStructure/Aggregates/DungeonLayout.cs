using System;

/// <summary>
/// Defines the immutable layout of a dungeon, including its floors and corridors.
/// </summary>
public sealed class DungeonLayout
{
    /// <summary>
    /// The floors in the dungeon layout.
    /// </summary>
    public Floor[] Floors { get; }

    /// <summary>
    /// The corridors in the dungeon layout.
    /// </summary>
    public Corridor[] Corridors { get; }

    public DungeonLayout(Floor[] floors, Corridor[] corridors)
    {
        Floors = floors ?? throw new ArgumentNullException(nameof(floors), "Floors cannot be null.");
        Corridors = corridors ?? throw new ArgumentNullException(nameof(corridors), "Corridors cannot be null.");
    }
}