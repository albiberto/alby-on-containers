using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.ProductAggregate;

public record Product(string Name, string Description, Guid CategoryId, Guid? Id = default) : Entity(Id), IAggregateRoot
{
    public string Name { get; private set; } = Name;
    public string Description { get; private set; } = Description;
    public Guid CategoryId { get; private set; } = CategoryId;

    public Category? Category { get; private set; }
    public ICollection<ProductAttribute> ProductsAttributes { get; private set; } = [];
    public ICollection<ProductDescription> ProductsDescriptions { get; private set; } = [];

    public void Update(string name, string description, Guid? id)
    {
        Name = name;
        Description = description;
        
        if(id.HasValue) CategoryId = id.Value;
    }
}