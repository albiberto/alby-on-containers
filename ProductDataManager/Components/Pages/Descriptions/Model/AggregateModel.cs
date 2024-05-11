using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using ProductDataManager.Components.Shared.Model;
using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;

namespace ProductDataManager.Components.Pages.Descriptions.Model;

[method: JsonConstructor]
public class AggregateModel(
    Guid id,
    string name,
    string description,
    bool mandatory,
    IEnumerable<ValueModel>? values = default,
    IEnumerable<JoinModel>? joins = default,
    Status? status = default) : IStatus
{
    public AggregateModel(DescriptionType type, IEnumerable<Category> categories) : this(
        type.Id!.Value,
        type.Name,
        type.Description,
        type.Mandatory,
        type.DescriptionValues.Select(value => new ValueModel(value)),
        categories.Select(category =>
        {
            var join = type.DescriptionTypesCategories.FirstOrDefault(join => join.CategoryId == category.Id);
            return new JoinModel(join?.Id, category.Id!.Value, category.Name, join is not null);
        }))
    {
    }

    public static AggregateModel New(Guid id) => new(id, string.Empty, string.Empty, false, status: new(Value.Added));

    public TypeModel Type { get; } = new(id, name, description, mandatory, status);
    public ObservableCollection<ValueModel> Values { get; set; } = new(values ?? []);
    public ObservableCollection<JoinModel> Joins { get; set; } = new(joins ?? []);

    public bool IsValid => Type.IsValid && Values.All(value => value.IsValid);

    public Status Status
    {
        get
        {
            if(!Type.Status.IsUnchanged) return Type.Status;
            
            return Values.All(value => value.Status.IsUnchanged) 
                ? Type.Status 
                : new(Value.Modified);
        }
    }

    public void AddValue(Guid id, string value = "", string description = "") => Values.Add(new(id, value, description, new(Value.Added)));
    public void RemoveValue(ValueModel value)
    {
        if (value.Status.IsAdded) Values.Remove(value);
        else value.Status.Deleted();
    }

    public void Save()
    {
        Type.Save();
        
        foreach (var value in Values.ToHashSet())
        {
            if (value.Status.IsDeleted) Values.Remove(value);
            else value.Save();
        }   
    }

    public void Clear()
    {
        Type.Clear();
        
        foreach (var value in Values.ToList())
        {
            if (value.Status.IsAdded) Values.Remove(value);
            else value.Clear();
        }
    }
}
