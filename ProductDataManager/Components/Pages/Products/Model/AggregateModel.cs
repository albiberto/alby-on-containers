using System.Text.Json.Serialization;
using ProductDataManager.Components.Shared.Model;
using ProductDataManager.Domain.Aggregates.CategoryAggregate;
using ProductDataManager.Domain.Aggregates.ProductAggregate;

namespace ProductDataManager.Components.Pages.Products.Model;

[method: JsonConstructor]
public class AggregateModel(
    Guid id,
    string name,
    string description,
    Guid? categoryId,
    Status? status = default) : IStatus
{

    public AggregateModel(Product product) : this(
        product.Id!.Value,
        product.Name,
        product.Description,
        product.CategoryId)
    {
    }
    
    public ProductModel Product { get; } = new(id, name, description, categoryId, status);

    public bool IsValid => Product.IsValid;

    public Status Status => Product.Status;

    public void Save() => Product.Save();

    public void Clear() => Product.Clear();
}
