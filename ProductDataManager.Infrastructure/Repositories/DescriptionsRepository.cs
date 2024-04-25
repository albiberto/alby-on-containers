using Microsoft.EntityFrameworkCore;
using ProductDataManager.Domain.Aggregates.DescriptionAggregate;
using ProductDataManager.Domain.SeedWork;

namespace ProductDataManager.Infrastructure.Repositories;

public class DescriptionsRepository(ProductContext context) : IDescriptionRepository
{
    public IUnitOfWork UnitOfWork { get; } = context;
    
    public Task<List<DescriptionType>> GetAllAsync() => context.DescriptionTypes
        .Include(type => type.Categories)
        .Include(type => type.DescriptionValues)
        .OrderByDescending(type => type.Name).ToListAsync();

    public async Task<DescriptionType> AddAsync(string name, string description)
    {
        var descriptionType = new DescriptionType(name, description);
        var entity = await context.DescriptionTypes.AddAsync(descriptionType);

        return entity.Entity;
    }
}