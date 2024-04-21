namespace ProductDataManager.Infrastructure.Domain;

using Abstract;

public record Category(string Name, string Description, Guid? ParentId = default, Guid? Id = default) : Entity(Id)
{
    public Category? Parent { get; set; }

    public ICollection<Category> Categories { get; set; } = new HashSet<Category>();
    public ICollection<CategoryAttrType> CategoryAttrTypes { get; set; } = new HashSet<CategoryAttrType>();
}