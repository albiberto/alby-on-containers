using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;

namespace ProductDataManager.Components.Pages.Descriptions;

#nullable enable

public partial class Descriptions
{
    [Inject] public required IDescriptionRepository Repository { get; set; }
    [Inject] public required IDialogService DialogService { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    [Inject] public required ILogger<Categories> Logger { get; set; }
    
    public class Data(DescriptionType type)
    {
        public Guid? Id { get; } = type.Id;
        public DescriptionType Type { get; } = type;

        public string Name { get; set; } = type.Name ?? string.Empty;
        public string Description { get; set; } = type.Description ?? string.Empty;
        
        public HashSet<Category> Categories { get; } = type.Categories.ToHashSet();
        public HashSet<DescriptionValue> Values { get; } = type.DescriptionValues.ToHashSet();
    }
}