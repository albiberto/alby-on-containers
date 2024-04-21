namespace ProductDataManager.Infrastructure.Domain;

using Abstract;

public record CategoryAttrType(Guid CategoryId, Guid TypeId) : Auditable
{
    public Category? Category { get; set; }
    public AttrType? Type { get; set; }
}