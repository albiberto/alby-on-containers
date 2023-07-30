namespace ProductDataManager.Pages;

using Infrastructure.Domain;

public partial class AttrTypes
{
    protected void OnCreateRow(AttrType model)
    {
        ToInsert = default;

        Context.AddRange(model);
        Context.SaveChanges();
    }

    protected void OnUpdateRow(AttrType model)
    {
        // if (model.Equals(ToInsert)) ToInsert = default;
        // ToUpdate = default;
        //
        // // var existing = Context.CategoryAttrTypes.Where(join => join.TypeId == model.Id).AsNoTracking().ToArray();
        //
        // // var toAdd = existing.Intersect(elements, new CategoryAttrTypeComparer.Update());
        // // var toDelete = existing.Except(elements, new CategoryAttrTypeComparer.Delete());
        //
        // Context.CategoryAttrTypes.UpdateRange(elements);
        // // Context.AddRange(toAdd);
        // // Context.RemoveRange(toDelete);
        // Context.SaveChanges();
    }
    
    protected virtual Task<bool> OnDeleteRowAsync(AttrType element) => Task.FromResult(true);

    protected async Task DeleteRowAsync(AttrType element)
    {
        if (await DialogService.Confirm("Are you sure you want to delete this record?") == false) return;
        if (!await OnDeleteRowAsync(element)) return;

        if (element.Equals(ToInsert)) ToInsert = null;
        if (element.Equals(ToUpdate)) ToUpdate = null;

        if (Types.Contains(element))
        {
            Context.Remove(element);
            await Context.SaveChangesAsync();

            await Grid.Reload();
        }
        else
        {
            Grid.CancelEditRow(element);
            await Grid.Reload();
        }
    }


}