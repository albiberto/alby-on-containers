using MudBlazor;
using ProductDataManager.Components.Pages.Descriptions.Model;
using ProductDataManager.Components.Shared.Model;

namespace ProductDataManager.Components.Shared.Filters;

#nullable enable

public partial class StateFilter<T> where T: IStatus
{
    bool open;
}