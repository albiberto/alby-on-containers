namespace ProductDataManager.Infrastructure.Domain;

using Abstract;

public class CategoryAttrType : Auditable
{
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = new();

    public AttrType Type { get; set; } = new();
    public Guid TypeId { get; set; }
}