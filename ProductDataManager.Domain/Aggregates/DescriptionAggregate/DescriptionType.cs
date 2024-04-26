using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.DescriptionAggregate;

public record DescriptionType(string Name, string Description, Guid? Id = default) : Entity(Id), IAggregateRoot
{
    public string Name { get; private set; } = Name;
    public string Description { get; private set; } = Description;
    
    public ICollection<Category> Categories { get; private set; } = [];
    public ICollection<DescriptionValue> DescriptionValues { get; private set; } = [];
    
    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
