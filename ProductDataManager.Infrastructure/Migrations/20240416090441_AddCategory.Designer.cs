﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProductDataManager.Infrastructure;

#nullable disable

namespace ProductDataManager.Infrastructure.Migrations
{
    [DbContext(typeof(ProductContext))]
    [Migration("20240416090441_AddCategory")]
    partial class AddCategory
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ProductDataManager.Infrastructure.Domain.AttrType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("LastModified")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AttrTypes");
                });

            modelBuilder.Entity("ProductDataManager.Infrastructure.Domain.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("LastModified")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ProductDataManager.Infrastructure.Domain.CategoryAttrType", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TypeId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("LastModified")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("CategoryId", "TypeId");

                    b.HasIndex("TypeId");

                    b.ToTable("CategoryAttrTypes");
                });

            modelBuilder.Entity("ProductDataManager.Infrastructure.Domain.Category", b =>
                {
                    b.HasOne("ProductDataManager.Infrastructure.Domain.Category", "Parent")
                        .WithMany("Categories")
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("ProductDataManager.Infrastructure.Domain.CategoryAttrType", b =>
                {
                    b.HasOne("ProductDataManager.Infrastructure.Domain.Category", "Category")
                        .WithMany("CategoryAttrTypes")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProductDataManager.Infrastructure.Domain.AttrType", "Type")
                        .WithMany("CategoryAttrTypes")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("ProductDataManager.Infrastructure.Domain.AttrType", b =>
                {
                    b.Navigation("CategoryAttrTypes");
                });

            modelBuilder.Entity("ProductDataManager.Infrastructure.Domain.Category", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("CategoryAttrTypes");
                });
#pragma warning restore 612, 618
        }
    }
}
