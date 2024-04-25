using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.CategoryAggregate;

public record Category(string Name, string Description, Guid? ParentId = default, Guid? Id = default) : Entity(Id), IAggregateRoot
{
    public string Name { get; private set; } = Name;
    public string Description { get; private set; } = Description;
    public Guid? ParentId { get; private set; } = ParentId;

    public Category? Parent { get; private set; }
    
    public ICollection<Category> Categories { get; private set; } = [];

    public void Update(string name, string description, Guid? parentId)
    {
        Name = name;
        Description = description;
        ParentId = parentId;
    }
}