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

    public AttributeTypeModel Type { get; } = new(id, name, description, status);
    public ObservableCollection<AttributeModel> Attributes { get; set; } = new(attrs ?? []);

    public bool IsValid => Type.IsValid && Attributes.All(value => value.IsValid);

    public Status Status
    {
        get
        {
            if (!Type.Status.IsUnchanged) return Type.Status;

            return Attributes.All(value => value.Status.IsUnchanged)
                ? Type.Status
                : new(Value.Modified);
        }
    }

    public void AddAttribute(Guid id, string value = "", string description = "") =>
        Attributes.Add(new(id, value, description, Type.Id, new(Value.Added)));

    public void RemoveAttribute(AttributeModel attribute)
    {
        if (attribute.Status.IsAdded) Attributes.Remove(attribute);
        else attribute.Status.Deleted();
    }

    public void Save()
    {
        Type.Save();

        foreach (var value in Attributes.ToHashSet())
        {
            if (value.Status.IsDeleted) Attributes.Remove(value);
            else value.Save();
        }
    }

    public void Clear()
    {
        Type.Clear();

        foreach (var value in Attributes.ToList())
        {
            if (value.Status.IsAdded) Attributes.Remove(value);
            else value.Clear();
        }
    }
    
    public IEnumerable<AttributeModel> GetDirtyAttributes => Attributes.Where(value => value.IsDirtyTypeId);
}