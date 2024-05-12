using System.Collections.Frozen;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Components.Pages.Attributes.Model;
using ProductDataManager.Domain.Aggregates.AttributeAggregate;
using Attribute = ProductDataManager.Domain.Aggregates.AttributeAggregate.Attribute;

namespace ProductDataManager.Components.Pages.Attributes;

public partial class Attributes : ComponentBase
{
     async Task AddAttributeAsync()
    {
        try
        {
            var entity = await AttributeRepository.AddAttributeAsync(Aggregate.Type.Id);
            Aggregate.AddAttribute(entity.Id!.Value);
            
            await AggregateChanged.InvokeAsync(Aggregate);
            Snackbar.Add("Attribute tracked for insertion", Severity.Info);
        }
        catch(Exception e)
        {
            Logger.LogError(e, "Error while adding description");
            Snackbar.Add("Error while adding description", Severity.Error);
        }
    }
    
    async Task UpdateAttributeAsync(AttributeModel attribute)
    {
        try
        {
            if (attribute.IsDirty)
            {
                await AttributeRepository.UpdateAttributeAsync(attribute.Id, attribute.Name, attribute.Description, attribute.TypeId);
                attribute.Status.Modified();
            }
            else await ClearAsync(attribute);
            
            await AggregateChanged.InvokeAsync(Aggregate);
            if(attribute.Status.IsModified) Snackbar.Add("Attribute tracked for update", Severity.Info);
        }
        catch(Exception e)
        {
            Logger.LogError(e, "Error while updating description");
            Snackbar.Add("Error while updating description", Severity.Error);
        }
    }

    async Task DeleteAttributeAsync(AttributeModel attribute)
    {
        try
        {
            await AttributeRepository.DeleteAttributeAsync(attribute.Id);
            Aggregate.RemoveAttribute(attribute);
            
            await AggregateChanged.InvokeAsync(Aggregate);
            Snackbar.Add("Attribute tracked for deletion", Severity.Info);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while deleting description");
            Snackbar.Add("Error while deleting description", Severity.Error);
        }
    }

    async Task ClearAsync(AttributeModel attribute)
    {
        try
        {
            await AttributeRepository.Clear<Attribute>(attribute.Id);
            attribute.Clear();
            
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
            foreach (var attribute in Aggregate.Attributes.ToFrozenSet())
                if (attribute.Status.IsDeleted) Aggregate.RemoveAttribute(attribute);
                else attribute.Save();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while deleting category");
            Snackbar.Add("Error while deleting category", Severity.Error);
        }
    }
}