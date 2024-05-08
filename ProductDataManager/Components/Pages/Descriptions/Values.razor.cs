using System.Collections.Frozen;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Components.Pages.Descriptions.Model;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;

namespace ProductDataManager.Components.Pages.Descriptions;

public partial class Values : ComponentBase
{
    async Task AddValueAsync()
    {
        try
        {
            var entity = await Repository.AddValueAsync(Aggregate.Type.Id);
            Aggregate.AddValue(entity.Id!.Value);
            
            await AggregateChanged.InvokeAsync(Aggregate);
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
            value.Status.Modified();
            
            await AggregateChanged.InvokeAsync(Aggregate);
            if(value.Status.IsModified) Snackbar.Add("Value tracked for update", Severity.Info);
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
            await Repository.DeleteValueAsync(value.Id);
            Aggregate.RemoveValue(value);
            
            await AggregateChanged.InvokeAsync(Aggregate);
            Snackbar.Add("Value tracked for deletion", Severity.Info);
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
            foreach (var value in Aggregate.Values.ToFrozenSet())
                if (value.Status.IsDeleted) Aggregate.RemoveValue(value);
                else value.Save();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while deleting category");
            Snackbar.Add("Error while deleting category", Severity.Error);
        }
    }
}