namespace AlbyOnContainers.ProductDataManager.Pages.DescriptionsComponent;

using Data;
using Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Linq;
using Extensions;

public partial class DescrTypes
{
    [Inject] ProductContext Context { get; set; }
    [Inject] DialogService DialogService { get; set; }

    IEnumerable<DescrType> types;
    int count;
    bool isLoading;
    
    async Task LoadDataAsync(LoadDataArgs args)
    {
        isLoading = true;
        await Task.Yield();

        var query = Context.DescrTypes
            .Include(type => type.CategoryDescrTypes)
            .ThenInclude(join => join.Category)
            .Include(type => type.DescrValues)
            .AsQueryable();

        var result = await query.LoadDataAsync(args);

        types = result.Entities;
        count = result.Count;
        
        isLoading = false;
    }

    async Task OnCreateRowAsync(DescrType type)
    {
        toInsert = null;

        await Context.AddAsync(type);
        await Context.SaveChangesAsync();
        
        await grid.FirstPage();
    }

    async Task OnUpdateRowAsync(DescrType type)
    {
        if (type.Equals(toInsert)) toInsert = null;
        toUpdate = null;
        
        Context.Update(type);
        await Context.SaveChangesAsync();
    }
    
    async Task DeleteRowAsync(DescrType type)
    {
        if (await DialogService.Confirm("Are you sure you want to delete this record?") == false) return;
        if (await Context.DescrValues.AnyAsync(value => value.DescrTypeId == type.Id))
        {
            await DialogService.Alert("This record is currently binding to value/values and it cannot be deleted.", "Cannot Delete");
            return;
        }

        if (type.Equals(toInsert)) toInsert = null;
        if (type.Equals(toUpdate)) toUpdate = null;

        if (types.Contains(type))
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