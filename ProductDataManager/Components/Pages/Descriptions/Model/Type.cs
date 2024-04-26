using System.Text.Json.Serialization;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;
using ProductDataManager.Enums;
using ProductDataManager.Validators;

namespace ProductDataManager.Components.Pages.Descriptions.Model;

[method: JsonConstructor]
public class Type(string name, string description, Guid id, IEnumerable<Value>? values = default, Status status = Status.Unchanged)
{
    string originalName = name;
    string originalDescription = description;
    Status originalStatus = status;

    public Type(DescriptionType type, Status state) : this(type.Name, type.Description, type.Id!.Value, type.DescriptionValues.Select(value => new Value(value.Id!.Value, value.Name, value.Description)), state)
    {
    }
    
    public Guid Id { get; } = id;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public Status Status { get; set; } = status;

    public HashSet<Value> Values { get; } = (values ?? []).ToHashSet();

    public bool IsDirty => !string.Equals(originalName, Name, StringComparison.InvariantCulture) || !string.Equals(originalDescription, Description, StringComparison.InvariantCulture);
    
    public void Clear()
    {
        Status = originalStatus;
        
        Name = originalName;
        Description = originalDescription;
    }

    public void Reload()
    {
        Status = Status.Unchanged;
        originalStatus = Status;
        
        originalName = Name;
        originalDescription = Description;
    }

    bool IsValidName => StringValidators.Name().Validate(Name).IsValid;
    bool IsValidDescription => StringValidators.Description().Validate(Name).IsValid;
    
    public bool IsValid => IsValidName && IsValidDescription;
}