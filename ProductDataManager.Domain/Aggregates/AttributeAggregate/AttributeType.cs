using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.AttributeAggregate;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class AttributeType : Entity
{
  [ObservableProperty] string name;
  [ObservableProperty] string description;
  [ObservableProperty] bool tech;
  
  public ObservableCollection<Attribute> Attributes { get; } = [];
}
