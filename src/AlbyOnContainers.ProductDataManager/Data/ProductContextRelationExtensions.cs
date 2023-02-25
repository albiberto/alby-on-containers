namespace AlbyOnContainers.ProductDataManager.Data;

using Microsoft.EntityFrameworkCore;
using Models;

public static partial class ProductContextExtensions
{
    public static ModelBuilder BuildProductRelations(this ModelBuilder modelBuilder)
    {
        var product = modelBuilder.Entity<Product>();

        product
            .HasOne(product => product.Category)
            .WithMany(category => category.Products)
            .HasForeignKey(p => p.CategoryId)
            .HasConstraintName("FK_product_categories");

        product
            .HasMany(product => product.Descrs)
            .WithOne(descr => descr.Product)
            .HasForeignKey(descr => descr.ProductId)
            .HasConstraintName("FK_descr_products");

        product
            .HasMany(product => product.Attrs)
            .WithOne(attr => attr.Product)
            .HasForeignKey(attr => attr.ProductId)
            .HasConstraintName("FK_attr_products");

        return modelBuilder;
    }

    public static ModelBuilder BuildCategoryRelations(this ModelBuilder modelBuilder)
    {
        var category = modelBuilder.Entity<Category>();

        category
            .HasOne(category => category.Parent)
            .WithMany(category => category.Categories)
            .HasForeignKey(category => category.ParentId)
            .IsRequired(false)
            .HasConstraintName("FK_category_categories");

        return modelBuilder;
    }

    public static ModelBuilder BuildAttributeRelations(this ModelBuilder modelBuilder)
    {
        var type = modelBuilder.Entity<AttrType>();
        var join = modelBuilder.Entity<CategoryAttrType>();
        var attr = modelBuilder.Entity<Attr>();

        join.HasKey(join => new { join.CategoryId, join.AttrTypeId });

        modelBuilder.Entity<CategoryAttrType>()
            .HasOne(sc => sc.Category)
            .WithMany(s => s.CategoryAttrTypes)
            .HasForeignKey(sc => sc.CategoryId)
            .HasConstraintName("FK_Category_CategoryAttrTypes");

        modelBuilder.Entity<CategoryAttrType>()
            .HasOne(sc => sc.AttrType)
            .WithMany(s => s.CategoryAttrTypes)
            .HasForeignKey(sc => sc.AttrTypeId)
            .HasConstraintName("FK_AttrType_CategoryAttrTypes");

        attr.HasOne(i => i.AttrType)
            .WithMany(i => i.Attrs)
            .HasForeignKey(i => i.AttrTypeId)
            .HasConstraintName("FK_AttrType_Attrs");

        return modelBuilder;
    }

    public static ModelBuilder BuildDescriptionRelations(this ModelBuilder modelBuilder)
    {
        var type = modelBuilder.Entity<DescrType>();
        var join = modelBuilder.Entity<CategoryDescrType>();
        var descr = modelBuilder.Entity<Descr>();
        var value = modelBuilder.Entity<DescrValue>();

        join.HasKey(join => new { join.CategoryId, join.DescrTypeId });

        join
            .HasOne(sc => sc.Category)
            .WithMany(s => s.CategoryDescrTypes)
            .HasForeignKey(sc => sc.CategoryId)
            .HasConstraintName("FK_Category_CategoryDescrTypes");

        join
            .HasOne(sc => sc.DescrType)
            .WithMany(s => s.CategoryDescrTypes)
            .HasForeignKey(sc => sc.DescrTypeId)
            .HasConstraintName("FK_DescrType_CategoryDescrTypes");

        value
            .HasOne(value => value.DescrType)
            .WithMany(type => type.DescrValues)
            .HasForeignKey(value => value.DescrTypeId)
            .HasConstraintName("FK_DescrType_DescrValues");

        descr
            .HasOne(descr => descr.DescrValue)
            .WithMany(value => value.Descrs)
            .HasForeignKey(descr => descr.DescrValueId)
            .HasConstraintName("FK_DescrValue_Descrs");

        return modelBuilder;
    }
}