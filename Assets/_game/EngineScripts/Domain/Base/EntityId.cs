using System;
using System.Collections.Generic;

/// <summary>
/// Base class for all entity IDs.
/// This class is used to represent the unique identifier of an entity.
/// It is a value object, meaning that two instances with the same value are considered equal.
/// </summary>
public class EntityId : ValueObject
{
    public string Value { get; }

    public EntityId()
    {
        Value = Guid.NewGuid().ToString();
    }

    public EntityId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("EntityId value cannot be null or empty.", nameof(value));
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}