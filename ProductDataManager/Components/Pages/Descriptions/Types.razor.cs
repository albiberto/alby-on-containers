using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using ProductDataManager.Components.Pages.Descriptions.Model;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;
using ProductDataManager.Enums;

namespace ProductDataManager.Components.Pages.Descriptions;

public partial class Types : ComponentBase
{
    readonly ObservableCollection<TypeModel> types = [];

    protected override async Task OnInitializedAsync()
    {
        var types = await Repository.GetAllAsync();
        foreach (var type in types)
        {
            var state = type.Id.HasValue ? (await Repository.GetStateAsync<DescriptionType>(type.Id.Value)).Map() : Status.Added;
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
            
            Snackbar.Add("Type tracked for insertion", Severity.Info);
        }
        catch(Exception e)
        {
            Logger.LogError(e, "Error while adding description");
            Snackbar.Add("Error while adding description", Severity.Error);
        }
    }

    async Task UpdateTypeAsync(TypeModel typeModel)
    {
        try
        {
            await Repository.UpdateAsync(typeModel.Id, typeModel.Name, typeModel.Description);
            typeModel.Status = typeModel.Status == Status.Added ? Status.Added : Status.Modified;
            
            if(typeModel.Status != Status.Added) Snackbar.Add("Type tracked for update", Severity.Info);
        }
        catch(Exception e)
        {
            Logger.LogError(e, "Error while updating description");
            Snackbar.Add("Error while updating description", Severity.Error);
        }
    }

    async Task DeleteTypeAsync(TypeModel typeModel)
    {
        try
        {
            if(typeModel.Status == Status.Added) types.Remove(typeModel);
            else
            {
                await Repository.DeleteAsync(typeModel.Id);
                typeModel.Status = Status.Deleted;
                
                Snackbar.Add("Type tracked for deletion", Severity.Info);
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

    async Task Clear(TypeModel data)
    {
        try
        {
            await Repository.Clear<DescriptionType>(data.Id);
            
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