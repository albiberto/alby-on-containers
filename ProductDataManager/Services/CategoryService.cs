namespace ProductDataManager.Services;

using System.Text.Encodings.Web;
using Infrastructure;
using Infrastructure.Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

public class CategoryService : ServiceBase
{
    public CategoryService(ProductContext Context, NavigationManager navigation) : base(Context, navigation)
    {
    }

    public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);
    
    public void ExportCategoriesToExcel(Query? query = default, string? fileName = default)
    {
        Navigation.NavigateTo(query != null ? query.ToUrl($"export/product/categories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/product/categories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
    }

    public void ExportCategoriesToCsv(Query? query = default, string? fileName = default)
    {
        Navigation.NavigateTo(query != null ? query.ToUrl($"export/product/categories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/product/categories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
    }

    public IQueryable<Category> GetCategories(Query? query = default)
    {
        var items = Context.Categories.AsQueryable();

        items = items.Include(i => i.Parent);

        if (query == null) return items;
        
        if (!string.IsNullOrEmpty(query.Expand))
        {
            items = query.Expand.Split(',').Aggregate(items, (current, p) => current.Include(p.Trim()));
        }

        ApplyQuery(ref items, query);

        return items;
    }

    public Category GetCategoryById(Guid id) =>
        Context.Categories
            .AsNoTracking()
            .Include(i => i.Parent)
            .Single(i => i.Id == id);

    public Category CreateCategory(Category category)
    {
        var existingItem = Context.Categories
            .FirstOrDefault(i => i.Id == category.Id);

        if (existingItem != null) throw new("Item already available");

        try
        {
            Context.Categories.Add(category);
            Context.SaveChanges();
        }
        catch
        {
            Context.Entry(category).State = EntityState.Detached;
            throw;
        }

        return category;
    }

    public async Task<Category> CancelCategoryChanges(Category item)
    {
        var entityToCancel = Context.Entry(item);
        
        if (entityToCancel.State != EntityState.Modified) return item;
        
        entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
        entityToCancel.State = EntityState.Unchanged;

        return item;
    }

    public Category UpdateCategory(Category category)
    {
        var itemToUpdate = Context.Categories
            .FirstOrDefault(i => i.Id == category.Id);

        if (itemToUpdate == null) throw new("Item no longer available");

        var entryToUpdate = Context.Entry(itemToUpdate);
        entryToUpdate.CurrentValues.SetValues(category);
        entryToUpdate.State = EntityState.Modified;

        Context.SaveChanges();

        return category;
    }

    public Category DeleteCategory(Category category)
    {
        var itemToDelete = Context.Categories
            .Where(i => i.Id == category.Id)
            .Include(i => i.Parent)
            .FirstOrDefault();

        if (itemToDelete == null) throw new("Item no longer available");


        Context.Categories.Remove(itemToDelete);

        try
        {
            Context.SaveChanges();
        }
        catch
        {
            Context.Entry(itemToDelete).State = EntityState.Unchanged;
            throw;
        }

        return itemToDelete;
    }
}