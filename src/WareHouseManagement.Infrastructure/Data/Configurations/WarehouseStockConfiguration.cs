using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Infrastructure.Data.Configurations;

public class WarehouseStockConfiguration : IEntityTypeConfiguration<WarehouseStock>
{
    public void Configure(EntityTypeBuilder<WarehouseStock> builder)
    {
        builder.HasKey(ws => ws.Id);

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

        builder.HasOne(ws => ws.Manufacturer)
            .WithMany(m => m.WarehouseStocks)
            .HasForeignKey(ws => ws.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(ws => !ws.IsDeleted);
    }
}

