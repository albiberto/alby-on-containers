using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Components.Pages.Categories.Model;
using ProductDataManager.Components.Shared.Dialogs;
using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Exstensions;
using ProductDataManager.Infrastructure.Specifications;

namespace ProductDataManager.Components.Pages.Categories;

public partial class Categories : ComponentBase
{
    List<Category> categories = [];
    HashSet<Data> Forest { get; set; } = [];
    HashSet<Parent> Parents { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        categories = await Repository.ListAsync(new OrderedCategorySpecification());
        UpdateForest();
    }

    void UpdateForest()
    {
        static Func<Data, bool> Selector(string filter)
        {
            return data => string.IsNullOrEmpty(filter)
                           || data.Name.Contains(filter, StringComparison.InvariantCultureIgnoreCase)
                           || data.ParentName.Contains(filter, StringComparison.InvariantCultureIgnoreCase)
                           || data.HasChild;
        }
        
        var currentFilter = Selector(Filter);

        Forest = categories
            .Where(c => c.ParentId == null)
            .Select(c => c.ConvertToData(currentFilter))
            .Where(currentFilter)
            .OrderByDescending(c => c.Name)
            .ToHashSet();

        Parents = categories
            .Select(category => new Parent(category.Id!.Value, category.Name))
            .Append(new(default, "Macro"))
            .ToHashSet();

        StateHasChanged();
    }

    async Task AddCategoryAsync(Guid? parentId = default)
    {
        var dialog = await DialogService.ShowAsync<CategoryDialog>("Add Macro Category");
        var result = await dialog.Result;
        
        var model = (CategoryDialog.Model)result.Data;

        if (categories.Select(category => category.Name).Contains(model.Name))
        {
            Snackbar.Add("Category already exists", Severity.Error);
            return;
        }

        if (!result.Canceled)
            try
            {
                var category = await Repository.AddAsync(new(model.Name, model.Description, parentId));
                await Repository.SaveChangesAsync();
                
                if(parentId is null) categories.Add(category);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error while saving category");
                Snackbar.Add("Error while saving category", Severity.Error);
            }

        UpdateForest();
        Snackbar.Add("Category Added!", Severity.Success);
    }

    async Task UpdateCategoryAsync(Data data)
    {
        try
        {
            var category = await Repository.GetByIdAsync(data.Id!.Value);
            
            if(category is null) throw new ArgumentException("Category not found!");
            
            category.Update(data.Name, data.Description, data.ParentId);
            await Repository.UpdateAsync(category);
            await Repository.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while updating category");
            Snackbar.Add("Error while updating category", Severity.Error);
        }

        UpdateForest();
        Snackbar.Add("Category Updated!", Severity.Success);
    }

    async Task DeleteCategoryAsync(Data data)
    {
        if (data.Items.Count > 0)
        {
            Snackbar.Add("Category has subcategories", Severity.Error);
            return;
        }

        var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>("Delete Category");
        var result = await dialog.Result;

        if (!result.Canceled)
            try
            {
                var category = await Repository.GetByIdAsync(data.Id!.Value);
                
                if(category is null) throw new ArgumentException("Category not found!");

                await Repository.DeleteAsync(category);
                await Repository.SaveChangesAsync();
                
                if(data.ParentId is null) categories.RemoveAll(c => c.Id == data.Id);
                UpdateForest();
                
                Snackbar.Add("Category Deleted!", Severity.Success);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error while deleting category");
                Snackbar.Add("Error while deleting category", Severity.Error);
            }
    }
}