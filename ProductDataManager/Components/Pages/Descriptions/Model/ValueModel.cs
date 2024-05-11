using System.Text.Json.Serialization;
using ProductDataManager.Components.Shared.Model;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;
using ProductDataManager.Validators;

namespace ProductDataManager.Components.Pages.Descriptions.Model;

[method: JsonConstructor]
public record ValueModel(Guid Id, string Value, string Description, Status? Status = default)
{
    string originalValue = Value;
    string originalDescription = Description;

    public ValueModel(DescriptionValue value, Status? state = default) : this(value.Id!.Value, value.Value, value.Description, state)
    {
    }
    
    public string Value { get; set; } = Value;
    public string Description { get; set; } = Description;
    public Status Status { get; private set; } = Status ?? new();
    
    public bool IsDirty => !string.Equals(originalValue, Value, StringComparison.InvariantCulture) || 
                           !string.Equals(originalDescription, Description, StringComparison.InvariantCulture);
    
    public void Clear()
    {
        Status.Clear();
        
        Value = originalValue;
        Description = originalDescription;
    }

    public void Save()
    {
        Status.Save();

        originalValue = Value;
        originalDescription = Description;
    }

    bool IsValidValue => StringValidators.Value().Validate(Value).IsValid;
    bool IsValidDescription => StringValidators.Description().Validate(Value).IsValid;
    public bool IsValid => IsValidValue && IsValidDescription;
}