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
    IEnumerable<CategoryDescrType> types;

    protected override void OnParametersSet()
    {
        Elements = Product.Descrs;
        
        types = Context.CategoryDescrTypes
            .Include(join => join.DescrType)
            .ThenInclude(type => type.DescrValues)
            .Where(join => join.CategoryId == Product.CategoryId)
            .OrderBy(join => join.DescrType.Name);
    }
}