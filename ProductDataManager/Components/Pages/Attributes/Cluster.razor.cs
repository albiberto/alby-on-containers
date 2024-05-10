using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Components.Pages.Attributes.Model;
using ProductDataManager.Domain.Aggregates.AttributeAggregate;

namespace ProductDataManager.Components.Pages.Attributes;

public partial class Cluster : ComponentBase
{
    AggregatesModel Model { get; set; } = new();
    
    protected override async Task OnInitializedAsync() => Model = new(await AttributeRepository.GetAllAsync());

    protected override void OnAfterRender(bool firstRender)
    
    {
        if (firstRender) registration = Navigation.RegisterLocationChangingHandler(OnLocationChanging);
    }

    async Task AddClusterAsync()
    {
        try
        {
            var entity = await AttributeRepository.AddAsync();
            Model.Add(entity.Id!.Value);
            
            Snackbar.Add("Cluster tracked for insertion", Severity.Info);
        }
        catch(Exception e)
        {
            Logger.LogError(e, "Error while adding description");
            Snackbar.Add("Error while adding description", Severity.Error);
        }
    }

    async Task UpdateClusterAsync(AggregateModel aggregate)
    {
        try
        {
            if (aggregate.Cluster.IsDirty)
            {
                await AttributeRepository.UpdateAsync(aggregate.Cluster.Id, aggregate.Cluster.Name, aggregate.Cluster.Description);
                Model.Modified(aggregate);
            }
            else await ClearAsync(aggregate);
            
            if(!aggregate.Cluster.Status.IsAdded) Snackbar.Add("Cluster tracked for update", Severity.Info);
        }
        catch(Exception e)
        {
            Logger.LogError(e, "Error while updating description");
            Snackbar.Add("Error while updating description", Severity.Error);
        }
    }

    async Task DeleteClusterAsync(AggregateModel aggregate)
    {
        try
        {
            await AttributeRepository.DeleteAsync(aggregate.Cluster.Id);
            Model.Delete(aggregate);    
            
            if(!aggregate.Status.IsAdded) Snackbar.Add("Cluster tracked for deletion", Severity.Info);
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
            await AttributeRepository.Clear<AttributeCluster>(aggregate.Cluster.Id);
            aggregate.Cluster.Clear();
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