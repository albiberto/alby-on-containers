using AlbyOnContainers.ProductDataManager.Data;
using AlbyOnContainers.ProductDataManager.Models;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Radzen.Blazor;

namespace AlbyOnContainers.ProductDataManager.Pages;

public partial class ProductAttrs
{
    [Parameter] public Product Product { get; set; }
    [Inject] private ProductContext Context { get; set; }
    [Inject] private DialogService DialogService { get; set; }

    private IEnumerable<AttrType> _types;
    private IEnumerable<string> _selectedType;

    protected override void OnInitialized()
    {
        _types = Context.AttrTypes.ToList();
    }

    private RadzenDataGrid<Attr> _grid;
    private IList<Attr> _attrs;

    private void OnSelectedCompanyNamesChange(object value)
    {
        if (_selectedType != null && !_selectedType.Any())
        {
            _selectedType = null;  
        }
    }
    
    private void Reset()
    {
        _attrToInsert = null;
        _attrToUpdate = null;
    }

    private async Task EditRow(Attr attr)
    {
        _attrToUpdate = attr;
        await _grid.EditRow(attr);
    }

    private async Task OnUpdateRow(Attr attr)
    {
        if (attr == _attrToInsert)
        {
            _attrToInsert = null;
        }

        _attrToUpdate = null;

        Context.Update(attr);
        await Context.SaveChangesAsync();
    }

    private async Task SaveRow(Attr attr)
    {
        await _grid.UpdateRow(attr);
    }

    private void CancelEdit(Attr attr)
    {
        if (attr == _attrToInsert)
        {
            _attrToInsert = null;
        }

        _attrToUpdate = null;

        _grid.CancelEditRow(attr);

        var orderEntry = Context.Entry(attr);
        if (orderEntry.State != EntityState.Modified) return;
        orderEntry.CurrentValues.SetValues(orderEntry.OriginalValues);
        orderEntry.State = EntityState.Unchanged;
    }

    private async Task DeleteRow(Attr attr)
    {
        if (await DialogService.Confirm("Are you sure you want to delete this record?") == false) return;

        if (attr == _attrToInsert)
        {
            _attrToInsert = null;
        }

        if (attr == _attrToUpdate)
        {
            _attrToUpdate = null;
        }

        if (Product.Attrs.Contains(attr))
        {
            Context.Remove(attr);

            await Context.SaveChangesAsync();

            await _grid.Reload();
        }
        else
        {
            _grid.CancelEditRow(attr);
            await _grid.Reload();
        }
    }

    Attr _attrToInsert;
    Attr _attrToUpdate;

    private async Task InsertRow()
    {
        _attrToInsert = new()
        {
            // ProductId = Product.Id
        };
        await _grid.InsertRow(_attrToInsert);
    }

    private async Task OnCreateRow(Attr attr)
    {
        await Context.AddAsync(attr);
        await Context.SaveChangesAsync();
        
        _attrToInsert = null;
    }
}