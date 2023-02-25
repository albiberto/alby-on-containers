using System.Linq.Dynamic.Core;
using AlbyOnContainers.ProductDataManager.Data;
using AlbyOnContainers.ProductDataManager.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Radzen.Blazor;

namespace AlbyOnContainers.ProductDataManager.Pages;

public partial class Products
{
    [Inject] private ProductContext Context { get; set; }
    [Inject] private DialogService DialogService { get; set; }


    private RadzenDataGrid<Product> _grid;
    private IQueryable<Product> _products;
    private int _count;


    private bool _isLoading;
    
    private Product _productToInsert;
    private Product _productToUpdate;
    
    // protected override void OnInitialized()
    // {
    //     _categories = Context.Categories.ToList();
    // }



    private void Reset()
    {
        _productToInsert = null;
        _productToUpdate = null;
    }

    private async Task LoadData(LoadDataArgs args)
    {
        _isLoading = true;

        await Task.Yield();

        var query = Context.Products.Include(p => p.Category).Include(p => p.Attrs).AsQueryable();

        if (!string.IsNullOrEmpty(args.Filter)) query = query.Where(args.Filter);
        if (!string.IsNullOrEmpty(args.OrderBy)) query = query.OrderBy(args.OrderBy);

        _count = query.Count();
        _products = query.Skip(args.Skip.Value).Take(args.Top.Value);

        _isLoading = false;
    }

    private async Task EditRow(Product product)
    {
        _productToUpdate = product;
        await _grid.EditRow(product);
    }

    private async Task OnUpdateRow(Product product)
    {
        if (product == _productToInsert)
        {
            _productToInsert = null;
        }

        _productToUpdate = null;

        Context.Update(product);

        await Context.SaveChangesAsync();
    }

    private async Task SaveRow(Product product)
    {
        await _grid.UpdateRow(product);
    }

    private void CancelEdit(Product product)
    {
        if (product == _productToInsert)
        {
            _productToInsert = null;
        }

        _productToUpdate = null;

        _grid.CancelEditRow(product);

        var orderEntry = Context.Entry(product);
        if (orderEntry.State != EntityState.Modified) return;
        
        orderEntry.CurrentValues.SetValues(orderEntry.OriginalValues);
        orderEntry.State = EntityState.Unchanged;
    }

    private async Task DeleteRow(Product product)
    {
        if (await DialogService.Confirm("Are you sure you want to delete this record?") == false) return;

        if (product == _productToInsert)
        {
            _productToInsert = null;
        }

        if (product == _productToUpdate)
        {
            _productToUpdate = null;
        }

        if (_products.Contains(product))
        {
            Context.Remove(product);
            
            await Context.SaveChangesAsync();
            await _grid.Reload();
        }
        else
        {
            _grid.CancelEditRow(product);
            await _grid.Reload();
        }
    }

    private async Task InsertRow()
    {
        _productToInsert = new();
        await _grid.InsertRow(_productToInsert);
    }

    private async Task OnCreateRow(Product product)
    {
        await Context.AddAsync(product);
        await Context.SaveChangesAsync();
        
        _productToInsert = null;
    }
}