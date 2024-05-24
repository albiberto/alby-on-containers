using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.DescriptionAggregate;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class DescriptionType : Entity
{
    [ObservableProperty] string name;
    [ObservableProperty] string description;
    [ObservableProperty] bool mandatory;
    
    public ObservableCollection<DescriptionTypeCategory> DescriptionTypesCategories { get; } = [];
    public ObservableCollection<DescriptionValue> DescriptionValues { get; } = []; 
}
