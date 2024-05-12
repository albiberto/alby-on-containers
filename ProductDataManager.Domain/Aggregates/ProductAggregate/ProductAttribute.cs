using ProductDataManager.Domain.SeedWork;
using Attribute = ProductDataManager.Domain.Aggregates.AttributeAggregate.Attribute;

namespace ProductDataManager.Domain.Aggregates.ProductAggregate;

public record ProductAttribute(Guid ProductId, Guid AttributeId, Guid? Id = default) : Entity(Id)
{
    public Product? Product { get; private set; }
    public Attribute? Attribute { get; private set; }
}