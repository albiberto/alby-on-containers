namespace AlbyOnContainers.ProductDataManager.Pages.ProductsComponents;

using System.Linq;
using Extensions;
using Microsoft.AspNetCore.Components;
using Models;
using Microsoft.EntityFrameworkCore;
using Radzen;

public partial class Attrs
{
    [Parameter]public Product Product { get; set; }
    
    async Task OnCreateRowAsync(Attr type)
    {
        toInsert = null;

        await Context.AddAsync(type);
        await Context.SaveChangesAsync();
    }

    async Task OnUpdateRowAsync(Attr type)
    {
        if (type.Equals(toInsert)) toInsert = null;
        toUpdate = null;
        
        Context.Update(type);
        await Context.SaveChangesAsync();
    }
    
    async Task DeleteRowAsync(Attr 
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

        if (Product.Attrs.Contains(type))
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