namespace AlbyOnContainers.ProductDataManager.Pages.ProductsComponents;

using System.Linq;
using Extensions;
using Microsoft.AspNetCore.Components;
using Models;
using Microsoft.EntityFrameworkCore;
using Radzen;

public partial class Descrs
{
    [Parameter]public Product Product { get; set; }
    ICollection<CategoryDescrType> Types { get; set; } = new List<CategoryDescrType>();

    protected override async Task OnParametersSetAsync()
    {
        Elements = Product.Descrs;
        
        Types = await Context.CategoryDescrTypes
            .Include(join => join.DescrType)
            .ThenInclude(type => type.DescrValues)
            .Where(join => join.CategoryId == Product.CategoryId)
            .OrderBy(join => join.DescrType.Name)
            .AsNoTracking()
            .ToListAsync();
    }

    async Task ValueChange(Guid valueId, Guid typeId)
    {
        if (valueId == Guid.Empty)
        {
            Product.Descrs = Product.Descrs.Where(descr => descr.DescrTypeId != typeId).ToList();
            return;
        }
        
        var descr = Product
            .Descrs
            .FirstOrDefault(descr => descr.DescrTypeId == typeId);
        
        if (descr is null)
        {
            var @new = new Descr(Product.Id!.Value, valueId, typeId);
            Product.Descrs.Add(@new);
        }
        else if (descr.DescrValueId == valueId)
        {
            // no changes
        }
        else
        {
            descr.DescrValueId = valueId;
            Context.Update(Product);
        }
    }
    
    Task OnSubmit(Product product) => Context.SaveChangesAsync();

    void Cancel()
    {
        //
    }
}