using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Components.Pages.Descriptions.Model;
using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;

namespace ProductDataManager.Components.Pages.Descriptions;

public partial class TypeCategories : ComponentBase
{
    [Inject] public required ILogger<TypeCategories> Logger { get; set; }
    [Inject] public required ICategoryRepository CategoryRepository { get; set; }
    [Inject] public required IDescriptionRepository DescriptionRepository { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    
    [Parameter] public required Guid TypeId { get; set; }
    [Parameter] public required HashSet<CategoryModel> CategoryModel { get; set; }
    [Parameter] public required EventCallback<HashSet<CategoryModel>> CategoryModelChanged { get; set; }

    bool all;
}