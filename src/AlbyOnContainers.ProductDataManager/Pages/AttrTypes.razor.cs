using System.Linq.Dynamic.Core;
using AlbyOnContainers.ProductDataManager.Data;
using AlbyOnContainers.ProductDataManager.Models;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Radzen.Blazor;

namespace AlbyOnContainers.ProductDataManager.Pages;

public partial class AttrTypes
{
    
    [Inject] private ProductContext Context { get; set; }
    [Inject] private DialogService DialogService { get; set; }

    private RadzenDataGrid<AttrType> _grid;
    private IQueryable<AttrType> _types;
    private int _count;
    private bool _isLoading;
    
    private AttrType _typeToInsert;
    private AttrType _typeToUpdate;

    private void Reset()
    {
        _typeToInsert = null;
        _typeToUpdate = null;
    }

    // protected override async Task OnInitializedAsync()
    // {
    //     await base.OnInitializedAsync();
    //     _types = Context.AttrTypes;
    // }

    private void BindingChanged(IEnumerable<Category> categories, AttrType type)
    {
        type.CategoryAttrTypes = categories.Select(c => new CategoryAttrType()
        {
            Category = c,
            AttrType = type
        }).ToList();
    }
    
    private async Task EditRow(AttrType type)
    {
        _typeToUpdate = type;
        await _grid.EditRow(type);
    }

    private async Task LoadData(LoadDataArgs args)
    {
        _isLoading = true;

        await Task.Yield();

        var query = Context.AttrTypes.AsQueryable();

        if (!string.IsNullOrEmpty(args.Filter)) query = query.Where(args.Filter);
        if (!string.IsNullOrEmpty(args.OrderBy)) query = query.OrderBy(args.OrderBy);

        _count = query.Count();
        _types = query.Skip(args.Skip.Value).Take(args.Top.Value);

        _isLoading = false;
    }
    
    private async Task OnUpdateRow(AttrType type)
    {
        if (type == _typeToInsert)
        {
            _typeToInsert = null;
        }

        _typeToUpdate = null;

        Context.Update(type);
        await Context.SaveChangesAsync();
    }

    private async Task SaveRow(AttrType type)
    {
        await _grid.UpdateRow(type);
    }

    private void CancelEdit(AttrType type)
    {
        if (type == _typeToInsert)
        {
            _typeToInsert = null;
        }

        _typeToUpdate = null;

        _grid.CancelEditRow(type);

        // For production
        var orderEntry = Context.Entry(type);
        if (orderEntry.State != EntityState.Modified) return;
        
        orderEntry.CurrentValues.SetValues(orderEntry.OriginalValues);
        orderEntry.State = EntityState.Unchanged;
    }

    private async Task DeleteRow(AttrType type)
    {
        if (await DialogService.Confirm("Are you sure you want to delete this record?") == false) return;

        if (type == _typeToInsert)
        {
            _typeToInsert = null;
        }

        if (type == _typeToUpdate)
        {
            _typeToUpdate = null;
        }

        if (_types.Contains(type))
        {
            Context.Remove(type);

            await Context.SaveChangesAsync();
            await _grid.Reload();
        }
        else
        {
            _grid.CancelEditRow(type);
            await _grid.Reload();
        }
    }

    private async Task InsertRow()
    {
        _typeToInsert = new();
        await _grid.InsertRow(_typeToInsert);
    }

    private async Task OnCreateRow(AttrType type)
    {
        Context.Add(type);
        await Context.SaveChangesAsync();
        
        _typeToInsert = null;
    }
}
