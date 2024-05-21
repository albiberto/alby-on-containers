using Microsoft.EntityFrameworkCore;
using ProductDataManager.Domain.Aggregates.CategoryAggregate;

namespace ProductDataManager.Infrastructure.Repositories;

public class CategoryRepository(DbContext context) : Repository<Category>(context), ICategoryRepository;