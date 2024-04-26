using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using ProductDataManager.Enums;
using Type = ProductDataManager.Components.Pages.Descriptions.Model.Type;

namespace ProductDataManager.Components.Pages.Descriptions;

public partial class Types : ComponentBase
{
    readonly ObservableCollection<Type> types = [];

    protected override async Task OnInitializedAsync()
    {
        var types = await Repository.GetAllAsync();
        foreach (var type in types)
        {
            var state = type.Id.HasValue ? (await Repository.GetStateAsync(type.Id.Value)).Map() : Status.Added;
            this.types.Add(new(type, state));
        }
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender) registration = Navigation.RegisterLocationChangingHandler(OnLocationChanging);
    }

    async Task AddTypeAsync()
    {
        try
        {
            var entity = await Repository.AddAsync();
            types.Add(new(entity.Name, entity.Description, entity.Id!.Value, status: Status.Added));
            
            Snackbar.Add("Entity tracked for insertion", Severity.Info);
        }
        catch(Exception e)
        {
            Logger.LogError(e, "Error while adding description");
            Snackbar.Add("Error while adding description", Severity.Error);
        }
    }

    async Task UpdateTypeAsync(Type type)
    {
        try
        {
            await Repository.UpdateAsync(type.Id, type.Name, type.Description);
            type.Status = type.Status == Status.Added ? Status.Added : Status.Modified;
            
            if(type.Status != Status.Added) Snackbar.Add("Entity tracked for update", Severity.Info);
        }
        catch(Exception e)
        {
            Logger.LogError(e, "Error while updating description");
            Snackbar.Add("Error while updating description", Severity.Error);
        }
    }

    async Task DeleteTypeAsync(Type type)
    {
        try
        {
            if(type.Status == Status.Added) types.Remove(type);
            else
            {
                await Repository.DeleteAsync(type.Id);
                type.Status = Status.Deleted;
                
                Snackbar.Add("Entity tracked for deletion", Severity.Info);
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while deleting description");
            Snackbar.Add("Error while deleting description", Severity.Error);
        }
    }

    async Task SaveDescriptionTypeAsync()
    {
        try
        {
            await Repository.UnitOfWork.SaveChangesAsync();
            
            foreach (var type in types.ToList()) 
                if(type.Status == Status.Deleted) 
                    types.Remove(type);
                else
                    type.Reload();
            
            Snackbar.Add("Changes Saved!", Severity.Success);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while deleting category");
            Snackbar.Add("Error while deleting category", Severity.Error);
        }
    }

    async Task Clear(Type data)
    {
        try
        {
            await Repository.Clear(data.Id);
            
            data.Clear();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while clearing description");
            Snackbar.Add("Error while clearing description", Severity.Error);
        }
    }

    void ClearAll()
    {
        try
        {
            Repository.Clear();
            
            foreach (var description in types.ToList())
                if(description.Status == Status.Added)
                    types.Remove(description);
                else
                    description.Clear();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while clearing all descriptions");
            Snackbar.Add("Error while clearing all descriptions", Severity.Error);
        }
    }
}