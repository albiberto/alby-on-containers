namespace ProductDataManager.Infrastructure.Domain;

using Abstract;

public class CategoryAttrType : Auditable
{
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }

    public AttrType? Type { get; set; }
    public Guid TypeId { get; set; }
}