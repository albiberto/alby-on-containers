using Ardalis.Specification;
using ProductDataManager.Domain.Aggregates.CategoryAggregate;

namespace ProductDataManager.Infrastructure.Specifications;

public sealed class OrderedCategorySpecification : Specification<Category>
{
    public OrderedCategorySpecification() => Query.OrderBy(category => category.Name);
}
