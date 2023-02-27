namespace AlbyOnContainers.ProductDataManager.Pages.ProductsComponents;

using System.Linq;
using Extensions;
using Microsoft.AspNetCore.Components;
using Models;
using Microsoft.EntityFrameworkCore;
using Radzen;

public partial class Attrs
{
    [Parameter]public Product Product { get; set; }

    protected override void OnParametersSet() => Elements = Product.Attrs;
}