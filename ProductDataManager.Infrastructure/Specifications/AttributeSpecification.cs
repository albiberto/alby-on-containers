using Ardalis.Specification;
using ProductDataManager.Domain.Aggregates.AttributeAggregate;

namespace ProductDataManager.Infrastructure.Specifications;

public sealed class AttributeSpecification : Specification<AttributeType>
{
    public AttributeSpecification() => Query.Include(attr => attr.Attributes);
}