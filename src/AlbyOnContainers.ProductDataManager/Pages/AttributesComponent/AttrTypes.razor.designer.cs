namespace AlbyOnContainers.ProductDataManager.Pages.AttributesComponent;

using Radzen.Blazor;
using Models;
using Microsoft.EntityFrameworkCore;

public partial class AttrTypes
{
    RadzenDataGrid<AttrType> grid;

    AttrType toInsert;
    AttrType toUpdate;   
    
    async Task InsertRowAsync()
    {
        toInsert = new();
        await grid.InsertRow(toInsert);
    }
    
    async Task EditRowAsync(AttrType type)
    {
        toUpdate = type;
        await grid.EditRow(type);
    }

    void CancelEdit(AttrType type)
    {
        if (type.Equals(toInsert)) toInsert = null;

        toUpdate = null;

        grid.CancelEditRow(type);

        var orderEntry = Context.Entry(type);
        if (orderEntry.State != EntityState.Modified) return;
        
        orderEntry.CurrentValues.SetValues(orderEntry.OriginalValues);
        orderEntry.State = EntityState.Unchanged;
    }
    
    Task SaveRowAsync(AttrType type) => grid.UpdateRow(type);
    
    void Reset()
    {
        toInsert = null;
        toUpdate = null;
    }
}