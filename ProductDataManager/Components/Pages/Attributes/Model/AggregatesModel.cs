using System.Collections.ObjectModel;
using ProductDataManager.Domain.Aggregates.AttributeAggregate;

namespace ProductDataManager.Components.Pages.Attributes.Model;

public class AggregatesModel(IEnumerable<AttributeType>? types = default)
{
    public ObservableCollection<AggregateModel> Aggregates { get; } = new((types ?? []).Select(type => new AggregateModel(type)));

    public void Add(Guid id) => Aggregates.Add(AggregateModel.New(id));

    public void Modified(AggregateModel aggregate)
    {
        if (aggregate.AttributeType.IsDirty) aggregate.AttributeType.Status.Modified();
        else aggregate.AttributeType.Status.Unchanged();
    }
    
    public void Delete(AggregateModel aggregate)
    {
        if(aggregate.AttributeType.Status.IsAdded) Aggregates.Remove(aggregate);
        else aggregate.AttributeType.Status.Deleted();
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