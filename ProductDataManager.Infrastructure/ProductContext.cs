namespace ProductDataManager.Infrastructure;

using Domain;
using Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class ProductContext: DbContext
{

    public ProductContext(DbContextOptions<ProductContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new List<IInterceptor>
        {
            new AuditableInterceptor()
        });
    }
}