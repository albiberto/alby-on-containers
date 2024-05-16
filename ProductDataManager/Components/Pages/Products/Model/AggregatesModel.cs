using System.Collections.ObjectModel;
using ProductDataManager.Components.Shared.Model;
using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.Aggregates.ProductAggregate;

namespace ProductDataManager.Components.Pages.Products.Model;

public class AggregatesModel(IEnumerable<Product>? products = default, IEnumerable<Category>? categories = default)
{
    public HashSet<CategoryModel> Categories { get; } = categories?.Select(category => new CategoryModel(category.Id!.Value, category.Name)).ToHashSet() ?? [];

    public ObservableCollection<AggregateModel> Aggregates { get; } = new((products ?? []).Select(product => new AggregateModel(product)));
    
    public void Add(Guid id) => Aggregates.Add(new(id, string.Empty, string.Empty, default, new(Value.Added)));

    public void Modified(AggregateModel aggregate)
    {
        if (aggregate.Product.IsDirty) aggregate.Product.Status.Modified();
        else aggregate.Product.Status.Unchanged();
    }
    
    public void Delete(AggregateModel aggregate)
    {
        if(aggregate.Product.Status.IsAdded) Aggregates.Remove(aggregate);
        else aggregate.Product.Status.Deleted();
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