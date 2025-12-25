﻿﻿using Microsoft.EntityFrameworkCore;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Infrastructure.Data.Seed;

namespace WareHouseManagement.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanyLocation> CompanyLocations { get; set; }
    public DbSet<CompanyProduct> CompanyProducts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<WarehouseLocation> WarehouseLocations { get; set; }
    public DbSet<WarehouseStock> WarehouseStocks { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Debtor> Debtors { get; set; }
    public DbSet<UnitTypeRule> UnitTypeRules { get; set; }
    public DbSet<ProductDetails> ProductDetails { get; set; }
    public DbSet<AlcoholicProductDetails> AlcoholicProductDetails { get; set; }
    public DbSet<PackagingDetails> PackagingDetails { get; set; }
    public DbSet<AlcoholicStockDetails> AlcoholicStockDetails { get; set; }
    public DbSet<WarehouseStockHistory> WarehouseStockHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        // Seed data
        modelBuilder.SeedUnitTypeRules();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.Entity is Domain.Common.BaseEntity baseEntity)
            {
                if (entry.State == EntityState.Added)
                {
                    baseEntity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    baseEntity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}

