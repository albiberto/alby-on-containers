using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Components.Pages.Descriptions.Model;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;
using ProductDataManager.Enums;

namespace ProductDataManager.Components.Pages.Descriptions;

public partial class Values : ComponentBase
{
    readonly ObservableCollection<ValueModel> values = [];

    protected override void OnParametersSet()
    {
        foreach (var value in ValuesModel) values.Add(value);
    }

    async Task AddValueAsync()
    {
        try
        {
            var entity = await Repository.AddValueAsync(TypeId);
            values.Add(new(entity.Value, entity.Description, entity.Id!.Value, status: Status.Added));
            
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
        try
        {
            await Repository.UpdateValueAsync(value.Id, value.Value, value.Description);
            value.Status = value.Status == Status.Added ? Status.Added : Status.Modified;
            
            if(value.Status != Status.Added) Snackbar.Add("Value tracked for update", Severity.Info);
        }
        catch(Exception e)
        {
            Logger.LogError(e, "Error while updating description");
            Snackbar.Add("Error while updating description", Severity.Error);
        }
    }

    async Task DeleteValueAsync(ValueModel value)
    {
        try
        {
            if(value.Status == Status.Added) values.Remove(value);
            else
            {
                await Repository.DeleteValueAsync(value.Id);
                value.Status = Status.Deleted;
                
                Snackbar.Add("Value tracked for deletion", Severity.Info);
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while deleting description");
            Snackbar.Add("Error while deleting description", Severity.Error);
        }
    }

    async Task Clear(ValueModel value)
    {
        try
        {
            await Repository.Clear<DescriptionType>(value.Id);
            
            value.Clear();
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
            
            foreach (var description in values.ToList())
                if(description.Status == Status.Added)
                    values.Remove(description);
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