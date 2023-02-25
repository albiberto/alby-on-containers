namespace AlbyOnContainers.ProductDataManager.Pages.DescriptionsComponent;

using Radzen.Blazor;
using Models;
using Microsoft.EntityFrameworkCore;

public partial class DescrValues
{
    RadzenDataGrid<DescrValue> grid;

    DescrValue toInsert;
    DescrValue toUpdate;   
    
    async Task InsertRowAsync()
    {
        toInsert = new();
        await grid.InsertRow(toInsert);
    }
    
    async Task EditRowAsync(DescrValue value)
    {
        toUpdate = value;
        await grid.EditRow(value);
    }

    void CancelEdit(DescrValue value)
    {
        if (value.Equals(toInsert)) toInsert = null;

        toUpdate = null;

        grid.CancelEditRow(value);

        var orderEntry = Context.Entry(value);
        if (orderEntry.State != EntityState.Modified) return;
        
        orderEntry.CurrentValues.SetValues(orderEntry.OriginalValues);
        orderEntry.State = EntityState.Unchanged;
    }
    
    Task SaveRowAsync(DescrValue value)
    {
        value.DescrType = DescrType;
        return grid.UpdateRow(value);
    }

    void Reset()
    {
        toInsert = null;
        toUpdate = null;
    }
}