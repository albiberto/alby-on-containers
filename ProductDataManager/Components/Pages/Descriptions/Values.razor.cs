using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Components.Pages.Descriptions.Model;
using ProductDataManager.Components.Shared;
using ProductDataManager.Components.Shared.Dialogs;

namespace ProductDataManager.Components.Pages.Descriptions;

public partial class Values : ComponentBase
{
    [Parameter] public required HashSet<Value> Items { get; set; } = [];
    [Parameter] public required Guid TypeId { get; set; } 

    readonly ObservableCollection<Value> values = [];

    protected override void OnParametersSet()
    {
        foreach(var value in Items) values.Add(value);
    }

    async Task AddDescriptionValueAsync()
    {
        var dialog = await DialogService.ShowAsync<NavigationDialog>("Add Value");
        var result = await dialog.Result;
        
        // var model = (NavigationDialog.Model)result.Data;
        //
        // if (!result.Canceled)
        //     try
        //     {
        //         var description = await Repository.AddValueAsync(model.Name, model.Description, TypeId);
        //         await Repository.UnitOfWork.SaveChangesAsync();
        //
        //         Snackbar.Add("Description Added!", Severity.Success);
        //
        //         values.Add(new(description.Id!.Value, description.Name, description.Description));
        //     }
        //     catch (Exception e)
        //     {
        //         Logger.LogError(e, "Error while saving description");
        //         Snackbar.Add("Error while saving description", Severity.Error);
        //     }
    }
}