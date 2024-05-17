using ProductDataManager.Domain.Aggregates.DescriptionAggregate;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.ProductAggregate;

public record ProductDescription(Guid ProductId, Guid DescriptionId, Guid? Id = default) : Entity(Id)
{
    public Product? Product { get; private set; }
    public DescriptionValue? Description { get; private set; }
}