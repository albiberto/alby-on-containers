namespace AlbyOnContainers.ProductDataManager.Pages;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Models;
using Radzen;
using Radzen.Blazor;
using Services;

public partial class Categories
{
    [Inject] private ProductContext Context { get; set; }
    [Inject] private DialogService DialogService { get; set; }
    [Inject] private NotificationService NotificationService { get; set; }
    [Inject] private CategoryService CategoryService { get; set; }

    private IEnumerable<Category> _categories;
    private RadzenDataGrid<Category> _grid;

    private Category _categoryToInsert;
    private Category _categoryToUpdate;
    
    protected override async Task OnInitializedAsync() => _categories = Context.Categories.Include(c => c.Categories).Where(e => e.ParentId == null);

    private void LoadChildData(DataGridLoadChildDataEventArgs<Category> args)
    {
        foreach (var category in args.Item.Categories)
        {
            var children = Context.Categories.Where(e => e.ParentId == category.Id).ToList();
            category.Categories = children;

        }

        args.Data = args.Item.Categories;
    }

    // Disable inspection because it causes multiple database calls.
    // ReSharper disable once ReplaceWithSingleCallToAny
    private void RowRender(RowRenderEventArgs<Category> args) => args.Expandable = args.Data.Categories.Any();
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender) await _grid.ExpandRow(_categories.FirstOrDefault());
    }

    private void Reset()
    {
        _categoryToInsert = null;
        _categoryToUpdate = null;
    }

    private async Task EditRow(Category category)
    {
        _categoryToUpdate = category;
        await _grid.EditRow(category);
    }

    private async Task OnUpdateRow(Category category)
    {
        if (category == _categoryToInsert) _categoryToInsert = null;

        _categoryToUpdate = null;

        Context.Update(category);
        
        await Context.SaveChangesAsync();
    }

    private async Task SaveRow(Category order)
    {
        await _grid.UpdateRow(order);
    }

    private void CancelEdit(Category order)
    {
        if (order == _categoryToInsert) _categoryToInsert = null;

        _categoryToUpdate = null;

        _grid.CancelEditRow(order);

        // For production
        var orderEntry = Context.Entry(order);
       
        if (orderEntry.State != EntityState.Modified) return;
        
        orderEntry.CurrentValues.SetValues(orderEntry.OriginalValues);
        orderEntry.State = EntityState.Unchanged;
    }

    private async Task DeleteRow(Category category)
    {
        if (await DialogService.Confirm("Are you sure you want to delete this record?") == false) return;
        
        if (category == _categoryToInsert)
        {
            _categoryToInsert = null;
        }

        if (category == _categoryToUpdate)
        {
            _categoryToUpdate = null;
        }

        if (_categories.Contains(category))
        {
            Context.Remove(category);
            
            await Context.SaveChangesAsync();
            await _grid.Reload();
        }
        else
        {
            _grid.CancelEditRow(category);
            await _grid.Reload();
        }
    }

    private async Task InsertRow()
    {
        _categoryToInsert = new();
        await _grid.InsertRow(_categoryToInsert);
    }

    private async Task OnCreateRow(Category category)
    {
        Context.Add(category);
        
        await Context.SaveChangesAsync();
        
        _categoryToInsert = null;
    }

    private async Task ExportClick(RadzenSplitButtonItem args)
    {
        if (args?.Value == "csv")
        {
            await CategoryService.ExportCategoriesToCsv(new()
            { 
                Filter = $@"{(string.IsNullOrEmpty(_grid.Query.Filter)? "true" : _grid.Query.Filter)}", 
                OrderBy = $"{_grid.Query.OrderBy}", 
                Expand = "Parent", 
                Select = string.Join(",", _grid.ColumnsCollection.Where(c => c.GetVisible()).Select(c => c.Property))
            }, "Categories");
        }

        if (args == null || args.Value == "xlsx")
        {
            await CategoryService.ExportCategoriesToExcel(new()
            { 
                Filter = $@"{(string.IsNullOrEmpty(_grid.Query.Filter)? "true" : _grid.Query.Filter)}", 
                OrderBy = $"{_grid.Query.OrderBy}", 
                Expand = "Parent", 
                Select = string.Join(",", _grid.ColumnsCollection.Where(c => c.GetVisible()).Select(c => c.Property))
            }, "Categories");
        }
    }
}