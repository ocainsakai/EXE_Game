using System;

/// <summary>
/// Represents a corridor in the dungeon, defined by its start and end points.
/// </summary>
public sealed class Corridor
{
    /// <summary>
    /// The start point of the corridor.
    /// </summary>
    public RoomCoordinate Start { get; }

    /// <summary>
    /// The end point of the corridor.
    /// </summary>
    public RoomCoordinate End { get; }

    public Corridor(RoomCoordinate start, RoomCoordinate end)
    {
        Start = start ?? throw new ArgumentNullException(nameof(start), "Start cannot be null.");
        End = end ?? throw new ArgumentNullException(nameof(end), "End cannot be null.");
    }
}