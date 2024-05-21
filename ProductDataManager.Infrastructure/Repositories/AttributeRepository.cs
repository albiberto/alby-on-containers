using Microsoft.EntityFrameworkCore;
using ProductDataManager.Domain.Aggregates.AttributeAggregate;
using ProductDataManager.Domain.SeedWork;
using Attribute = ProductDataManager.Domain.Aggregates.AttributeAggregate.Attribute;

namespace ProductDataManager.Infrastructure.Repositories;

public class AttributeRepository(DbContext context) : Repository<AttributeType>(context), IAttributeRepository;