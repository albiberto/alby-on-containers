namespace AlbyOnContainers.ProductDataManager.Pages.DescriptionsComponent;

using Radzen.Blazor;
using Models;
using Microsoft.EntityFrameworkCore;

public partial class DescrTypes
{
    RadzenDataGrid<DescrType> grid;

    DescrType toInsert;
    DescrType toUpdate;   
    
    async Task InsertRowAsync()
    {
        toInsert = new();
        await grid.InsertRow(toInsert);
    }
    
    async Task EditRowAsync(DescrType type)
    {
        toUpdate = type;
        await grid.EditRow(type);
    }

    void CancelEdit(DescrType type)
    {
        if (type.Equals(toInsert)) toInsert = null;

        toUpdate = null;

        grid.CancelEditRow(type);

        var orderEntry = Context.Entry(type);
        if (orderEntry.State != EntityState.Modified) return;
        
        orderEntry.CurrentValues.SetValues(orderEntry.OriginalValues);
        orderEntry.State = EntityState.Unchanged;
    }
    
    Task SaveRowAsync(DescrType type) => grid.UpdateRow(type);
    
    void Reset()
    {
        toInsert = null;
        toUpdate = null;
    }
}