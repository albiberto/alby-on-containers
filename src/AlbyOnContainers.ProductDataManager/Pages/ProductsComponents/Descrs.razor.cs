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
    IEnumerable<CategoryDescrType> Types { get; set; }

    protected override void OnParametersSet()
    {
        Elements = Product.Descrs.AsQueryable();
        
        Types = Context.CategoryDescrTypes
            .AsNoTracking()
            .Include(join => join.DescrType)
            .ThenInclude(type => type.DescrValues)
            .Where(join => join.CategoryId == Product.CategoryId)
            .OrderBy(join => join.DescrType.Name);
    }

    async Task UpsertAsync(Guid valueId)
    {
        var descr = Product.Descrs.FirstOrDefault(descr => descr.DescrValueId == valueId);

        if (descr is null)
        {
            Product.Descrs.Add(new(Product.Id!.Value, valueId));
        }
        else
        {
            descr.DescrValueId = valueId;
            Context.Update(Product);
        }

        await Context.SaveChangesAsync();
    }
    
    async Task OnSubmit(Product product)
    {
        Context.Update(product);
        await Context.SaveChangesAsync();
    }
}