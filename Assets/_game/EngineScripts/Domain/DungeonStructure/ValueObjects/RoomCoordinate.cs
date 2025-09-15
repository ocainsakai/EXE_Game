using System.Collections.Generic;

/// <summary>
/// Represents a coordinate in a dungeon room, defined by its floor and room index.
/// </summary>
public sealed class RoomCoordinate : ValueObject
{
    /// <summary>
    /// The floor number of the room.
    /// </summary>
    public int Floor { get; }

    /// <summary>
    /// The index of the room on the specified floor.
    /// </summary>
    public int RoomIndex { get; }

    public RoomCoordinate(int floor, int roomIndex)
    {
        Floor = floor;
        RoomIndex = roomIndex;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Floor;
        yield return RoomIndex;
    }
}