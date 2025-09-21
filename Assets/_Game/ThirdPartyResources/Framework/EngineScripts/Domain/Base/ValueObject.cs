using System.Collections.Generic;

/// <summary>
/// ValueObject is a base class for creating value objects.
/// Value objects are immutable and are compared based on their values.
/// This class provides a mechanism for comparing value objects based on their properties.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// GetEqualityComponents should return an enumerable collection of objects that represent the properties of the value object.
    /// These properties will be used to determine equality between value objects.
    /// </summary>
    /// <returns>An enumerable collection of objects representing the properties of the value object.</returns>
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != GetType()) return false;

        var other = (ValueObject)obj;
        using (var thisComponents = GetEqualityComponents().GetEnumerator())
        using (var otherComponents = other.GetEqualityComponents().GetEnumerator())
        {
            while (thisComponents.MoveNext() && otherComponents.MoveNext())
            {
                if (ReferenceEquals(thisComponents.Current, null) ^ ReferenceEquals(otherComponents.Current, null)) return false;
                if (thisComponents.Current != null && !thisComponents.Current.Equals(otherComponents.Current)) return false;
            }

            return !thisComponents.MoveNext() && !otherComponents.MoveNext();
        }
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            foreach (var obj in GetEqualityComponents())
                hash = hash * 23 + (obj?.GetHashCode() ?? 0);
            return hash;
        }
    }
}