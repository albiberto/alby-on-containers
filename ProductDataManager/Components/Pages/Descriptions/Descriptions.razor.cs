using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Components.Shared;

namespace ProductDataManager.Components.Pages.Descriptions;

public partial class Descriptions : ComponentBase
{
    ObservableCollection<Data> descriptions = [];

    protected override async Task OnInitializedAsync()
    {
        var types = await Repository.GetAllAsync();
        foreach(var type in types)
            descriptions.Add(new(type));
    }

    async Task AddDescriptionAsync()
    {
        var dialog = await DialogService.ShowAsync<DescriptionDialog>("Add Macro Category");
        var result = await dialog.Result;
        
        var model = (DescriptionDialog.Model)result.Data;
        
        if (!result.Canceled)
            try
            {
                var description = await Repository.AddAsync(model.Name, model.Description);
                await Repository.UnitOfWork.SaveChangesAsync();
                
                descriptions.Add(new(description));
                StateHasChanged();
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error while saving description");
                Snackbar.Add("Error while saving description", Severity.Error);
            }

        Snackbar.Add("Category Added!", Severity.Success);
    }
}