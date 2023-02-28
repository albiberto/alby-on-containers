namespace AlbyOnContainers.ProductDataManager.Pages.Abstract;

using Models.Abstract;

public abstract class LoadChildDataGridBase<T> : GridBase<T> where T : Entity, new()
{
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender) await Grid.ExpandRow(Elements.FirstOrDefault());
    }

}