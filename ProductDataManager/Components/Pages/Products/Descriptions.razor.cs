using Microsoft.AspNetCore.Components;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;

namespace ProductDataManager.Components.Pages.Products;

public partial class Descriptions : ComponentBase
{
    List<DescriptionType> descriptions = [];
    
    protected override async Task OnInitializedAsync() => descriptions = await DescriptionsRepository.GetAllAsync();
}