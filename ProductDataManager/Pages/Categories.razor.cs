namespace ProductDataManager.Pages;

using Infrastructure;
using Infrastructure.Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Radzen.Blazor;

public partial class Categories
{
    [Inject] protected ProductContext Context { get; set; }
    [Inject] protected DialogService DialogService { get; set; }

    protected IEnumerable<Category> Elements { get; set; }

    protected override void OnInitialized()
    {
        Elements = Context.Categories
            .Include(c => c.Categories)
            .Include(c => c.Parent)
            .Where(e => e.ParentId == null);
    }

    void LoadChildData(DataGridLoadChildDataEventArgs<Category> args)
    {
        foreach (var category in args.Item.Categories)
        {
            var children = Context.Categories.Where(e => e.ParentId == category.Id).ToList();
            category.Categories = children;
        }

        args.Data = args.Item.Categories;
    }

    void RowRender(RowRenderEventArgs<Category> args) => args.Expandable = args.Data.Categories.Any();

    async Task InsertRowAsync(Category category)
    {
        ToInsert = new();
        category.Categories.Add(ToInsert);

        await Grid.Reload();
        StateHasChanged();
    }

    #region Grid

    protected RadzenDataGrid<Category> Grid;

    protected Category ToInsert;
    protected Category ToUpdate;

    protected async Task InsertRowAsync()
    {
        ToInsert = new();
        await Grid.InsertRow(ToInsert);
    }

    protected async Task EditRowAsync(Category element)
    {
        ToUpdate = element;
        await Grid.EditRow(element);
    }

    protected void CancelEdit(Category element)
    {
        if (element.Equals(ToInsert)) ToInsert = null;

        ToUpdate = null;

        Grid.CancelEditRow(element);

        var orderEntry = Context.Entry(element);
        if (orderEntry.State != EntityState.Modified) return;

        orderEntry.CurrentValues.SetValues(orderEntry.OriginalValues);
        orderEntry.State = EntityState.Unchanged;
    }

    protected Task SaveRowAsync(Category element)
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

    protected async Task OnCreateRowAsync(Category element)
    {
        ToInsert = null;

        await Context.AddAsync(element);
        await Context.SaveChangesAsync();
    }

    protected async Task OnUpdateRowAsync(Category element)
    {
        if (element.Equals(ToInsert)) ToInsert = null;
        ToUpdate = null;

        Context.Update(element);
        await Context.SaveChangesAsync();
    }

    protected virtual Task<bool> OnDeleteRowAsync(Category element)
    {
        return Task.FromResult(true);
    }

    protected async Task DeleteRowAsync(Category element)
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

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender) await Grid.ExpandRow(Elements.FirstOrDefault());
    }

    #endregion
}