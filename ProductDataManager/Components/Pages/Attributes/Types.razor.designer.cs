﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Components.Pages.Attributes.Model;
using ProductDataManager.Domain.Aggregates.AttributeAggregate;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;

namespace ProductDataManager.Components.Pages.Attributes;

public partial class Types : IDisposable
{
    [Inject] public required IAttributeRepository AttributeRepository { get; set; }
    [Inject] public required IDialogService DialogService { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    [Inject] public required ILogger<Types> Logger { get; set; }
    [Inject] public required NavigationManager Navigation { get; set; }

    [Parameter] public required AggregateModel Aggregate { get; set; }
    [Parameter] public required EventCallback<AggregateModel> AggregateChanged { get; set; }

    public void Dispose()
    {
        Snackbar.Dispose();
    }
}