using Microsoft.AspNetCore.Components;
using ProductDataManager.Domain.Aggregates.CategoryAggregate;

namespace ProductDataManager.Components.Pages.Descriptions;

public partial class DescriptionCategories : ComponentBase
{
    [Parameter] public HashSet<Category> Categories { get; set; } = [];
}