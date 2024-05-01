using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Components.Pages.Descriptions.Model;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;
using ProductDataManager.Enums;

namespace ProductDataManager.Components.Pages.Descriptions;

public partial class Values : ComponentBase
{
    readonly ObservableCollection<ValueModel> values = [];

    protected override void OnParametersSet()
    {
        values.Clear();
        foreach (var value in ValuesModel) values.Add(value);
    }

    async Task AddValueAsync()
    {
        try
        {
            var entity = await Repository.AddValueAsync(TypeId);
            values.Add(new(entity.Value, entity.Description, entity.Id!.Value, status: Status.Added));
            
            Snackbar.Add("Value tracked for insertion", Severity.Info);
        }
        catch(Exception e)
        {
            Logger.LogError(e, "Error while adding description");
            Snackbar.Add("Error while adding description", Severity.Error);
        }
    }
    
    async Task UpdateValueAsync(ValueModel value)
    {
        try
        {
            await Repository.UpdateValueAsync(value.Id, value.Value, value.Description);
            value.Status = value.Status == Status.Added ? Status.Added : Status.Modified;
            
            await ValuesModelChanged.InvokeAsync(values.ToHashSet());
            
            if(value.Status != Status.Added) Snackbar.Add("Value tracked for update", Severity.Info);
        }
        catch(Exception e)
        {
            Logger.LogError(e, "Error while updating description");
            Snackbar.Add("Error while updating description", Severity.Error);
        }
    }

    async Task DeleteValueAsync(ValueModel value)
    {
        try
        {
            if (value.Status != Status.Added)
            {
                await Repository.DeleteValueAsync(value.Id);
                value.Status = Status.Deleted;
                
                await ValuesModelChanged.InvokeAsync(values.ToHashSet());
                
                Snackbar.Add("Value tracked for deletion", Severity.Info);
            }
            else
            {
                await Repository.DeleteValueAsync(value.Id);
                values.Remove(value);
                await ValuesModelChanged.InvokeAsync(values.ToHashSet());
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while deleting description");
            Snackbar.Add("Error while deleting description", Severity.Error);
        }
    }

    async Task Clear(ValueModel value)
    {
        try
        {
            await Repository.Clear<DescriptionValue>(value.Id);
            
            value.Clear();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while clearing description");
            Snackbar.Add("Error while clearing description", Severity.Error);
        }
    }

    void SaveDescriptionTypeAsync()
    {
        try
        {
            foreach (var value in values.ToList())
                if (value.Status == Status.Deleted)
                {
                    values.Remove(value);
                } 
                else value.Reload();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while deleting category");
            Snackbar.Add("Error while deleting category", Severity.Error);
        }
    }
}