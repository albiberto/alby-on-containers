using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Components.Pages.Descriptions.Model;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;

namespace ProductDataManager.Components.Pages.Descriptions;

public partial class Types : ComponentBase
{
    AggregatesModel Model { get; set; } = new();
    protected override async Task OnInitializedAsync() => Model = new(await DescriptionRepository.GetAllAsync(), await CategoryRepository.GetAllAsync());

    protected override void OnAfterRender(bool firstRender)
    
    {
        if (firstRender) registration = Navigation.RegisterLocationChangingHandler(OnLocationChanging);
    }

    async Task AddTypeAsync()
    {
        try
        {
            var entity = await DescriptionRepository.AddAsync();
            Model.Add(entity.Id!.Value);
            
            Snackbar.Add("Type tracked for insertion", Severity.Info);
        }
        catch(Exception e)
        {
            Logger.LogError(e, "Error while adding description");
            Snackbar.Add("Error while adding description", Severity.Error);
        }
    }

    async Task UpdateTypeAsync(AggregateModel aggregate)
    {
        try
        {
            if (aggregate.Type.IsDirty)
            {
                await DescriptionRepository.UpdateAsync(aggregate.Type.Id, aggregate.Type.Name, aggregate.Type.Description, aggregate.Type.Mandatory);
                Model.Modified(aggregate);
            }
            else await ClearAsync(aggregate);
            
            if(!aggregate.Type.Status.IsAdded) Snackbar.Add("Type tracked for update", Severity.Info);
        }
        catch(Exception e)
        {
            Logger.LogError(e, "Error while updating description");
            Snackbar.Add("Error while updating description", Severity.Error);
        }
    }

    async Task DeleteTypeAsync(AggregateModel aggregate)
    {
        try
        {
            await DescriptionRepository.DeleteAsync(aggregate.Type.Id);
            Model.Delete(aggregate);    
            
            if(!aggregate.Status.IsAdded) Snackbar.Add("Type tracked for deletion", Severity.Info);
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
            await DescriptionRepository.Clear<DescriptionType>(aggregate.Type.Id);
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
            await DescriptionRepository.UnitOfWork.SaveChangesAsync();
            Model.Save();
            
            Snackbar.Add("Changes Saved!", Severity.Success);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while deleting category");
            Snackbar.Add("Error while deleting category", Severity.Error);
        }
    }

    public bool DisableSave => !DescriptionRepository.HasChanges || !Model.IsValid;
    public bool DisableClearAll => !DescriptionRepository.HasChanges;

    void ClearAll()
    {
        try
        {
            DescriptionRepository.Clear();
            Model.Clear();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while clearing all descriptions");
            Snackbar.Add("Error while clearing all descriptions", Severity.Error);
        }
    }
}