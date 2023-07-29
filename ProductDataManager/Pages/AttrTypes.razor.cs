namespace ProductDataManager.Pages;

using Infrastructure;
using Infrastructure.Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Radzen.Blazor;

public partial class AttrTypes
{
    [Inject] protected ProductContext Context { get; set; } = null!;
    [Inject] protected DialogService DialogService { get; set; } = null!;

    protected IEnumerable<Model> Elements { get; set; } = Array.Empty<Model>();
    protected IEnumerable<Category> Categories { get; set; } = Array.Empty<Category>();

    protected override async Task OnInitializedAsync()
    {
        var join = await Context.CategoryAttrTypes
            .Include(join => join.Type)
            .Include(join => join.Category)
            .GroupBy(k => k.Type)
            .ToDictionaryAsync(k => k.Key, k => k.Select(v => v.Category));
        
        Elements = join.Select(j => new Model(j));
        Categories = Context.Categories;
    }

    #region Grid

    protected RadzenDataGrid<Model> Grid = null!;
    protected RadzenDropDownDataGrid<IEnumerable<Category>> CategoryGrid = null!;

    protected Model? ToInsert;
    protected Model? ToUpdate;

    protected async Task InsertRowAsync()
    {
        ToInsert = new();
        await Grid.InsertRow(ToInsert);
    }

    protected async Task EditRowAsync(Model element)
    {
        ToUpdate = element;
        await Grid.EditRow(element);
    }

    protected async Task CancelEdit(Model element)
    {
        if (element.Equals(ToInsert)) ToInsert = null;

        ToUpdate = null;

        Grid.CancelEditRow(element);

        var orderEntry = Context.Entry(element);
        if (orderEntry.State != EntityState.Modified) return;

        orderEntry.CurrentValues.SetValues(orderEntry.OriginalValues);
        orderEntry.State = EntityState.Unchanged;
    }

    protected Task SaveRowAsync(Model element)
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

    protected void OnCreateRowAsync(Model model)
    {
        ToInsert = null;

        var toAdd = model.Build();

        foreach (var add in toAdd)
        {
            Context.CategoryAttrTypes.Add(add);
        }

        Context.SaveChanges();
    }   

    protected async Task OnUpdateRowAsync(Model element)
    {
        if (element.Equals(ToInsert)) ToInsert = null;
        ToUpdate = null;

        Context.Update(element);
        await Context.SaveChangesAsync();
    }

    protected virtual Task<bool> OnDeleteRowAsync(Model element)
    {
        return Task.FromResult(true);
    }

    protected async Task DeleteRowAsync(Model element)
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