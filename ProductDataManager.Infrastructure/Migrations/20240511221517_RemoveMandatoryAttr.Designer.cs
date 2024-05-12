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
    [Migration("20240511221517_RemoveMandatoryAttr")]
    partial class RemoveMandatoryAttr
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-preview.3.24172.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ProductDataManager.Domain.Aggregates.AttributeAggregate.Attribute", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ClusterId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ClusterId");

                    b.ToTable("Attribute");
                });

            modelBuilder.Entity("ProductDataManager.Domain.Aggregates.AttributeAggregate.AttributeType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Tech")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AttributeType");
                });

            modelBuilder.Entity("ProductDataManager.Domain.Aggregates.CategoryAggregate.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("ParentId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ProductDataManager.Domain.Aggregates.DescriptionAggregate.DescriptionType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("Mandatory")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("DescriptionTypes");
                });

            modelBuilder.Entity("ProductDataManager.Domain.Aggregates.DescriptionAggregate.DescriptionTypeCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("DescriptionTypeId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("DescriptionTypeId");

                    b.ToTable("DescriptionTypesCategories");
                });

            modelBuilder.Entity("ProductDataManager.Domain.Aggregates.DescriptionAggregate.DescriptionValue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("DescriptionTypeId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DescriptionTypeId");

                    b.ToTable("DescriptionValues");
                });

            modelBuilder.Entity("ProductDataManager.Domain.Aggregates.AttributeAggregate.Attribute", b =>
                {
                    b.HasOne("ProductDataManager.Domain.Aggregates.AttributeAggregate.AttributeType", "Cluster")
                        .WithMany("Types")
                        .HasForeignKey("ClusterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cluster");
                });

            modelBuilder.Entity("ProductDataManager.Domain.Aggregates.CategoryAggregate.Category", b =>
                {
                    b.HasOne("ProductDataManager.Domain.Aggregates.CategoryAggregate.Category", "Parent")
                        .WithMany("Categories")
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("ProductDataManager.Domain.Aggregates.DescriptionAggregate.DescriptionTypeCategory", b =>
                {
                    b.HasOne("ProductDataManager.Domain.Aggregates.CategoryAggregate.Category", "Category")
                        .WithMany("DescriptionTypesCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProductDataManager.Domain.Aggregates.DescriptionAggregate.DescriptionType", "DescriptionType")
                        .WithMany("DescriptionTypesCategories")
                        .HasForeignKey("DescriptionTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("DescriptionType");
                });

            modelBuilder.Entity("ProductDataManager.Domain.Aggregates.DescriptionAggregate.DescriptionValue", b =>
                {
                    b.HasOne("ProductDataManager.Domain.Aggregates.DescriptionAggregate.DescriptionType", "DescriptionType")
                        .WithMany("DescriptionValues")
                        .HasForeignKey("DescriptionTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DescriptionType");
                });

            modelBuilder.Entity("ProductDataManager.Domain.Aggregates.AttributeAggregate.AttributeType", b =>
                {
                    b.Navigation("Types");
                });

            modelBuilder.Entity("ProductDataManager.Domain.Aggregates.CategoryAggregate.Category", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("DescriptionTypesCategories");
                });

            modelBuilder.Entity("ProductDataManager.Domain.Aggregates.DescriptionAggregate.DescriptionType", b =>
                {
                    b.Navigation("DescriptionTypesCategories");

                    b.Navigation("DescriptionValues");
                });
#pragma warning restore 612, 618
        }
    }
}