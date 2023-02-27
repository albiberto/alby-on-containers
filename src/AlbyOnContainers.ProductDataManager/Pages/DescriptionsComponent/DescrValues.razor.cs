namespace AlbyOnContainers.ProductDataManager.Pages.DescriptionsComponent;

using Data;
using Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Models;
using Radzen;
using Radzen.Blazor;

public partial class DescrValues
{
    [Parameter] public DescrType DescrType { get; set; }

    protected override void OnParametersSet() => Elements = DescrType.DescrValues;
}