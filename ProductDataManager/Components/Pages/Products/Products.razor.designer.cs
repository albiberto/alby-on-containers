#nullable enable

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;
using ProductDataManager.Components.Pages.Descriptions;
using ProductDataManager.Components.Shared;
using ProductDataManager.Components.Shared.Dialogs;
using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;
using ProductDataManager.Domain.Aggregates.ProductAggregate;

namespace ProductDataManager.Components.Pages.Products;

public partial class Products : IDisposable
{
    [Inject] public required IProductRepository ProductRepository { get; set; }
    [Inject] public required ICategoryRepository CategoryRepository { get; set; }
    [Inject] public required IDialogService DialogService { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    [Inject] public required ILogger<Types> Logger { get; set; }
    [Inject] public required NavigationManager Navigation { get; set; }

    IDisposable? registration;

    async ValueTask OnLocationChanging(LocationChangingContext context)
    {
        if (!ProductRepository.HasChanges) return;

        var dialog = await DialogService.ShowAsync<NavigationDialog>("Leave page?", Constants.DialogOptions);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            ProductRepository.Clear();
            return;
        }

        context.PreventNavigation();
    }

    public void Dispose()
    {
        Snackbar.Dispose();
        registration?.Dispose();
    }
}