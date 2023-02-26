namespace AlbyOnContainers.ProductDataManager.Pages.ProductsComponents;

using AlbyOnContainers.ProductDataManager.Data;
using AlbyOnContainers.ProductDataManager.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Radzen.Blazor;

public partial class Attrs
{
    [Inject] ProductContext Context { get; set; }
    [Inject] DialogService DialogService { get; set; }

    RadzenDataGrid<Attr> grid;

    Attr toInsert;
    Attr toUpdate;   
    
    async Task InsertRowAsync()
    {
        toInsert = new();
        await grid.InsertRow(toInsert);
    }
    
    async Task EditRowAsync(Attr type)
    {
        toUpdate = type;
        await grid.EditRow(type);
    }

    void CancelEdit(Attr type)
    {
        if (type.Equals(toInsert)) toInsert = null;

        toUpdate = null;

        grid.CancelEditRow(type);

        var orderEntry = Context.Entry(type);
        if (orderEntry.State != EntityState.Modified) return;
        
        orderEntry.CurrentValues.SetValues(orderEntry.OriginalValues);
        orderEntry.State = EntityState.Unchanged;
    }
    
    Task SaveRowAsync(Attr type) => grid.UpdateRow(type);
    
    void Reset()
    {
        toInsert = null;
        toUpdate = null;
    }
}