namespace AlbyOnContainers.ProductDataManager.Pages.ProductsComponents;

using AlbyOnContainers.ProductDataManager.Data;
using AlbyOnContainers.ProductDataManager.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Radzen.Blazor;

public partial class Descrs
{
    [Inject] ProductContext Context { get; set; }
    [Inject] DialogService DialogService { get; set; }

    RadzenDataGrid<Descr> grid;

    Descr toInsert;
    Descr toUpdate;   
    
    async Task InsertRowAsync()
    {
        toInsert = new();
        await grid.InsertRow(toInsert);
    }
    
    async Task EditRowAsync(Descr type)
    {
        toUpdate = type;
        await grid.EditRow(type);
    }

    void CancelEdit(Descr type)
    {
        if (type.Equals(toInsert)) toInsert = null;

        toUpdate = null;

        grid.CancelEditRow(type);

        var orderEntry = Context.Entry(type);
        if (orderEntry.State != EntityState.Modified) return;
        
        orderEntry.CurrentValues.SetValues(orderEntry.OriginalValues);
        orderEntry.State = EntityState.Unchanged;
    }
    
    Task SaveRowAsync(Descr type) => grid.UpdateRow(type);
    
    void Reset()
    {
        toInsert = null;
        toUpdate = null;
    }
}