using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.DescriptionAggregate;

public record DescriptionValue(string Value, string Description, Guid DescriptionTypeId, Guid? Id = default) : Entity(Id)
{
    public DescriptionType? DescriptionType { get; private set; }
    public string Value { get; set; } = Value;
    public string Description { get; set; } = Description;

    public void Update(string value, string description)
    {
        Value = value;
        Description = description;
    }
}