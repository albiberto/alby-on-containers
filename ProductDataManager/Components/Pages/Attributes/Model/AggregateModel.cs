using System.Collections.ObjectModel;
using ProductDataManager.Components.Shared.Model;
using ProductDataManager.Domain.Aggregates.AttributeAggregate;

namespace ProductDataManager.Components.Pages.Attributes.Model;

public class AggregateModel(
    Guid id,
    string name,
    string description,
    IEnumerable<AttributeModel>? attrs = default,
    Status? status = default) : IStatus
{
    public AggregateModel(AttributeType type) : this
    (
        type.Id!.Value,
        type.Name,
        type.Description,
        type.Attributes.Select(value => new AttributeModel(value)))
    {
    }

    public static AggregateModel New(Guid id) => new(id, string.Empty, string.Empty, status: new(Value.Added));

    public AttributeTypeModel AttributeType { get; } = new(id, name, description, status);
    public ObservableCollection<AttributeModel> Types { get; set; } = new(attrs ?? []);

    public bool IsValid => AttributeType.IsValid && Types.All(value => value.IsValid);

    public Status Status
    {
        get
        {
            if (!AttributeType.Status.IsUnchanged) return AttributeType.Status;

            return Types.All(value => value.Status.IsUnchanged)
                ? AttributeType.Status
                : new(Value.Modified);
        }
    }

    public void AddType(Guid id, string value = "", string description = "") =>
        Types.Add(new(id, value, description, new(Value.Added)));

    public void RemoveType(AttributeModel attribute)
    {
        if (attribute.Status.IsAdded) Types.Remove(attribute);
        else attribute.Status.Deleted();
    }

    public void Save()
    {
        AttributeType.Save();

        foreach (var value in Types.ToHashSet())
        {
            if (value.Status.IsDeleted) Types.Remove(value);
            else value.Save();
        }
    }

    public void Clear()
    {
        AttributeType.Clear();

        foreach (var value in Types.ToList())
        {
            if (value.Status.IsAdded) Types.Remove(value);
            else value.Clear();
        }
    }
}