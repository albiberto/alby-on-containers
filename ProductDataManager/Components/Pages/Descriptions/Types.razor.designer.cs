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

public partial class Types : IDisposable
{
    [Inject] public required IDescriptionRepository DescriptionRepository { get; set; }
    [Inject] public required ICategoryRepository CategoryRepository { get; set; }
    [Inject] public required IDialogService DialogService { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    [Inject] public required ILogger<Types> Logger { get; set; }
    [Inject] public required NavigationManager Navigation { get; set; }
    
    IDisposable? registration;
    
    async ValueTask OnLocationChanging(LocationChangingContext context)
    {
        if(!DescriptionRepository.HasChanges) return;
        
        var dialog = await DialogService.ShowAsync<NavigationDialog>("Leave page?", Constants.DialogOptions);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            DescriptionRepository.Clear();
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