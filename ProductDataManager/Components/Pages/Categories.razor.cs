using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MudBlazor;
using ProductDataManager.Components.Shared;
using ProductDataManager.Exstensions;
using ProductDataManager.Infrastructure;
using ProductDataManager.Infrastructure.Domain;

namespace ProductDataManager.Components.Pages;

public partial class Categories
{
    List<Category> categories = [];

    string filter = string.Empty;

    void FilterChanged(string value)
    {
        filter = value;
        UpdateForest();
    }
    
    [Inject] public required ProductContext Context { get; set; }
    [Inject] public required IDialogService DialogService { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    [Inject] public required ILogger<Categories> Logger { get; set; }

    HashSet<Data> Forest { get; set; } = [];
    
    void UpdateForest() => Forest =  categories
        .Where(c => c.ParentId == null)
        .Select(c => c.ConvertToData(Selector))
        .Where(Selector)
        .OrderByDescending(c => c.Name)
        .ToHashSet();

    HashSet<Parent> Parents => categories.Select(category => new Parent(category.Id!.Value, category.Name))
        .Append(new(default, "Macro")).ToHashSet();

    Func<Data, bool> Selector => data => string.IsNullOrEmpty(filter)
                                         || data.Name.Contains(filter, StringComparison.InvariantCultureIgnoreCase)
                                         || data.Description.Contains(filter, StringComparison.InvariantCultureIgnoreCase)
                                         || data.HasChild;

    protected override async Task OnInitializedAsync()
    {
        categories = await Context.Categories.ToListAsync();
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

            var entity = await Context.Categories.AddAsync(category);
            await Context.SaveChangesAsync();

            categories.Add(entity.Entity);
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
            
            foreach (var category in categories)
            foreach (var child in category.Categories.Where(child => child.ParentId != category.Id || child.ParentId is null)) category.Categories.RemoveWhere(c => c.Id == child.Id);

            UpdateForest();

            Snackbar.Add("Category Updated!", Severity.Success);
            StateHasChanged();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while updating category");
            Snackbar.Add("Error while updating category", Severity.Error);
        }
    }

    async Task DeleteAsync(Data data)
    {
        if (data.Items.Count > 0)
        {
            Snackbar.Add("Category has subcategories", Severity.Error);
            return;
        }

        var parameters = new DialogParameters<ConfirmDeleteDialog>
        {
            { x => x.ContentText, "Do you really want to delete these records? This process cannot be undone." },
            { x => x.ButtonText, "Delete" },
            { x => x.Color, Color.Error }
        };

        var options = new DialogOptions { CloseButton = true, Position = DialogPosition.Center, MaxWidth = MaxWidth.ExtraExtraLarge };

        var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>("Delete", parameters, options);
        var result = await dialog.Result;

        if (result.Canceled) return;

        var current = await Context.Categories.FindAsync(data.Id);
        
        if(current is null)
        {
            Snackbar.Add("Error while deleting category", Severity.Error);
            return;
        }
        
        Context.Categories.Remove(current);
        await Context.SaveChangesAsync();

        categories.Remove(current);
        UpdateForest();
    }
}