namespace AlbyOnContainers.ProductDataManager.Shared;

using System;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

public partial class CulturePicker
{
    [Inject]
    protected IJSRuntime JSRuntime { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    protected DialogService DialogService { get; set; }

    [Inject]
    protected TooltipService TooltipService { get; set; }

    [Inject]
    protected ContextMenuService ContextMenuService { get; set; }

    [Inject]
    protected NotificationService NotificationService { get; set; }

    protected string culture;

    protected override void OnInitialized()
    {
        culture = CultureInfo.CurrentCulture.Name;
    }

    protected void ChangeCulture()
    {
        var redirect = new Uri(NavigationManager.Uri).GetComponents(UriComponents.PathAndQuery | UriComponents.Fragment, UriFormat.UriEscaped);

        var query = $"?culture={Uri.EscapeDataString(culture)}&redirectUri={redirect}";

        NavigationManager.NavigateTo("Culture/SetCulture" + query, forceLoad: true);
    }
}