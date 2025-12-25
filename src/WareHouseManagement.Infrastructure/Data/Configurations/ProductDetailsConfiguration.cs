using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Infrastructure.Data.Configurations;

/// <summary>
/// ProductDetails - ძირითადი ცხრილი (ყველა პროდუქტისთვის)
/// </summary>
public class ProductDetailsConfiguration : IEntityTypeConfiguration<ProductDetails>
{
    public void Configure(EntityTypeBuilder<ProductDetails> builder)
    {
        builder.HasKey(pd => pd.Id);

        builder.ToTable("ProductDetails");

        // One-to-One relationship with Product
        builder.HasOne(pd => pd.Product)
            .WithOne(p => p.ProductDetails)
            .HasForeignKey<ProductDetails>(pd => pd.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique constraint on ProductId
        builder.HasIndex(pd => pd.ProductId)
            .IsUnique();

        builder.Property(pd => pd.CountryOfOrigin)
            .HasMaxLength(100);

        builder.Property(pd => pd.ProductType)
            .HasMaxLength(50);

        builder.Property(pd => pd.AdditionalNotes)
            .HasMaxLength(1000);

        // Optional One-to-One relationship with AlcoholicProductDetails
        builder.HasOne(pd => pd.AlcoholicDetails)
            .WithOne(apd => apd.ProductDetails)
            .HasForeignKey<AlcoholicProductDetails>(apd => apd.ProductDetailsId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(pd => !pd.IsDeleted);
    }
}

/// <summary>
/// AlcoholicProductDetails - ცალკე ცხრილი მხოლოდ ალკოჰოლური პროდუქტებისთვის
/// </summary>
public class AlcoholicProductDetailsConfiguration : IEntityTypeConfiguration<AlcoholicProductDetails>
{
    public void Configure(EntityTypeBuilder<AlcoholicProductDetails> builder)
    {
        builder.HasKey(apd => apd.Id);
        
        builder.ToTable("AlcoholicProductDetails");

        // Unique constraint on ProductDetailsId (one-to-one)
        builder.HasIndex(apd => apd.ProductDetailsId)
            .IsUnique();

        builder.Property(apd => apd.AlcoholPercentage)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(apd => apd.Region)
            .HasMaxLength(100);

        builder.Property(apd => apd.ServingTemperature)
            .HasPrecision(5, 2);

        builder.Property(apd => apd.QualityClass)
            .HasMaxLength(50);

        builder.HasQueryFilter(apd => !apd.IsDeleted);
    }
}

