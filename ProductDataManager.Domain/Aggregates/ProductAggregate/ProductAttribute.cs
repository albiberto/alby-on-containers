
namespace ProductDataManager.Domain.Aggregates.ProductAggregate;

using CommunityToolkit.Mvvm.ComponentModel;
using SeedWork;
using Attribute = AttributeAggregate.Attribute;

public partial class ProductAttribute : Entity
{
    [ObservableProperty] Guid productId;
    [ObservableProperty] Guid attributeId;
    [ObservableProperty] Product? product;
    [ObservableProperty] Attribute? attribute;
}
