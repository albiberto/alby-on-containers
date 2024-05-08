using System.Text.Json.Serialization;
using ProductDataManager.Validators;

namespace ProductDataManager.Components.Pages.Descriptions.Model;

[method: JsonConstructor]
public record TypeModel(Guid Id, string Name, string Description, Status? Status = default)
{
    string originalName = Name;
    string originalDescription = Description;
    
    public string Name { get; set; } = Name;
    public string Description { get; set; } = Description;
    
    public Status Status { get; } = Status ?? new();

    public bool IsDirty => !string.Equals(originalName, Name, StringComparison.InvariantCulture) ||
                           !string.Equals(originalDescription, Description, StringComparison.InvariantCulture);

    public void Save()
    {
        Status.Save();

        originalName = Name;
        originalDescription = Description;
    }
    
    public void Clear()
    {
        Status.Clear();
        
        Name = originalName;
        Description = originalDescription;
    }

    bool IsValidName => StringValidators.Name().Validate(Name).IsValid;
    bool IsValidDescription => StringValidators.Description().Validate(Name).IsValid;
    public bool IsValid => IsValidName && IsValidDescription;
}