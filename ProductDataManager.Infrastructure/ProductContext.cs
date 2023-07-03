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
    public DbSet<AttrType> AttrTypes { get; set; } = null!;
    public DbSet<CategoryAttrType> CategoryAttrTypes { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new List<IInterceptor>
        {
            new AuditableInterceptor()
        });
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<CategoryAttrType>()
            .HasKey(c => new { c.CategoryId, c.TypeId });
    }
}