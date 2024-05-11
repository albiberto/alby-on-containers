using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.AttributeAggregate;

public record AttributeType(string Name, string Description, Guid? Id = default, bool Mandatory = false, bool Tech = false) : Entity(Id), IAggregateRoot
{
  public string Name { get; private set; } = Name;
  public string Description { get; private set; } = Description;
  
  public ICollection<Attribute> Types { get; } = [];
  
  public void Update(string name, string description)
  {
    Name = name;
    Description = description;
  }
}