using Microsoft.AspNetCore.Components;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;

namespace ProductDataManager.Components.Pages.Descriptions;

public partial class DescriptionValues : ComponentBase
{
    [Parameter] public HashSet<DescriptionValue> Values { get; set; } = [];
}