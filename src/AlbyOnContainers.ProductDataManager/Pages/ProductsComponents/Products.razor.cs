namespace AlbyOnContainers.ProductDataManager.Pages.ProductsComponents;

using System.Linq;
using Extensions;
using Models;
using Microsoft.EntityFrameworkCore;
using Radzen;

public partial class Products
{
    async Task LoadDataAsync(LoadDataArgs args)
    {
        isLoading = true;
        await Task.Yield();

        var query = Context.Products
            .Include(product => product.Descrs)
            .ThenInclude(join => join.DescrValue)
            .Include(product => product.Attrs)
            .AsQueryable();

        var result = await query.LoadDataAsync(args);

        elements = result.Entities;
        count = result.Count;
        
        isLoading = false;
    }

    async Task OnCreateRowAsync(Product type)
    {
        toInsert = null;

        await Context.AddAsync(type);
        await Context.SaveChangesAsync();
    }

    async Task OnUpdateRowAsync(Product type)
    {
        if (type.Equals(toInsert)) toInsert = null;
        toUpdate = null;
        
        Context.Update(type);
        await Context.SaveChangesAsync();
    }
    
    async Task DeleteRowAsync(Product 
        type)
    {
        if (await DialogService.Confirm("Are you sure you want to delete this record?") == false) return;
        if (await Context.DescrValues.AnyAsync(value => value.DescrTypeId == type.Id))
        {
            await DialogService.Alert("This record is currently binding to value/values and it cannot be deleted.", "Cannot Delete");
            return;
        }

        if (type.Equals(toInsert)) toInsert = null;
        if (type.Equals(toUpdate)) toUpdate = null;

        if (elements.Contains(type))
        {
            Context.Remove(type);
            await Context.SaveChangesAsync();

            await grid.Reload();
        }
        else
        {
            grid.CancelEditRow(type);
            await grid.Reload();
        }
    }
}