namespace ProductDataManager.Components.Shared.Filters;

using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Model;
using MudBlazor;
using Pages.Attributes;

public partial class StateFilter<T> where T : class
{
    [Parameter] public required FilterContext<Model<T>> Context { get; set; }
    
    List<FilterModel> models =
    [
        new FilterModel(EntityState.Unchanged, true),
        new FilterModel(EntityState.Modified, true),
        new FilterModel(EntityState.Added, true),
        new FilterModel(EntityState.Deleted, true)
    ];

    FilterDefinition<Model<T>> FilterDefinition => new()
    {
        FilterFunction = type => models.Any(model => model.Checked && model.Value == type.State)
    };

    void OpenFilter() => open = true;
    void CloseFilter() => open = false;

    void SelectAll(bool value = false)
    {
        foreach (var model in models)
        {
            model.Set(value);
        }
    }

    async Task ClearFilterAsync()
    {
        SelectAll(true);
        await Context.Actions.ClearFilterAsync(FilterDefinition);
        open = false;
    }

    static void SelectedChanged(bool value, FilterModel model) => model.Set(value);

    async Task ApplyFilterAsync()
    {
        await Context.Actions.ApplyFilterAsync(FilterDefinition);
        open = false;
    }
}
