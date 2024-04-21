using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using ProductDataManager.Components.Shared;
using ProductDataManager.Exstensions;
using ProductDataManager.Infrastructure;
using ProductDataManager.Infrastructure.Domain;

namespace ProductDataManager.Components.Pages;

public partial class Categories
{
    [Inject] public required ProductContext Context { get; set; }
    [Inject] public required IDialogService DialogService { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    [Inject] public required ILogger<Categories> Logger { get; set; }

    [Parameter] public required HashSet<Data> Forest { get; set; } = [];
    [Parameter] public bool Checkable { get; set; }
    [Parameter] public Color Color { get; set; } = Color.Primary;

    [Parameter] public bool Any { get; set; }
    [Parameter] public EventCallback<bool> AnyChanged { get; set; }

    // TODO: Implement search functionality
    string filter = string.Empty;
    
    protected override async Task OnInitializedAsync()
    {
        // TODO: Manage multiple includes
        var result =  await Context.Categories
            .AsNoTracking()
            .Where(c => c.ParentId == null)
            .AsNoTracking()
            .Include(c => c.Categories)
            .ThenInclude(c => c.Categories)
            .ThenInclude(c => c.Categories)
            .ThenInclude(c => c.Categories)
            .AsNoTracking()
            .ToListAsync();
        
        Forest = result.Select(c => c.ConvertToData()).ToHashSet();
    }

    async Task AddCategoryAsync(Guid? parentId = default)
    {
        var dialog = await DialogService.ShowAsync<CategoryDialog>("Add Macro Category");
        var result = await dialog.Result;
        
        if (!result.Canceled)
        {
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
        }

        async Task AddCategory()
        {
            var model = (CategoryDialog.Model)result.Data;
            var category = new Category(model.Name, model.Description, parentId);
            
            // TODO: Add in non connected mode
            var entity = await Context.Categories.AddAsync(category);
            await Context.SaveChangesAsync();
            
            Forest.AddToForest(entity.Entity);
        }
    }

    async Task UpdateCategoryAsync(Data data)
    {
        try
        {
            await UpdateAsync();
            Snackbar.Add("Category Updated!", Severity.Success);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while updating category");
            Snackbar.Add("Error while updating category", Severity.Error);
        }

        async Task UpdateAsync()
        {
            var category = new Category(data.Name, data.Description, data.Parent?.Id, data.Id);
            
            // TODO: Update in non connected mode
            Context.Update(category);
            await Context.SaveChangesAsync();
        }
    }

    async Task DeleteAsync(Data data)
    {
        if(data.Items.Count > 0)
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

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>("Delete", parameters, options);
        var result = await dialog.Result;

        if (result.Canceled) return;
        
        var category = await Context.Categories.FindAsync(data.Id);
        if (category != null)
        {
            var entity = Context.Categories.Remove(category);
            await Context.SaveChangesAsync();
            Forest.RemoveFromForest(entity.Entity);

        }
    }
}