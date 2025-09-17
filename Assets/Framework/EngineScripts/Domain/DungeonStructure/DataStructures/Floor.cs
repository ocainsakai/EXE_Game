using System;

/// <summary>
/// Represents a floor in the dungeon, containing rooms.
/// </summary>
public sealed class Floor
{
    public Room[] Rooms { get; }

    public Floor(Room[] rooms)
    {
        Rooms = rooms ?? throw new ArgumentNullException(nameof(rooms));
    }
}