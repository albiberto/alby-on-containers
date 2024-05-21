using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.AttributeAggregate;

public record AttributeType(string? Name = default, string? Description = default, Guid? Id = default, bool Tech = false) : Entity(Id), IAggregateRoot
{
  public string Name { get; private set; } = Name ?? string.Empty;
  public string Description { get; private set; } = Description ?? string.Empty;
  
  public ICollection<Attribute> Attributes { get; } = [];
  
  public void Update(string name, string description)
  {
    Name = name;
    Description = description;
  }
}