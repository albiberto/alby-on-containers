namespace AlbyOnContainers.ProductDataManager.Pages.DescriptionsComponent;

using Microsoft.EntityFrameworkCore;
using Models;

public partial class DescrTypes
{
    protected override IQueryable<DescrType> OnLoadData() =>
            Context.DescrTypes
                .Include(type => type.CategoryDescrTypes)
                .ThenInclude(join => join.Category)
                .Include(type => type.DescrValues)
                .AsQueryable();

    protected override async Task<bool> OnDeleteRowAsync(DescrType element)
    {
        if (!await Context.DescrValues.AnyAsync(value => value.DescrTypeId == element.Id)) return true;
        
        await DialogService.Alert("This record is currently binding to value/values and it cannot be deleted.", "Cannot Delete");
        return false;

    }
}