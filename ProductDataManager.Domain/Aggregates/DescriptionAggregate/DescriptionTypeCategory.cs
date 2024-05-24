using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.DescriptionAggregate;

using CommunityToolkit.Mvvm.ComponentModel;

public partial class DescriptionTypeCategory : Entity
{
    [ObservableProperty] Guid descriptionTypeId ;
    [ObservableProperty] Guid categoryId;
    DescriptionType? descriptionType;
    Category? category;
}
