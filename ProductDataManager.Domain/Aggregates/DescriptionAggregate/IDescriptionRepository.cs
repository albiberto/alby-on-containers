using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Domain.Aggregates.DescriptionAggregate;

public interface IDescriptionRepository : IRepository<DescriptionType>
{
    Task<List<DescriptionType>> GetAllAsync();
    Task<DescriptionType> AddAsync(string name, string description);
}