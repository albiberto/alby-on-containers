namespace AlbyOnContainers.ProductDataManager.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Models;

public class ProductContext : DbContext
{
    public ProductContext(DbContextOptions<ProductContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryAttrType> CategoryAttrTypes { get; set; }
    public DbSet<CategoryDescrType> CategoryDescrTypes { get; set; }

    public DbSet<AttrType> AttrTypes { get; set; }
    public DbSet<Attr> Attrs { get; set; }

    public DbSet<DescrType> DescrTypes { get; set; }
    public DbSet<DescrValue> DescrValues { get; set; }
    public DbSet<Descr> Descrs { get; set; }

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
            .BuildCategoryAttrType()
            .BuildCategoryDescrType();
    }
}