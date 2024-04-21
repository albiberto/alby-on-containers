namespace ProductDataManager.Infrastructure.Domain;

using Abstract;

public record AttrType(string Name, string Description, Guid? Id) : Entity(Id)
{
    public ICollection<CategoryAttrType> CategoryAttrTypes { get; set; } = new HashSet<CategoryAttrType>();
}