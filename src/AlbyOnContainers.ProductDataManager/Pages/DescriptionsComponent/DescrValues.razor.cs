namespace AlbyOnContainers.ProductDataManager.Pages.DescriptionsComponent;

using Microsoft.AspNetCore.Components;
using Models;

public partial class DescrValues
{
    [Parameter] public DescrType DescrType { get; set; }

    protected override void OnParametersSet() => Elements = DescrType.DescrValues;
}