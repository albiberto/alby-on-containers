using System.Text.Json.Serialization;
using ProductDataManager.Components.Shared.Model;
using ProductDataManager.Validators;

namespace ProductDataManager.Components.Pages.Products.Model;

[method: JsonConstructor]
public record ProductModel(Guid Id, string Name, string Description, Guid? CategoryId, Status? Status = default)
{
    string originalName = Name;
    string originalDescription = Description;
    Guid? originalCategory = CategoryId;
    
    public string Name { get; set; } = Name;
    public string Description { get; set; } = Description;
    public Guid? CategoryId { get; set; } = CategoryId;
    
    public Status Status { get; } = Status ?? new();

    public bool IsDirty => !string.Equals(originalName, Name, StringComparison.InvariantCulture) ||
                           !string.Equals(originalDescription, Description, StringComparison.InvariantCulture) ||
                           originalCategory != CategoryId;

    public void Save()
    {
        Status.Save();

        originalName = Name;
        originalDescription = Description;
        originalCategory = CategoryId;
    }
    
    public void Clear()
    {
        Status.Clear();
        
        Name = originalName;
        Description = originalDescription;
        CategoryId = originalCategory;
    }

    bool IsValidName => StringValidators.Name().Validate(Name).IsValid;
    bool IsValidDescription => StringValidators.Description().Validate(Name).IsValid;
    bool IsValidCategory => CategoryId.HasValue;
    public bool IsValid => IsValidName && IsValidDescription && IsValidCategory;
}