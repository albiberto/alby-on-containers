using System.Collections.Frozen;

namespace ProductDataManager.Infrastructure.Domain;

using Abstract;

public record Category(string Name, string Description, Guid? ParentId = default, Guid? Id = default) : Entity(Id)
{
    public string Name { get; private set; } = Name;
    public string Description { get; private set; } = Description;
    public Guid? ParentId { get; private set; } = ParentId;

    public Category? Parent { get; private set; }
    
    public HashSet<Category> Categories { get; } = [];
    public HashSet<CategoryAttrType> CategoryAttrTypes { get; } = [];

    public void Update(string name, string description, Guid? parentId)
    {
        Name = name;
        Description = description;
        ParentId = parentId;
    }
}