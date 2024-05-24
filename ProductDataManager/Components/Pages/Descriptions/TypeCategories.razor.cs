using MudBlazor;
using ProductDataManager.Components.Pages.Descriptions.Model;

namespace ProductDataManager.Components.Pages.Descriptions;

using Domain.Aggregates.DescriptionAggregate;

public partial class TypeCategories
{
    async Task SelectedChanged(bool value, JoinModel join)
    {
        if (value)
        {
            try
            {
                await AddAsync(join);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to add category to description type");
                Snackbar.Add("Failed to add category to description type", Severity.Error);
            }

            Snackbar.Add("Category added to description type", Severity.Success);
        }
        else
        {
            try
            {
                await RemoveAsync(join);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to remove category from description type");
                Snackbar.Add("Failed to remove category from description type", Severity.Error);
            }

            Snackbar.Add("Category removed from description type", Severity.Success);
        }

        await AggregateChanged.InvokeAsync(Aggregate);
    }
    
    async Task SelectAll(bool value)
    {
        if (value)
        {
            try
            {
                foreach (var join in Aggregate.Joins.Where(join => !join.Checked)) await AddAsync(join);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to add categories to description type");
                Snackbar.Add("Failed to add categories to description type", Severity.Error);
            }
        }
        else
        {
            try
            {
                foreach (var join in Aggregate.Joins.Where(join => join.Checked)) await RemoveAsync(join);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to add categories to description type");
                Snackbar.Add("Failed to add categories to description type", Severity.Error);
            }
        }

        await AggregateChanged.InvokeAsync(Aggregate);
    }
    
    async Task RemoveAsync(JoinModel join)
    {
        DbContext.DescriptionTypesCategories.Remove(DbContext.DescriptionTypesCategories.Local.FindEntry(join.Id!.Value)!.Entity);
        join.Update();
    }

    async Task AddAsync(JoinModel join)
    {
        DbContext.DescriptionTypesCategories.Add(new DescriptionTypeCategory
        {
            DescriptionTypeId = Aggregate.Type.Id,
            CategoryId = join.CategoryId
        });
    }
}
