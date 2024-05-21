using Microsoft.EntityFrameworkCore;
using ProductDataManager.Domain.Aggregates.ProductAggregate;

namespace ProductDataManager.Infrastructure.Repositories;

public class ProductRepository(DbContext context) : Repository<Product>(context), IProductRepository;