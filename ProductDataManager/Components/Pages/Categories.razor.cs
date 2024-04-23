using Microsoft.EntityFrameworkCore;
using MudBlazor;
using ProductDataManager.Components.Shared;
using ProductDataManager.Exstensions;
using ProductDataManager.Infrastructure.Domain;

namespace ProductDataManager.Components.Pages;

public partial class Categories
{
    List<Category> categories = [];
    HashSet<Data> Forest { get; set; } = [];
    HashSet<Parent> Parents { get; set; } = [];
    string filter = string.Empty;
    
    protected override async Task OnInitializedAsync()
    {
        categories = await Context.Categories.ToListAsync();
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
            .Select(category => new Parent(category.Id!.Value, category.Name))
            .Append(new(default, "Macro"))
            .ToHashSet();
        
        StateHasChanged();
        return;

        static Func<Data, bool> Selector(string filter) =>
            data => string.IsNullOrEmpty(filter)
                    || data.Name.Contains(filter, StringComparison.InvariantCultureIgnoreCase)
                    || data.Description.Contains(filter, StringComparison.InvariantCultureIgnoreCase)
                    || data.HasChild;
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

        if (!result.Canceled)
            try
            {
                await AddCategory();
                Snackbar.Add("Category Added!", Severity.Success);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error while saving category");
                Snackbar.Add("Error while saving category", Severity.Error);
            }

        async Task AddCategory()
        {
            var model = (CategoryDialog.Model)result.Data;
            var category = new Category(model.Name, model.Description, parentId);

            await Context.Categories.AddAsync(category);
            await Context.SaveChangesAsync();

            UpdateForest();
        }
    }

    async Task UpdateCategoryAsync(Data data)
    {
        try
        {
            var current = await Context.Categories.FindAsync(data.Id);
            
            if(current is null)
            {
                Snackbar.Add("Error while updating category", Severity.Error);
                return;
            }
            
            current.Update(data.Name, data.Description, data.ParentId);
            await Context.SaveChangesAsync();

            UpdateForest();
            Snackbar.Add("Category Updated!", Severity.Success);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while updating category");
            Snackbar.Add("Error while updating category", Severity.Error);
        }
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
        if (result.Canceled) return;

        try
        {
            await DeleteAsync();
            Snackbar.Add("Category Deleted!", Severity.Success);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while deleting category");
            Snackbar.Add("Error while deleting category", Severity.Error);
        }

        async Task DeleteAsync()
        {
            Context.Categories.Remove(data.Category);
            await Context.SaveChangesAsync();

            UpdateForest();
        }
    }
}