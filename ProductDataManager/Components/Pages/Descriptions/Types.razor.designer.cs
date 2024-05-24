using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;
using ProductDataManager.Components.Shared;
using ProductDataManager.Components.Shared.Dialogs;
using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;

namespace ProductDataManager.Components.Pages.Descriptions;

#nullable enable
using Infrastructure;

public partial class Types : IDisposable
{
    [Inject] public required IDialogService DialogService { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    [Inject] public required ILogger<Types> Logger { get; set; }
    [Inject] public required NavigationManager Navigation { get; set; }
    [Inject] public required ProductContext DbContext { get; set; }
    
    IDisposable? registration;
    
    async ValueTask OnLocationChanging(LocationChangingContext context)
    {
        if(!DbContext.HasChanges.Value) return;
        
        var dialog = await DialogService.ShowAsync<NavigationDialog>("Leave page?", Constants.DialogOptions);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            DbContext.DiscardChanges();
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
