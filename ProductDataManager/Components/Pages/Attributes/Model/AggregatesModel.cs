using System.Collections.ObjectModel;
using ProductDataManager.Domain.Aggregates.AttributeAggregate;

namespace ProductDataManager.Components.Pages.Attributes.Model;

public class AggregatesModel(IEnumerable<AttributeType>? types = default)
{
    public ObservableCollection<AggregateModel> Aggregates { get; } = new((types ?? []).Select(type => new AggregateModel(type)));

    public void AddType(Guid id) => Aggregates.Add(AggregateModel.New(id));

    public void ModifyType(AggregateModel aggregate)
    {
        if (aggregate.Type.IsDirty) aggregate.Type.Status.Modified();
        else aggregate.Type.Status.Unchanged();
    }
    
    public void DeleteType(AggregateModel aggregate)
    {
        if(aggregate.Type.Status.IsAdded) Aggregates.Remove(aggregate);
        else aggregate.Type.Status.Deleted();
    }

    public HashSet<(Guid TypeId, string TypeName)> Types => 
        Aggregates
            .Select(aggregate => (aggregate.Type.Id, aggregate.Type.Name))
            .ToHashSet();
    
    public void Save()
    {
        foreach (var aggregate in Aggregates)
        {
            foreach (var attribute in aggregate.GetDirtyAttributes)
            {
                aggregate.RemoveAttribute(attribute);
                Aggregates.Single(@new => @new.Type.Id == attribute.TypeId).AddAttribute(attribute.Id, attribute.Name, attribute.Description);
            }
        
            if (aggregate.Status.IsDeleted) Aggregates.Remove(aggregate);
            else aggregate.Save();   
        }
    }
    
    public bool IsValid => Aggregates.All(aggregate => aggregate.IsValid);
    
    public void Clear()
    {
        foreach (var aggregate in Aggregates.ToHashSet())
            if(aggregate.Status.IsAdded) Aggregates.Remove(aggregate);
            else aggregate.Clear();
    }
}