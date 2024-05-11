using System.Text.Json.Serialization;
using ProductDataManager.Components.Shared.Model;
using ProductDataManager.Domain.Aggregates.AttributeAggregate;
using ProductDataManager.Validators;
using Attribute = ProductDataManager.Domain.Aggregates.AttributeAggregate.Attribute;

namespace ProductDataManager.Components.Pages.Attributes.Model;

[method: JsonConstructor]
public record AttributeModel(Guid Id, string Name, string Description, Status? Status = default)
    {
        string originalName = Name;
        string originalDescription = Description;

    public AttributeModel(Attribute attribute, Status? state = default) : this(attribute.Id!.Value, attribute.Description, attribute.Description, state)
    {
    }
    
    public string Name { get; set; } = Name;
    public string Description { get; set; } = Description;
    public Status Status { get; private set; } = Status ?? new();
    
    public bool IsDirty => !string.Equals(originalName, Name, StringComparison.InvariantCulture) || 
                           !string.Equals(originalDescription, Description, StringComparison.InvariantCulture);
    
    public void Clear()
    {
        Status.Clear();
        
        Name = originalName;
        Description = originalDescription;
    }

    public void Save()
    {
        Status.Save();

        originalName = Name;
        originalDescription = Description;
    }

    bool IsValidValue => StringValidators.Value().Validate(Name).IsValid;
    bool IsValidDescription => StringValidators.Description().Validate(Name).IsValid;
    public bool IsValid => IsValidValue && IsValidDescription;   
}