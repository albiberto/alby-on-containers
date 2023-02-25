namespace AlbyOnContainers.ProductDataManager.Pages.DescriptionsComponent;

using Data;
using Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Models;
using Radzen;
using Radzen.Blazor;

public partial class DescrValues
{
    [Parameter] public DescrType DescrType { get; set; }
    
    [Inject] ProductContext Context { get; set; }
    [Inject] DialogService DialogService { get; set; }

    async Task OnCreateRowAsync(DescrValue values)
    {
        await Context.AddAsync(values);
        await Context.SaveChangesAsync();
        
        toInsert = null;
    }

    async Task OnUpdateRowAsync(DescrValue order)
    {
        if (order.Equals(toInsert)) toInsert = null;
        toUpdate = null;
        
        Context.Update(order);
        await Context.SaveChangesAsync();
    }
    
    async Task DeleteRowAsync(DescrValue value)
    {
        if (await DialogService.Confirm("Are you sure you want to delete this record?") == false) return;
        if (await Context.Descrs.AnyAsync(descr => descr.DescrValueId == value.Id))
        {
            await DialogService.Alert("This record is currently binding to product/products and it cannot be deleted.", "Cannot Delete");
            return;
        }
        
        if (value.Equals(toInsert)) toInsert = null;
        if (value.Equals(toUpdate)) toUpdate = null;

        if (DescrType.DescrValues.Contains(value))
        {
            Context.Remove(value);
            await Context.SaveChangesAsync();

            await grid.Reload();
        }
        else
        {
            grid.CancelEditRow(value);
            await grid.Reload();
        }
    }
}