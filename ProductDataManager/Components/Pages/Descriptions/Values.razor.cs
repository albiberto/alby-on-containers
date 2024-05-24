using System.Collections.Frozen;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Components.Pages.Descriptions.Model;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;

namespace ProductDataManager.Components.Pages.Descriptions;

using Infrastructure;

public partial class Values : ComponentBase
{
    async Task AddValueAsync()
    {
        try
        {
            DbContext.DescriptionValues.Add(new DescriptionValue
            {
                Description = "",
                DescriptionTypeId = Aggregate.Type.Id
            });
            
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
       
    }

    async Task DeleteValueAsync(ValueModel value)
    {
        try
        {
            DbContext.DescriptionValues.Remove(DbContext.DescriptionValues.Local.FindEntry(value.Id)!.Entity);
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

    async Task ClearAsync(ValueModel value)
    {
        try
        {
            DbContext.DescriptionValues.Local.FindEntry(value.Id)!.DiscardChanges();
            value.Clear();
            
            await AggregateChanged.InvokeAsync(Aggregate);
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
