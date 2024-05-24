using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Components.Pages.Descriptions.Model;
using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;

namespace ProductDataManager.Components.Pages.Descriptions;

using Infrastructure;

public partial class TypeCategories : ComponentBase
{
    [Inject] public required ILogger<TypeCategories> Logger { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    [Inject] public required ProductContext DbContext { get; set; }
    
    [Parameter] public required AggregateModel Aggregate { get; set; }
    [Parameter] public required EventCallback<AggregateModel> AggregateChanged { get; set; }
}
