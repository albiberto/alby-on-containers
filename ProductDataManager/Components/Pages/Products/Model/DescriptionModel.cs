using ProductDataManager.Domain.Aggregates.DescriptionAggregate;

namespace ProductDataManager.Components.Pages.Products.Model;

public class DescriptionModel(DescriptionType type)
{
    public string Name { get; } = type.Name;
}