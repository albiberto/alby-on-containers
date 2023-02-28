namespace AlbyOnContainers.ProductDataManager.Pages.Abstract;

using Extensions;
using Models.Abstract;
using Radzen;

public abstract class DataLoadGridBase<T> : GridBase<T> where T : Entity, new()
{
    protected int Count;
    protected bool IsLoading;

    protected abstract IQueryable<T> OnLoadData();

    protected async Task LoadDataAsync(LoadDataArgs args)
    {
        IsLoading = true;
        await Task.Yield();

        var (entities, count) = await OnLoadData().LoadDataAsync(args);

        Elements = entities;
        Count = count;
        
        IsLoading = false;
    }
}