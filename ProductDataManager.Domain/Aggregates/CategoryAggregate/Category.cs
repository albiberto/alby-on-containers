using ProductDataManager.Domain.Aggregates.DescriptionAggregate;
using ProductDataManager.Domain.Aggregates.ProductAggregate;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.CategoryAggregate;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;


public partial class Category : Entity
{
    [ObservableProperty] string name;
    [ObservableProperty] string description;
    [ObservableProperty] Guid? parentId;
    [ObservableProperty] Category? parent;
    
    public ObservableCollection<Category> Categories { get; } = [];
    public ObservableCollection<DescriptionTypeCategory> DescriptionTypesCategories { get; } = [];
    public ObservableCollection<Product> Products { get; } = [];
}
