namespace ProductDataManager.Domain.SeedWork;

using CommunityToolkit.Mvvm.ComponentModel;

[INotifyPropertyChanged]
public abstract partial class Entity : Auditable
{
    [ObservableProperty] Guid id;
}
