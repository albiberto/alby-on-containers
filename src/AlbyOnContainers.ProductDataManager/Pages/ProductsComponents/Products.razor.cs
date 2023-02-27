namespace AlbyOnContainers.ProductDataManager.Pages.ProductsComponents;

using System.Linq;
using Models;
using Microsoft.EntityFrameworkCore;

public partial class Products
{
    protected override IQueryable<Product> OnLoadData() =>
        Context.Products
            .Include(product => product.Descrs)
            .ThenInclude(join => join.DescrValue)
            .ThenInclude(value => value.DescrType)
            .ThenInclude(type => type.CategoryDescrTypes)
            .Include(product => product.Attrs)
            .AsQueryable();
}
