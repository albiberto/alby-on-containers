using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;

namespace ProductDataManager.Components.Pages.Descriptions;

#nullable enable

public partial class Values
{
    [Inject] public required IDescriptionRepository Repository { get; set; }
    [Inject] public required IDialogService DialogService { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    [Inject] public required ILogger<Values> Logger { get; set; }   
    
    public class Data
    {
        public Data(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
            
            OrignalName = name;
            OrignalDescription = description;
        }

        public Guid? Id { get; }
        public string OrignalName { get; }
        public string Name { get; set; }
        public string OrignalDescription { get; }
        public string Description { get; set; }
        
        public bool IsDirty => !string.Equals(OrignalName, Name, StringComparison.InvariantCulture) || !string.Equals(OrignalDescription, Description, StringComparison.InvariantCulture);
        
        public void Clear()
        {
            Name = OrignalName;
            Description = OrignalDescription;
        }
    }
}