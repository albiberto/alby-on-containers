namespace ProductDataManager.Pages;

using Infrastructure;
using Infrastructure.Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Radzen.Blazor;
using System.Linq.Dynamic.Core;
using System.Linq;

public partial class AttrTypes
{
    [Inject] ProductContext Context { get; set; } = null!;
    [Inject] DialogService DialogService { get; set; } = null!;

    IEnumerable<AttrType> Types { get; set; }
    int count;

    IEnumerable<Category> Categories { get; set; } = Array.Empty<Category>();
    IEnumerable<Category> SelectedCategories { get; set; } = Array.Empty<Category>();
    
    protected override void OnInitialized() => Categories = Context.Categories.AsNoTracking().ToList();

    void LoadData(LoadDataArgs args)
    {
        var query = Context.AttrTypes
            .Include(join => join.CategoryAttrTypes)
            .ThenInclude(join => join.Category)
            .AsQueryable();

        if (!string.IsNullOrEmpty(args.Filter))
        {
            query = query.Where(args.Filter);
            count = query.Count();
        }
        else
        {
            count = query.Count();
        }

        if (!string.IsNullOrEmpty(args.OrderBy)) query = query.OrderBy(args.OrderBy);

        Types = query
            .Skip(args.Skip ?? 0)
            .Take(args.Top ?? 10);
    }
    
    protected RadzenDataGrid<AttrType> Grid = null!;
    protected RadzenDropDownDataGrid<IEnumerable<Category>> CategoryGrid = null!;

    protected AttrType? ToInsert;
    protected AttrType? ToUpdate;

    protected async Task InsertRowAsync()
    {
        ToInsert = new();
        await Grid.InsertRow(ToInsert);
    }

    protected async Task EditRowAsync(AttrType element)
    {
        ToUpdate = element;
        await Grid.EditRow(element);
    }

    protected async Task CancelEdit(AttrType element)
    {
        if (element.Equals(ToInsert)) ToInsert = null;

        ToUpdate = null;

        Grid.CancelEditRow(element);

        var orderEntry = Context.Entry(element);
        if (orderEntry.State != EntityState.Modified) return;

        orderEntry.CurrentValues.SetValues(orderEntry.OriginalValues);
        orderEntry.State = EntityState.Unchanged;
    }

    protected Task SaveRowAsync(AttrType element) => Grid.UpdateRow(element);

    protected void Reset()
    {
        ToInsert = null;
        ToUpdate = null;
    }
}