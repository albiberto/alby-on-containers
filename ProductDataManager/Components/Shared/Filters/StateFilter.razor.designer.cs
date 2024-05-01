using MudBlazor;
using ProductDataManager.Components.Pages.Descriptions.Model;

namespace ProductDataManager.Components.Shared.Filters;

#nullable enable

public partial class StateFilter<T> where T : IModelBase
{
    bool all = true;
    bool open;
}