using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Domain.Aggregates.CategoryAggregate;

namespace ProductDataManager.Components.Pages.Categories;

#nullable enable

public partial class Categories
{
    [Inject] public required ICategoryRepository Repository { get; set; }
    [Inject] public required IDialogService DialogService { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    [Inject] public required ILogger<Categories> Logger { get; set; }
}