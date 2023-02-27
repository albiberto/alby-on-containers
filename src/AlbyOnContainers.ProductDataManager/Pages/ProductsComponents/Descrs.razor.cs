namespace AlbyOnContainers.ProductDataManager.Pages.ProductsComponents;

using System.Linq;
using Extensions;
using Microsoft.AspNetCore.Components;
using Models;
using Microsoft.EntityFrameworkCore;
using Radzen;

public partial class Descrs
{
    [Parameter]public Product Product { get; set; }
    IEnumerable<CategoryDescrType> types;

    protected override void OnInitialized()
    {
        types = Context.CategoryDescrTypes
            .Include(join => join.DescrType)
            .ThenInclude(type => type.DescrValues)
            .Where(join => join.CategoryId == Product.CategoryId)
            .OrderBy(join => join.DescrType.Name);
    }

    async Task OnCreateRowAsync(Descr type)
    {
        toInsert = null;

        await Context.AddAsync(type);
        await Context.SaveChangesAsync();
    }

    async Task OnUpdateRowAsync(Descr type)
    {
        if (type.Equals(toInsert)) toInsert = null;
        toUpdate = null;
        
        Context.Update(type);
        await Context.SaveChangesAsync();
    }
    
    async Task DeleteRowAsync(Descr 
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

        if (Product.Descrs.Contains(type))
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