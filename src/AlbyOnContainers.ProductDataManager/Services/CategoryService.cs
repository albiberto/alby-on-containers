namespace AlbyOnContainers.ProductDataManager.Services;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Models;
using Radzen;

public partial class CategoryService
{
    private readonly ProductContext _context;
    private readonly NavigationManager _navigationManager;

    public CategoryService(ProductContext context, NavigationManager navigationManager)
    {
        _context = context;
        _navigationManager = navigationManager;
    }

    public void Reset() => _context.ChangeTracker.Entries().Where(e => e.Entity is not null).ToList().ForEach(e => e.State = EntityState.Detached);
        
    public async Task ExportCategoriesToExcel(Query query = null, string fileName = null) => _navigationManager.NavigateTo(query != null ? query.ToUrl($"export/product/categories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/product/categories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);

    public async Task ExportCategoriesToCsv(Query query = null, string fileName = null) => _navigationManager.NavigateTo(query != null ? query.ToUrl($"export/product/categories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/product/categories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);

    public async Task<IQueryable<Category>> GetCategories(Query query = null)
    {
        var items = _context.Categories.AsQueryable();

        items = items.Include(i => i.Parent);

        if (query == null) return await Task.FromResult(items);
            
        if (!string.IsNullOrEmpty(query.Expand))
        {
            var propertiesToExpand = query.Expand.Split(',');
            items = propertiesToExpand.Aggregate(items, (current, p) => current.Include(p.Trim()));
        }

        if (!string.IsNullOrEmpty(query.Filter))
        {
            items = query.FilterParameters != null ? items.Where(query.Filter, query.FilterParameters) : items.Where(query.Filter);
        }

        if (!string.IsNullOrEmpty(query.OrderBy))
        {
            items = items.OrderBy(query.OrderBy);
        }

        if (query.Skip.HasValue)
        {
            items = Queryable.Skip(items, query.Skip.Value);
        }

        if (query.Top.HasValue)
        {
            items = Queryable.Take(items, query.Top.Value);
        }

        return await Task.FromResult(items);
    }

    public async Task<Category> GetCategoryById(Guid id)
    {
        var items = _context.Categories
            .AsNoTracking()
            .Where(i => i.Id == id);

        items = items.Include(i => i.Parent);

        var itemToReturn = Queryable.FirstOrDefault(items);

        return await Task.FromResult(itemToReturn);
    }

    public async Task<Category> CreateCategory(Category category)
    {
        var existingItem = _context.Categories
            .FirstOrDefault(i => i.Id == category.Id);

        if (existingItem != null)
        {
            throw new("Item already available");
        }

        try
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }
        catch
        {
            _context.Entry(category).State = EntityState.Detached;
            throw;
        }

        return category;
    }

    public async Task<Category> CancelCategoryChanges(Category item)
    {
        var entityToCancel = _context.Entry(item);
        if (entityToCancel.State != EntityState.Modified) return item;
            
        entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
        entityToCancel.State = EntityState.Unchanged;

        return item;
    }

    public async Task<Category> UpdateCategory(Guid id, Category category)
    {
        var itemToUpdate = _context.Categories
            .FirstOrDefault(i => i.Id == category.Id);

        if (itemToUpdate == null)
        {
            throw new("Item no longer available");
        }

        var entryToUpdate = _context.Entry(itemToUpdate);
        entryToUpdate.CurrentValues.SetValues(category);
        entryToUpdate.State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return category;
    }

    public async Task<Category> DeleteCategory(Guid id)
    {
        var itemToDelete = _context.Categories
            .FirstOrDefault(i => i.Id == id);

        if (itemToDelete == null)
        {
            throw new("Item no longer available");
        }

        _context.Categories.Remove(itemToDelete);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch
        {
            _context.Entry(itemToDelete).State = EntityState.Unchanged;
            throw;
        }

        return itemToDelete;
    }
}