using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Domain.Aggregates.CategoryAggregate;

namespace ProductDataManager.Components.Pages.Categories;

#nullable enable
using Infrastructure;

public partial class Categories
{
    [Inject] public required IDialogService DialogService { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    [Inject] public required ILogger<Categories> Logger { get; set; }
    [Inject] public required ProductContext DbContext { get; set; }
}
