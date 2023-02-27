namespace AlbyOnContainers.ProductDataManager.Pages.ProductsComponents;

using System.Linq;
using Extensions;
using Microsoft.AspNetCore.Components;
using Models;
using Microsoft.EntityFrameworkCore;
using Radzen;

public partial class Attrs
{
    [Parameter]public Product Product { get; set; }

    ICollection<AttrType> Types { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        Elements = Product.Attrs;
        
        Types = await Context.CategoryAttrTypes
            .Include(join => join.AttrType)
            .Where(join => join.CategoryId == Product.CategoryId)
            .OrderBy(join => join.AttrType.Name)
            .Select(join => join.AttrType)
            .AsNoTracking()
            .ToListAsync();
    }
}