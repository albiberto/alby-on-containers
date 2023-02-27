namespace AlbyOnContainers.ProductDataManager.Models.Abstract;

using System;

public abstract class Entity : Auditable, IEquatable<Entity>
{
    public Guid? Id { get; set; }

    public bool Equals(Entity other)
    {
        if (ReferenceEquals(null, other)) return false;
        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Entity)obj);
    }

    public override int GetHashCode() => Id.GetHashCode();
}