using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Components.Pages.Descriptions.Model;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;

namespace ProductDataManager.Components.Pages.Descriptions;

using Infrastructure;
using Microsoft.EntityFrameworkCore;

public partial class Types : ComponentBase
{
    AggregatesModel Model { get; set; } = new();
    protected override async Task OnInitializedAsync() => Model = new(await DbContext.DescriptionTypes.ToListAsync(), await DbContext.Categories.ToListAsync());

    protected override void OnAfterRender(bool firstRender)
    
    {
        if (firstRender) registration = Navigation.RegisterLocationChangingHandler(OnLocationChanging);
    }

    async Task AddTypeAsync()
    {
        try
        {
            var entity = new DescriptionType
            {
                Name = "",
                Description = ""
            };
            DbContext.DescriptionTypes.Add(entity);
            Model.Add(entity.Id);
            
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
      
    }

    async Task DeleteTypeAsync(AggregateModel aggregate)
    {
        try
        {
            DbContext.DescriptionTypes.Remove(DbContext.DescriptionTypes.Local.FindEntry(aggregate.Type.Id)!.Entity);
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
            DbContext.DescriptionTypes.Local.FindEntry(aggregate.Type.Id)!.DiscardChanges();
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
            await DbContext.SaveChangesAsync();
            Model.Save();
            
            Snackbar.Add("Changes Saved!", Severity.Success);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while deleting category");
            Snackbar.Add("Error while deleting category", Severity.Error);
        }
    }

    public bool DisableSave => !DbContext.HasChanges.Value || !Model.IsValid;
    public bool DisableClearAll => !DbContext.HasChanges.Value;

    void ClearAll()
    {
        try
        {
            DbContext.DiscardChanges();
            Model.Clear();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while clearing all descriptions");
            Snackbar.Add("Error while clearing all descriptions", Severity.Error);
        }
    }
}
