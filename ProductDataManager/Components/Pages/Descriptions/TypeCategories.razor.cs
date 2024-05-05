using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Components.Pages.Descriptions.Model;
using ProductDataManager.Domain.Aggregates.CategoryAggregate;

namespace ProductDataManager.Components.Pages.Descriptions;

public partial class TypeCategories : ComponentBase
{
    HashSet<CategoryModel> Categories { get; set; } = [];


    protected override async Task OnInitializedAsync()
    {
        var categories = await CategoryRepository.GetAllAsync();
        Categories = categories
            .Where(category => !CategoryModel.Select(model => model.CategoryId).Contains(category.Id!.Value))
            .Select(category => new CategoryModel(default, category.Id!.Value, category.Name)).ToHashSet()
            .Concat(CategoryModel)
            .ToHashSet();
        
        all = CategoryModel.Count == Categories.Count;
    }

    async Task SelectAll(bool value)
    {
        all = value;

        if (value)
        {

            try
            {
                await BulkAddJoinAsync(Categories.Where(category => !CategoryModel.Contains(category)));
                CategoryModel = Categories.Select(category => category).ToHashSet();
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
                BulkRemoveJoin(Categories.Where(category => CategoryModel.Contains(category)));
                CategoryModel.Clear();
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to add categories to description type");
                Snackbar.Add("Failed to add categories to description type", Severity.Error);
            }
        }

        await CategoryModelChanged.InvokeAsync(CategoryModel);
    }

    async Task SelectedChanged(bool value, CategoryModel model)
    {
        if (value)
        {
            try
            {
                await AddJoinAsync(model);
                CategoryModel.Add(model);
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
                RemoveJoin(model);
                CategoryModel.Remove(model);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to remove category from description type");
                Snackbar.Add("Failed to remove category from description type", Severity.Error);
            }

            Snackbar.Add("Category removed from description type", Severity.Success);
        }

        all = CategoryModel.Count == Categories.Count;
        await CategoryModelChanged.InvokeAsync(CategoryModel);
    }

    async Task AddJoinAsync(CategoryModel model)
    {
        var entity = await DescriptionRepository.AddCategory(TypeId, model.CategoryId);
        model.UpdateJoinId(entity.Id!.Value);
        
    }

    async Task BulkAddJoinAsync(IEnumerable<CategoryModel> missing)
    {
        foreach (var category in missing) await AddJoinAsync(category);
    }

    void RemoveJoin(CategoryModel model)
    {
        DescriptionRepository.RemoveCategory(model.Id!.Value);
    }

    void BulkRemoveJoin(IEnumerable<CategoryModel> missing)
    {
        foreach (var category in missing) RemoveJoin(category);
    }
}