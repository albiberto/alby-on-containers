using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Components.Pages.Attributes.Model;
using ProductDataManager.Domain.Aggregates.AttributeAggregate;
using ProductDataManager.Infrastructure.Specifications;

namespace ProductDataManager.Components.Pages.Attributes;

public partial class AttributeTypes : ComponentBase
{
    AggregatesModel Model { get; set; } = new();
    
    protected override async Task OnInitializedAsync()
    {
        var types = await AttributeRepository.ListAsync(new AttributeSpecification());
        Model = new(types);
    }

    protected override void OnAfterRender(bool firstRender)
    
    {
        if (firstRender) registration = Navigation.RegisterLocationChangingHandler(OnLocationChanging);
    }

    async Task AddAttributeTypeAsync()
    {
        try
        {
            var entity = await AttributeRepository.AddAsync(new());
            Model.AddType(entity.Id!.Value);
            
            Snackbar.Add("Attribute Type tracked for insertion", Severity.Info);
        }
        catch(Exception e)
        {
            Logger.LogError(e, "Error while adding description");
            Snackbar.Add("Error while adding description", Severity.Error);
        }
    }

    async Task UpdateAttributeTypeAsync(AggregateModel aggregate)
    {
        try
        {
            if (aggregate.Type.IsDirty)
            {
                await AttributeRepository.UpdateAsync(aggregate.Type.Id, aggregate.Type.Name, aggregate.Type.Description);
                Model.ModifyType(aggregate);
            }
            else await ClearAsync(aggregate);
            
            if(!aggregate.Type.Status.IsAdded) Snackbar.Add("Attribute type tracked for update", Severity.Info);
        }
        catch(Exception e)
        {
            Logger.LogError(e, "Error while updating description");
            Snackbar.Add("Error while updating description", Severity.Error);
        }
    }

    async Task DeleteAttributeTypeAsync(AggregateModel aggregate)
    {
        try
        {
            await AttributeRepository.DeleteAttributeTypeAsync(aggregate.Type.Id);
            Model.DeleteType(aggregate);    
            
            if(!aggregate.Status.IsAdded) Snackbar.Add("Attribute type tracked for deletion", Severity.Info);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while deleting description");
            Snackbar.Add("Error while deleting description", Severity.Error);
        }
    }
    
    async Task ClearAsync(AggregateModel aggregate)
    {
        try
        {
            await AttributeRepository.Clear<AttributeType>(aggregate.Type.Id);
            aggregate.Type.Clear();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while clearing description");
            Snackbar.Add("Error while clearing description", Severity.Error);
        }
    }

    async Task SaveAsync()
    {
        try
        {
            await AttributeRepository.UnitOfWork.SaveChangesAsync();
            Model.Save();
            
            Snackbar.Add("Changes Saved!", Severity.Success);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while deleting category");
            Snackbar.Add("Error while deleting category", Severity.Error);
        }
    }

    public bool DisableSave => !AttributeRepository.HasChanges || !Model.IsValid;
    public bool DisableClearAll => !AttributeRepository.HasChanges;

    void ClearAll()
    {
        try
        {
            AttributeRepository.Clear();
            Model.Clear();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while clearing all descriptions");
            Snackbar.Add("Error while clearing all descriptions", Severity.Error);
        }
    }
}