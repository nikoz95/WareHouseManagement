﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Infrastructure.Data.Configurations;

/// <summary>
/// WarehouseStock - ძირითადი ცხრილი (ყველა პროდუქტისთვის)
/// </summary>
public class WarehouseStockConfiguration : IEntityTypeConfiguration<WarehouseStock>
{
    public void Configure(EntityTypeBuilder<WarehouseStock> builder)
    {
        builder.HasKey(ws => ws.Id);
        
        builder.ToTable("WarehouseStocks");

        builder.Property(ws => ws.Quantity)
            .HasPrecision(18, 3);

        builder.Property(ws => ws.PurchasePrice)
            .HasPrecision(18, 2);

        builder.HasOne(ws => ws.WarehouseLocation)
            .WithMany(wl => wl.WarehouseStocks)
            .HasForeignKey(ws => ws.WarehouseLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ws => ws.Product)
            .WithMany(p => p.WarehouseStocks)
            .HasForeignKey(ws => ws.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Optional relationship with Manufacturer
        builder.HasOne(ws => ws.Manufacturer)
            .WithMany(m => m.WarehouseStocks)
            .HasForeignKey(ws => ws.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false); // ← Optional

        // Optional One-to-One relationship with PackagingDetails
        builder.HasOne(ws => ws.PackagingDetails)
            .WithOne(pd => pd.WarehouseStock)
            .HasForeignKey<PackagingDetails>(pd => pd.WarehouseStockId)
            .OnDelete(DeleteBehavior.Cascade);

        // Optional One-to-One relationship with AlcoholicStockDetails
        builder.HasOne(ws => ws.AlcoholicDetails)
            .WithOne(ad => ad.WarehouseStock)
            .HasForeignKey<AlcoholicStockDetails>(ad => ad.WarehouseStockId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(ws => !ws.IsDeleted);
    }
}

/// <summary>
/// PackagingDetails - უნივერსალური შეფუთვის მართვის ცხრილი (ნებისმიერი პროდუქტისთვის)
/// </summary>
public class PackagingDetailsConfiguration : IEntityTypeConfiguration<PackagingDetails>
{
    public void Configure(EntityTypeBuilder<PackagingDetails> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.ToTable("PackagingDetails");

        // Unique constraint on WarehouseStockId (one-to-one)
        builder.HasIndex(p => p.WarehouseStockId)
            .IsUnique();

        builder.Property(p => p.PackagingType)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.UnitsPerPackage)
            .IsRequired();

        builder.Property(p => p.FullPackagesCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(p => p.PartialPackagesCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(p => p.UnitsInPartialPackages)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(p => p.Notes)
            .HasMaxLength(500);

        // Computed properties are not mapped to database
        builder.Ignore(p => p.TotalPackagesCount);
        builder.Ignore(p => p.TotalUnitsCount);

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}

/// <summary>
/// AlcoholicStockDetails - ცალკე ცხრილი მხოლოდ ალკოჰოლური პროდუქტებისთვის
/// </summary>
public class AlcoholicStockDetailsConfiguration : IEntityTypeConfiguration<AlcoholicStockDetails>
{
    public void Configure(EntityTypeBuilder<AlcoholicStockDetails> builder)
    {
        builder.HasKey(a => a.Id);
        
        builder.ToTable("AlcoholicStockDetails");

        // Unique constraint on WarehouseStockId (one-to-one)
        builder.HasIndex(a => a.WarehouseStockId)
            .IsUnique();

        builder.Property(a => a.BatchNumber)
            .HasMaxLength(50);

        builder.Property(a => a.ExciseStampNumber)
            .HasMaxLength(100);

        builder.Property(a => a.CertificateNumber)
            .HasMaxLength(100);

        builder.Property(a => a.StorageTemperature)
            .HasPrecision(5, 2);


        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}
