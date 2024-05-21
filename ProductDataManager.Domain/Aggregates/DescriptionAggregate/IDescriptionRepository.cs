using Microsoft.EntityFrameworkCore;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.DescriptionAggregate;

public interface IDescriptionRepository : IRepository<DescriptionType>;