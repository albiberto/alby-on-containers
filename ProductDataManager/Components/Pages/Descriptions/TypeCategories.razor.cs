using MudBlazor;
using ProductDataManager.Components.Pages.Descriptions.Model;

namespace ProductDataManager.Components.Pages.Descriptions;

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
        await DescriptionRepository.RemoveCategoryAsync(join.Id!.Value);
        join.Update();
    }

    async Task AddAsync(JoinModel join)
    {
        var entity = await DescriptionRepository.AddCategoryAsync(Aggregate.Type.Id, join.CategoryId);
        join.Update(entity.Id);
    }
}