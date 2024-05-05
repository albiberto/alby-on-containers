using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.DescriptionAggregate;

public record DescriptionTypeCategory(Guid DescriptionTypeId, Guid CategoryId, Guid? Id = default) : Entity(Id)
{
    public DescriptionType? DescriptionType { get; private set; }
    public Category? Category { get; private set; }
}