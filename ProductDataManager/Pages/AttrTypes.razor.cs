namespace ProductDataManager.Pages;

using Infrastructure;
using Infrastructure.Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Radzen.Blazor;

public partial class AttryTypes
{
    [Inject] protected ProductContext Context { get; set; }
    [Inject] protected DialogService DialogService { get; set; }

    protected IEnumerable<CategoryAttrType> Elements { get; set; }

    protected override void OnInitialized()
    {
        Elements = Context.CategoryAttrTypes
            .Include(c => c.Category)
            .Include(c => c.Type);
    }
    async Task InsertRowAsync(CategoryAttrType join)
    {
        ToInsert = new();

        await Grid.Reload();
        StateHasChanged();
    }

    #region Grid

    protected RadzenDataGrid<CategoryAttrType> Grid;

    protected CategoryAttrType ToInsert;
    protected CategoryAttrType ToUpdate;

    protected async Task InsertRowAsync()
    {
        ToInsert = new();
        await Grid.InsertRow(ToInsert);
    }

    protected async Task EditRowAsync(CategoryAttrType element)
    {
        ToUpdate = element;
        await Grid.EditRow(element);
    }

    protected void CancelEdit(CategoryAttrType element)
    {
        if (element.Equals(ToInsert)) ToInsert = null;

        ToUpdate = null;

        Grid.CancelEditRow(element);

        var orderEntry = Context.Entry(element);
        if (orderEntry.State != EntityState.Modified) return;

        orderEntry.CurrentValues.SetValues(orderEntry.OriginalValues);
        orderEntry.State = EntityState.Unchanged;
    }

    protected Task SaveRowAsync(CategoryAttrType element)
    {
        return Grid.UpdateRow(element);
    }

    protected void Reset()
    {
        ToInsert = null;
        ToUpdate = null;
    }

    #endregion

    #region DbContext

    protected async Task OnCreateRowAsync(CategoryAttrType element)
    {
        ToInsert = null;

        await Context.AddAsync(element);
        await Context.SaveChangesAsync();
    }

    protected async Task OnUpdateRowAsync(CategoryAttrType element)
    {
        if (element.Equals(ToInsert)) ToInsert = null;
        ToUpdate = null;

        Context.Update(element);
        await Context.SaveChangesAsync();
    }

    protected virtual Task<bool> OnDeleteRowAsync(CategoryAttrType element)
    {
        return Task.FromResult(true);
    }

    protected async Task DeleteRowAsync(CategoryAttrType element)
    {
        if (await DialogService.Confirm("Are you sure you want to delete this record?") == false) return;
        if (!await OnDeleteRowAsync(element)) return;

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