namespace AlbyOnContainers.ProductDataManager.Pages.DescriptionsComponent;

using Radzen.Blazor;
using Models;
using Microsoft.EntityFrameworkCore;
using Data;
using Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Linq;
using Extensions;

public partial class DescrTypes
{
    [Inject] ProductContext Context { get; set; }
    [Inject] DialogService DialogService { get; set; }

    IEnumerable<DescrType> elemens;
    int count;
    bool isLoading;
    
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