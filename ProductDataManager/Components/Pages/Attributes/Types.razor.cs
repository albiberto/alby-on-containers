using System.Collections.Frozen;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Components.Pages.Attributes.Model;
using ProductDataManager.Domain.Aggregates.AttributeAggregate;

namespace ProductDataManager.Components.Pages.Attributes;

public partial class Types : ComponentBase
{
     async Task AddValueAsync()
    {
        try
        {
            var entity = await AttributeRepository.AddTypeAsync(Aggregate.Cluster.Id);
            Aggregate.AddType(entity.Id!.Value);
            
            await AggregateChanged.InvokeAsync(Aggregate);
            Snackbar.Add("Value tracked for insertion", Severity.Info);
        }
        catch(Exception e)
        {
            Logger.LogError(e, "Error while adding description");
            Snackbar.Add("Error while adding description", Severity.Error);
        }
    }
    
    async Task UpdateValueAsync(TypeModel value)
    {
        try
        {
            if (value.IsDirty)
            {
                await AttributeRepository.UpdateTypeAsync(value.Id, value.Name, value.Description);
                value.Status.Modified();
            }
            else await ClearAsync(value);
            
            await AggregateChanged.InvokeAsync(Aggregate);
            if(value.Status.IsModified) Snackbar.Add("Value tracked for update", Severity.Info);
        }
        catch(Exception e)
        {
            Logger.LogError(e, "Error while updating description");
            Snackbar.Add("Error while updating description", Severity.Error);
        }
    }

    async Task DeleteValueAsync(TypeModel value)
    {
        try
        {
            await AttributeRepository.DeleteTypeAsync(value.Id);
            Aggregate.RemoveType(value);
            
            await AggregateChanged.InvokeAsync(Aggregate);
            Snackbar.Add("Value tracked for deletion", Severity.Info);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while deleting description");
            Snackbar.Add("Error while deleting description", Severity.Error);
        }
    }

    async Task ClearAsync(TypeModel value)
    {
        try
        {
            await AttributeRepository.Clear<AttributeType>(value.Id);
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
            foreach (var value in Aggregate.Types.ToFrozenSet())
                if (value.Status.IsDeleted) Aggregate.RemoveType(value);
                else value.Save();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while deleting category");
            Snackbar.Add("Error while deleting category", Severity.Error);
        }
    }
}