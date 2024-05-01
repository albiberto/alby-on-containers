using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;
using ProductDataManager.Components.Pages.Descriptions.Model;
using ProductDataManager.Components.Shared;
using ProductDataManager.Components.Shared.Dialogs;
using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;
using ProductDataManager.Enums;

namespace ProductDataManager.Components.Pages.Descriptions;

#nullable enable

public partial class Values : IDisposable
{
    [Inject] public required IDescriptionRepository Repository { get; set; }
    [Inject] public required IDialogService DialogService { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    [Inject] public required ILogger<Types> Logger { get; set; }
    [Inject] public required NavigationManager Navigation { get; set; }
    
    [Parameter] public required Guid TypeId { get; set; }
    [Parameter] public required IEnumerable<ValueModel> ValuesModel { get; set; }
    [Parameter] public required EventCallback<HashSet<ValueModel>> ValuesModelChanged { get; set; }

    public void Dispose()
    {
        Snackbar.Dispose();
    }
}