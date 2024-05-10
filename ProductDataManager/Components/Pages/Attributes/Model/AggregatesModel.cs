using System.Collections.ObjectModel;
using ProductDataManager.Domain.Aggregates.AttributeAggregate;

namespace ProductDataManager.Components.Pages.Attributes.Model;

public class AggregatesModel(IEnumerable<AttributeCluster>? clusters = default)
{
    public ObservableCollection<AggregateModel> Aggregates { get; } = new((clusters ?? []).Select(cluster => new AggregateModel(cluster)));

    public void Add(Guid id) => Aggregates.Add(AggregateModel.New(id));

    public void Modified(AggregateModel aggregate)
    {
        if (aggregate.Cluster.IsDirty) aggregate.Cluster.Status.Modified();
        else aggregate.Cluster.Status.Unchanged();
    }
    
    public void Delete(AggregateModel aggregate)
    {
        if(aggregate.Cluster.Status.IsAdded) Aggregates.Remove(aggregate);
        else aggregate.Cluster.Status.Deleted();
    }

    public void Save()
    {
        foreach (var aggregate in Aggregates.ToHashSet())
            if (aggregate.Status.IsDeleted) Aggregates.Remove(aggregate);
            else aggregate.Save();
    }
    
    public bool IsValid => Aggregates.All(aggregate => aggregate.IsValid);
    
    public void Clear()
    {
        foreach (var aggregate in Aggregates.ToList())
            if(aggregate.Status.IsAdded) Aggregates.Remove(aggregate);
            else aggregate.Clear();
    }
}