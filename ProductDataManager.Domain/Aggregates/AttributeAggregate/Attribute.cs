using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.AttributeAggregate;

public record Attribute(string Name, string Description, Guid TypeId, Guid? Id = default) : Entity(Id)
{
    public string Name { get; private set; } = Name;
    public string Description { get; private set; } = Description;
    
    public AttributeType? Type { get; private set; }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }
}