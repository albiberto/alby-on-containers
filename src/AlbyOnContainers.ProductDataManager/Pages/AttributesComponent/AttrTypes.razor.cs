namespace AlbyOnContainers.ProductDataManager.Pages.AttributesComponent;

using Microsoft.EntityFrameworkCore;
using Models;

public partial class AttrTypes
{
    protected override async Task<bool> OnDeleteRowAsync(AttrType element)
    {
        if (!await Context.DescrValues.AnyAsync(value => value.DescrTypeId == element.Id)) return true;
        
        await DialogService.Alert("This record is currently binding to value/values and it cannot be deleted.", "Cannot Delete");
        return false;
    }
    
    protected override IQueryable<AttrType> OnLoadData() =>
        Context.AttrTypes
            .Include(type => type.CategoryAttrTypes)
            .ThenInclude(join => join.Category)
            .AsQueryable();
}