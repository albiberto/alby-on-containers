using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.DescriptionAggregate;

public record DescriptionValue(string Name, string Description, Guid DescriptionTypeId, Guid? Id = default) : Entity(Id)
{
    public DescriptionType? DescriptionType { get; private set; }
}