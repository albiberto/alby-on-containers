using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Components.Pages.Descriptions.Model;
using ProductDataManager.Components.Shared.Filters.Model;
using ProductDataManager.Components.Shared.Model;

namespace ProductDataManager.Components.Shared.Filters;

public partial class StateFilter<T> where T: IStatus
{
    [Parameter] public required FilterContext<T> Context { get; set; }
    
    HashSet<FilterModel> models = Enum.GetValues(typeof(Value)).Cast<Value>().Select(value => new FilterModel(value, true)).ToHashSet();
    
    FilterDefinition<T> FilterDefinition => new()
    {
        FilterFunction = type => models.Where(model => model.Checked).Select(model => model.Value).Contains(type.Status.Current),
    };

    void OpenFilter() => open = true;
    void CloseFilter() => open = false;
    
    void SelectAll(bool value = false)
    {
        foreach (var model in models) model.Set(value);
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