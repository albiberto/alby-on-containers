using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.DescriptionAggregate;

using CommunityToolkit.Mvvm.ComponentModel;

public partial class DescriptionValue : Entity
{
    [ObservableProperty] DescriptionType? descriptionType;
    [ObservableProperty] string value;
    [ObservableProperty] string description;
    [ObservableProperty] Guid descriptionTypeId;
}
