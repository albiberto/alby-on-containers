namespace AlbyOnContainers.ProductDataManager.Data;

using Microsoft.EntityFrameworkCore;
using Models;

public static partial class ProductContextExtensions
{
    public static ModelBuilder BuildProduct(this ModelBuilder modelBuilder)
    {
        var product = modelBuilder.Entity<Product>();

        product
            .HasKey(p => p.Id)
            .HasName("PK_Product");

        product
            .Property(p => p.Id)
            .HasColumnName("ProductId");

        return modelBuilder;
    }

    public static ModelBuilder BuildCategory(this ModelBuilder modelBuilder)
    {
        var category = modelBuilder.Entity<Category>();

        category
            .HasKey(p => p.Id)
            .HasName("PK_Category");

        category
            .Property(p => p.Id)
            .HasColumnName("CategoryId");

        return modelBuilder;
    }

    public static ModelBuilder BuildAttrType(this ModelBuilder modelBuilder)
    {
        var attrType = modelBuilder.Entity<AttrType>();

        attrType
            .HasKey(p => p.Id)
            .HasName("PK_AttrType");

        attrType
            .Property(p => p.Id)
            .HasColumnName("AttrTypeId");

        return modelBuilder;
    }

    public static ModelBuilder BuildAttr(this ModelBuilder modelBuilder)
    {
        var attr = modelBuilder.Entity<Attr>();

        attr
            .HasKey(p => p.Id)
            .HasName("PK_Attr");

        attr
            .Property(p => p.Id)
            .HasColumnName("AttrId");

        return modelBuilder;
    }

    public static ModelBuilder BuildDescrType(this ModelBuilder modelBuilder)
    {
        var descrType = modelBuilder.Entity<DescrType>();

        descrType
            .HasKey(p => p.Id)
            .HasName("PK_DescrType");

        descrType
            .Property(p => p.Id)
            .HasColumnName("DescrTypeId");

        return modelBuilder;
    }

    public static ModelBuilder BuildDescValue(this ModelBuilder modelBuilder)
    {
        var descrValue = modelBuilder.Entity<DescrValue>();

        descrValue
            .HasKey(p => p.Id)
            .HasName("PK_DescrValue");

        descrValue
            .Property(p => p.Id)
            .HasColumnName("DescrValueId");

        return modelBuilder;
    }

    public static ModelBuilder BuildDescr(this ModelBuilder modelBuilder)
    {
        var descr = modelBuilder.Entity<Descr>();

        descr
            .HasKey(p => p.Id)
            .HasName("PK_Descr");

        descr
            .Property(p => p.Id)
            .HasColumnName("DescrId");

        return modelBuilder;
    }

    public static ModelBuilder BuildCategoryDescrType(this ModelBuilder modelBuilder)
    {
        var categoryDescrType = modelBuilder.Entity<CategoryDescrType>();

        categoryDescrType
            .HasKey(join => new { join.CategoryId, join.DescrTypeId })
            .HasName("PK_CategoryDescrType");

        return modelBuilder;
    }

    public static ModelBuilder BuildCategoryAttrType(this ModelBuilder modelBuilder)
    {
        var categoryAttrType = modelBuilder.Entity<CategoryAttrType>();

        categoryAttrType
            .HasKey(join => new { join.CategoryId, join.AttrTypeId })
            .HasName("PK_CategoryAttrType");

        return modelBuilder;
    }
}