using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Components.Pages.Descriptions.Model;
using ProductDataManager.Enums;

namespace ProductDataManager.Components.Shared.Filters;

public partial class StateFilter<T> : ComponentBase where T : IModelBase
{
    [Parameter] public required FilterContext<T> Context { get; set; }
    
    static HashSet<Status> _states = Enum.GetValues(typeof(Status)).Cast<Status>().ToHashSet();
    
    HashSet<Status> selectedItems = _states.ToHashSet();
    
    FilterDefinition<T> FilterDefinition => new()
    {
        FilterFunction = type => selectedItems.Contains(type.Status)
    };

    void OpenFilter() => open = true;
    void CloseFilter() => open = false;
    
    void SelectAll(bool value)
    {
        all = value;

        if (value)
        {
            selectedItems = _states.ToHashSet();
        }
        else
        {
            selectedItems.Clear();
        }
    }
    
    async Task ClearFilterAsync()
    {
        selectedItems = _states.ToHashSet();
        await Context.Actions.ClearFilterAsync(FilterDefinition);
        open = false;
        all = true;
    }

    async Task ApplyFilterAsync()
    {
        await Context.Actions.ApplyFilterAsync(FilterDefinition);
        open = false;
    }
    
    void SelectedChanged(bool value, Status item)
    {
        if (value)
            selectedItems.Add(item);
        else
            selectedItems.Remove(item);

        all = selectedItems.Count == _states.Count;
    }
}