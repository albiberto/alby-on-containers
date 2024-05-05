using System.Text.Json.Serialization;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;
using ProductDataManager.Enums;
using ProductDataManager.Validators;

namespace ProductDataManager.Components.Pages.Descriptions.Model;

[method: JsonConstructor]
public class TypeModel(string name, string description, Guid id, IEnumerable<ValueModel>? values = default, IEnumerable<CategoryModel>? join = default, Status status = Status.Unchanged) : IModelBase
{
    string originalName = name;
    string originalDescription = description;
    Status originalStatus = status;

    public TypeModel(DescriptionType type, Status state) : this(type.Name, type.Description, type.Id!.Value, 
        type.DescriptionValues.Select(value => new ValueModel(value)), 
        type.DescriptionTypesCategories.Select(join => new CategoryModel(join.Id!.Value, join.CategoryId, join.Category!.Name)),
        state)
    {
    }
    
    public Guid Id { get; } = id;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;

    
    public HashSet<ValueModel> Values { get; set; } = (values ?? []).ToHashSet();
    public HashSet<CategoryModel> Categories { get; set; } = (join ?? []).ToHashSet();

    public bool IsDirty => 
        !string.Equals(originalName, Name, StringComparison.InvariantCulture) || 
        !string.Equals(originalDescription, Description, StringComparison.InvariantCulture) ||
        Values.Any(value => value.IsDirty);
    
    public void Clear()
    {
        Status = originalStatus;
        
        Name = originalName;
        Description = originalDescription;
    }

    public void Reload()
    {
        Status = Status.Unchanged;
        originalStatus = Status.Unchanged;
        
        originalName = Name;
        originalDescription = Description;

        foreach (var value in Values) value.Reload();
    }

    bool IsValidName => StringValidators.Name().Validate(Name).IsValid;
    bool IsValidDescription => StringValidators.Description().Validate(Name).IsValid;
    
    public bool IsValid => IsValidName && IsValidDescription && Values.All(value => value.IsValid);
    public Status Status { get; set; } = status;

    public Status CumulativeStatus
    {
        get
        {
            return Values.Any(value => value.Status != Status.Unchanged) ? Status.Modified : Status;   
        }
    }
}