using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Components.Pages.Attributes.Model;
using ProductDataManager.Domain.Aggregates.AttributeAggregate;

namespace ProductDataManager.Components.Pages.Attributes;

public partial class AttributeTypes : ComponentBase
{
    AggregatesModel Model { get; set; } = new();
    
    protected override async Task OnInitializedAsync() => Model = new(await AttributeRepository.GetAllAsync());

    protected override void OnAfterRender(bool firstRender)
    
    {
        if (firstRender) registration = Navigation.RegisterLocationChangingHandler(OnLocationChanging);
    }

    async Task AddAttrTypeAsync()
    {
        try
        {
            var entity = await AttributeRepository.AddAttributeTypeAsync();
            Model.Add(entity.Id!.Value);
            
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
            if (aggregate.AttributeType.IsDirty)
            {
                await AttributeRepository.UpdateAttributeTypeAsync(aggregate.AttributeType.Id, aggregate.AttributeType.Name, aggregate.AttributeType.Description);
                Model.Modified(aggregate);
            }
            else await ClearAsync(aggregate);
            
            if(!aggregate.AttributeType.Status.IsAdded) Snackbar.Add("Attribute type tracked for update", Severity.Info);
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
            await AttributeRepository.DeleteAttributeTypeAsync(aggregate.AttributeType.Id);
            Model.Delete(aggregate);    
            
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
            await AttributeRepository.Clear<AttributeType>(aggregate.AttributeType.Id);
            aggregate.AttributeType.Clear();
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