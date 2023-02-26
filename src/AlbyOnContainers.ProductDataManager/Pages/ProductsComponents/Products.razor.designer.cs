namespace AlbyOnContainers.ProductDataManager.Pages.ProductsComponents;

using AlbyOnContainers.ProductDataManager.Data;
using AlbyOnContainers.ProductDataManager.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Radzen.Blazor;

public partial class Products
{
    [Inject] ProductContext Context { get; set; }
    [Inject] DialogService DialogService { get; set; }

    IEnumerable<Product> elements;
    int count;
    bool isLoading;
    
    RadzenDataGrid<Product> grid;

    Product toInsert;
    Product toUpdate;   
    
    async Task InsertRowAsync()
    {
        toInsert = new();
        await grid.InsertRow(toInsert);
    }
    
    async Task EditRowAsync(Product type)
    {
        toUpdate = type;
        await grid.EditRow(type);
    }

    void CancelEdit(Product type)
    {
        if (type.Equals(toInsert)) toInsert = null;

        toUpdate = null;

        grid.CancelEditRow(type);

        var orderEntry = Context.Entry(type);
        if (orderEntry.State != EntityState.Modified) return;
        
        orderEntry.CurrentValues.SetValues(orderEntry.OriginalValues);
        orderEntry.State = EntityState.Unchanged;
    }
    
    Task SaveRowAsync(Product type) => grid.UpdateRow(type);
    
    void Reset()
    {
        toInsert = null;
        toUpdate = null;
    }
}