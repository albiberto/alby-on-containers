namespace ProductDataManager.Infrastructure.Domain;

using Abstract;

public class Category : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public Guid? ParentId { get; set; }
    public Category? Parent { get; set; }

    public ICollection<Category> Categories { get; set; } = new HashSet<Category>();
    public ICollection<CategoryAttrType> CategoryAttrTypes { get; set; } = new HashSet<CategoryAttrType>();
}