using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.DescriptionAggregate;

public record DescriptionType(string Name, string Description, bool Mandatory, Guid? Id = default) : Entity(Id), IAggregateRoot
{
    public string Name { get; private set; } = Name;
    public string Description { get; private set; } = Description;
    public bool Mandatory { get; private set; } = Mandatory;
    
    public ICollection<DescriptionTypeCategory> DescriptionTypesCategories { get; private set; } = [];
    public ICollection<DescriptionValue> DescriptionValues { get; private set; } = [];

    public void Update(string name, string description, bool mandatory)
    {
        Name = name;
        Description = description;
        Mandatory = mandatory;
    }
}
