namespace AlbyOnContainers.ProductDataManager.Models.Abstract;

using System;
using System.ComponentModel.DataAnnotations;

public abstract class Entity : Auditable, IEquatable<Entity>
{
    [Required]
    public Guid? Id { get; set; }

    [Required]
    [ConcurrencyCheck]
    public string Name { get; set; }
    
    [Required]
    [ConcurrencyCheck]
    public string Description { get; set; }
    
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