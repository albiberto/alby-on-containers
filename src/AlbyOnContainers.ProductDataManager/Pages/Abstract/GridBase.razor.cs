namespace AlbyOnContainers.ProductDataManager.Pages.Abstract;

using Radzen.Blazor;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Data;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Models.Abstract;
using Radzen;

public abstract class GridBase<T> :ComponentBase where T: Entity, new()
{
    [Inject] protected ProductContext Context { get; set; }
    [Inject] protected DialogService DialogService { get; set; }
    [Inject] protected IStringLocalizer<T> L { get; set; }

    protected IEnumerable<T> Elements { get; set; }

    #region Grid

    protected RadzenDataGrid<T> Grid;
    
    protected T ToInsert;
    protected T ToUpdate;   
    
    protected async Task InsertRowAsync()
    {
        ToInsert = new();
        await Grid.InsertRow(ToInsert);
    }
    
    protected async Task EditRowAsync(T element)
    {
        ToUpdate = element;
        await Grid.EditRow(element);
    }

    protected void CancelEdit(T element)
    {
        if (element.Equals(ToInsert)) ToInsert = null;

        ToUpdate = null;

        Grid.CancelEditRow(element);

        var orderEntry = Context.Entry(element);
        if (orderEntry.State != EntityState.Modified) return;
        
        orderEntry.CurrentValues.SetValues(orderEntry.OriginalValues);
        orderEntry.State = EntityState.Unchanged;
    }
    
    protected Task SaveRowAsync(T element) => Grid.UpdateRow(element);
    
    protected void Reset()
    {
        ToInsert = null;
        ToUpdate = null;
    }
    
    #endregion

    #region DbContext
    
    protected async Task OnCreateRowAsync(T element)
    {
        ToInsert = null;

        await Context.AddAsync(element);
        await Context.SaveChangesAsync();
    }

    protected async Task OnUpdateRowAsync(T element)
    {
        if (element.Equals(ToInsert)) ToInsert = null;
        ToUpdate = null;
        
        Context.Update(element);
        await Context.SaveChangesAsync();
    }

    protected virtual Task<bool> OnDeleteRowAsync(T element) => Task.FromResult(true);
    
    protected async Task DeleteRowAsync(T element)
    {
        if (await DialogService.Confirm("Are you sure you want to delete this record?") == false) return;
        if(!await OnDeleteRowAsync(element)) return;

        if (element.Equals(ToInsert)) ToInsert = null;
        if (element.Equals(ToUpdate)) ToUpdate = null;

        if (Elements.Contains(element))
        {
            Context.Remove(element);
            await Context.SaveChangesAsync();

            await Grid.Reload();
        }
        else
        {
            Grid.CancelEditRow(element);
            await Grid.Reload();
        }
    }
    
    #endregion
}