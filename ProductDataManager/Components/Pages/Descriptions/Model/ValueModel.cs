using System.Text.Json.Serialization;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;
using ProductDataManager.Enums;
using ProductDataManager.Validators;

namespace ProductDataManager.Components.Pages.Descriptions.Model;

[method: JsonConstructor]
public class ValueModel(string value, string description, Guid id, Status status = Status.Unchanged) : IModelBase
{
    string originalValue = value;
    string originalDescription = description;
    Status originalStatus = status;

    public ValueModel(DescriptionValue value, Status state = Status.Unchanged) : this(value.Value, value.Description, value.Id!.Value, state)
    {
    }
    
    public Guid Id { get; } = id;
    public string Value { get; set; } = value;
    public string Description { get; set; } = description;
    public Status Status { get; set; } = status;
    
    public bool IsDirty => !string.Equals(originalValue, Value, StringComparison.InvariantCulture) || !string.Equals(originalDescription, Description, StringComparison.InvariantCulture);
    
    public void Clear()
    {
        Status = originalStatus;
        
        Value = originalValue;
        Description = originalDescription;
    }

    public void Reload()
    {
        Status = Status.Unchanged;
        originalStatus = Status.Unchanged;
        
        originalValue = Value;
        originalDescription = Description;
    }

    bool IsValidValue => StringValidators.Value().Validate(Value).IsValid;
    bool IsValidDescription => StringValidators.Description().Validate(Value).IsValid;
    
    public bool IsValid => IsValidValue && IsValidDescription;
}