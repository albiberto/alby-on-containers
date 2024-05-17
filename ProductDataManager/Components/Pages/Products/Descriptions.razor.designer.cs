using Microsoft.AspNetCore.Components;
using ProductDataManager.Infrastructure.Repositories;

namespace ProductDataManager.Components.Pages.Products;

public partial class Descriptions
{
    [Inject] public required DescriptionsRepository DescriptionsRepository { get; set; }
}