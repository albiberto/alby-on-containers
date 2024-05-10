using ProductDataManager.Validators;

namespace ProductDataManager.Components.Pages.Attributes.Model;

public record ClusterModel(Guid Id, string Name, string Description, bool Mandatory, Status? Status = default)
{
    string originalName = Name;
    string originalDescription = Description;
    bool originalMandatory = Mandatory;
    
    public string Name { get; set; } = Name;
    public string Description { get; set; } = Description;
    
    public bool Mandatory { get; set; } = Mandatory;
    
    public Status Status { get; } = Status ?? new();

    public bool IsDirty => !string.Equals(originalName, Name, StringComparison.InvariantCulture) ||
                           !string.Equals(originalDescription, Description, StringComparison.InvariantCulture) ||
                           Mandatory != originalMandatory;

    public void Save()
    {
        Status.Save();

        originalName = Name;
        originalDescription = Description;
        originalMandatory = Mandatory;
    }
    
    public void Clear()
    {
        Status.Clear();
        
        Name = originalName;
        Description = originalDescription;
        Mandatory = originalMandatory;
    }

    bool IsValidName => StringValidators.Name().Validate(Name).IsValid;
    bool IsValidDescription => StringValidators.Description().Validate(Name).IsValid;
    public bool IsValid => IsValidName && IsValidDescription;
}