using System.Collections.ObjectModel;
using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;

namespace ProductDataManager.Components.Pages.Products.Model;

public class AggregatesModel(IEnumerable<DescriptionType>? types = default, IReadOnlyCollection<Category>? categories = default)
{
    public ObservableCollection<AggregateModel> Aggregates { get; } = new((types ?? []).Select(type => new AggregateModel(type, categories ?? [])));

    public void Add(Guid id) => Aggregates.Add(AggregateModel.New(id));

    public void Modified(AggregateModel aggregate)
    {
        if (aggregate.Type.IsDirty) aggregate.Type.Status.Modified();
        else aggregate.Type.Status.Unchanged();
    }
    
    public void Delete(AggregateModel aggregate)
    {
        if(aggregate.Type.Status.IsAdded) Aggregates.Remove(aggregate);
        else aggregate.Type.Status.Deleted();
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