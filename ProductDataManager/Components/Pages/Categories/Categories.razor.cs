using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Components.Pages.Categories.Model;
using ProductDataManager.Components.Shared.Dialogs;
using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Exstensions;

namespace ProductDataManager.Components.Pages.Categories;

using DynamicData;
using Microsoft.EntityFrameworkCore;

public partial class Categories : ComponentBase
{
    List<Category> categories = [];
    string filter = string.Empty;
    HashSet<Data> Forest { get; set; } = [];
    HashSet<Parent> Parents { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        DbContext.Changes
            .Connect(entry => entry.Entity is Category)
            .Transform(entry => (Category)entry.Entity)
            // .Group(entry => entry.ParentId ?? Guid.Empty)
            .TransformToTree(entry => entry.ParentId ?? Guid.Empty);
        
        categories = await DbContext.Categories.ToListAsync();
        UpdateForest();
    }

    void UpdateForest()
    {
        var currentFilter = Selector(filter);

        Forest = categories
            .Where(c => c.ParentId == null)
            .Select(c => c.ConvertToData(currentFilter))
            .Where(currentFilter)
            .OrderByDescending(c => c.Name)
            .ToHashSet();

        Parents = categories
            .Select(category => new Parent(category.Id, category.Name))
            .Append(new(default, "Macro"))
            .ToHashSet();

        StateHasChanged();
        return;

        static Func<Data, bool> Selector(string filter)
        {
            return data => string.IsNullOrEmpty(filter)
                           || data.Name.Contains(filter, StringComparison.InvariantCultureIgnoreCase)
                           || data.ParentName.Contains(filter, StringComparison.InvariantCultureIgnoreCase)
                           || data.HasChild;
        }
    }

    void FilterChanged(string value)
    {
        filter = value;
        UpdateForest();
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
                var category = new Category
                {
                    Name = model.Name,
                    Description = model.Description,
                    ParentId = parentId
                };
                
                DbContext.Add(category);
                
                await DbContext.SaveChangesAsync();
                
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
            await DbContext.SaveChangesAsync();
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
        
        // if (data.Items.Count > 0)
        // {
        //     Snackbar.Add("Description has related products", Severity.Error);
        //     return;
        // }

        var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>("Delete Category");

        var result = await dialog.Result;

        if (!result.Canceled)
            try
            {
                DbContext.Remove(data.Category);
                await DbContext.SaveChangesAsync();
                
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
