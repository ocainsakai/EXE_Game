/// <summary>
/// Represents a room in the dungeon.
/// </summary>
public sealed class Room
{
    public RoomDefinition RoomDefinition { get; }

    public Room(RoomDefinition roomDefinition)
    {
        RoomDefinition = roomDefinition;
    }
}