using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.DescriptionAggregate;

public record DescriptionType(string Name, string Description, Guid? Id = default) : Entity(Id), IAggregateRoot
{
    public ICollection<Category> Categories { get; private set; } = [];
    public ICollection<DescriptionValue> DescriptionValues { get; private set; } = [];
}
