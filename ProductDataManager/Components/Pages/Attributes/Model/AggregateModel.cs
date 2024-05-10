using System.Collections.ObjectModel;
using ProductDataManager.Domain.Aggregates.AttributeAggregate;

namespace ProductDataManager.Components.Pages.Attributes.Model;

public class AggregateModel(
    Guid id,
    string name,
    string description,
    bool mandatory,
    IEnumerable<TypeModel>? types = default,
    Status? status = default)
{
    public AggregateModel(AttributeCluster cluster) : this(
        cluster.Id!.Value,
        cluster.Name,
        cluster.Description,
        cluster.Mandatory,
        cluster.Types.Select(value => new TypeModel(value)))
    {
    }

    public static AggregateModel New(Guid id) => new(id, string.Empty, string.Empty, false, status: new(Value.Added));

    public ClusterModel Cluster { get; } = new(id, name, description, mandatory, status);
    public ObservableCollection<TypeModel> Types { get; set; } = new(types ?? []);

    public bool IsValid => Cluster.IsValid && Types.All(value => value.IsValid);

    public Status Status
    {
        get
        {
            if (!Cluster.Status.IsUnchanged) return Cluster.Status;

            return Types.All(value => value.Status.IsUnchanged)
                ? Cluster.Status
                : new(Value.Modified);
        }
    }

    public void AddType(Guid id, string value = "", string description = "") =>
        Types.Add(new(id, value, description, new(Value.Added)));

    public void RemoveType(TypeModel type)
    {
        if (type.Status.IsAdded) Types.Remove(type);
        else type.Status.Deleted();
    }

    public void Save()
    {
        Cluster.Save();

        foreach (var value in Types.ToHashSet())
        {
            if (value.Status.IsDeleted) Types.Remove(value);
            else value.Save();
        }
    }

    public void Clear()
    {
        Cluster.Clear();

        foreach (var value in Types.ToList())
        {
            if (value.Status.IsAdded) Types.Remove(value);
            else value.Clear();
        }
    }
}