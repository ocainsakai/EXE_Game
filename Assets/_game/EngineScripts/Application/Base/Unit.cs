using System.Threading.Tasks;

/// <summary>
/// Represents a unit of work that does not return a value.
/// </summary>
public readonly struct Unit
{
    public static readonly Unit Default = new();

    public static implicit operator Unit(ValueTask valueTask)
    {
        return Default;
    }

    public static implicit operator Unit(Task task)
    {
        return Default;
    }

    public static implicit operator Unit(Task<Unit> task)
    {
        return Default;
    }
}