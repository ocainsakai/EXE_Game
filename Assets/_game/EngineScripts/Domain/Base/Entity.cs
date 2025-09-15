using System;
using System.Collections.Generic;

/// <summary>
/// Base class for all entities.
/// An entity is an object that has a distinct identity that runs through its lifecycle.
/// </summary>
/// <typeparam name="TId">The type of the entity's identifier.</typeparam>
public abstract class Entity<TId> where TId : EntityId
{
    /// <summary>
    /// The unique identifier of the entity.
    /// </summary>
    public TId Id { get; }

    protected Entity(TId id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }

    public override bool Equals(object obj)
    {
        if (obj == null || obj is not Entity<TId>) return false;

        if (ReferenceEquals(this, obj)) return true;

        var other = (Entity<TId>)obj;

        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<TId>.Default.GetHashCode(Id);
    }
}