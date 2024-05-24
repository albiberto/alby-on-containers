using ProductDataManager.Domain.Aggregates.ProductAggregate;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.AttributeAggregate;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class Attribute : Entity
{
    [ObservableProperty] string name;
    [ObservableProperty] string description;
    [ObservableProperty] AttributeType? type;
    [ObservableProperty] Guid typeId;
    
    public ObservableCollection<ProductAttribute> ProductsAttributes { get; } = [];
}
