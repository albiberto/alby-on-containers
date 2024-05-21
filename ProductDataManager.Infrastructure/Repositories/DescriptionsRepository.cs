using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Infrastructure.Repositories;

public class DescriptionsRepository(DbContext context) : Repository<DescriptionType>(context), IDescriptionRepository;