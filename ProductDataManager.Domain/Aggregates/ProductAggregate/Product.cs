namespace ProductDataManager.Domain.Aggregates.ProductAggregate;

using System.Collections.ObjectModel;
using CategoryAggregate;
using CommunityToolkit.Mvvm.ComponentModel;
using SeedWork;

public partial class Product : Entity
{
    [ObservableProperty] string name;
    [ObservableProperty] string description;
    [ObservableProperty] Guid categoryId;
    [ObservableProperty] Category? category;

    public ObservableCollection<ProductAttribute> ProductsAttributes { get; } = [];
}
