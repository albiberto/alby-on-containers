using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.ProductAggregate;

public record Product(string Name, string Description, Guid CategoryId, Guid? Id) : Entity(Id), IAggregateRoot
{
    public Category? Category { get; private set; }
    public ICollection<ProductAttribute> ProductsAttributes { get; private set; } = [];
}