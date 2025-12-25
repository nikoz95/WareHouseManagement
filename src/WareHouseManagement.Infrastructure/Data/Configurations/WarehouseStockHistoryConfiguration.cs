using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Infrastructure.Data.Configurations;

public class WarehouseStockHistoryConfiguration : IEntityTypeConfiguration<WarehouseStockHistory>
{
    public void Configure(EntityTypeBuilder<WarehouseStockHistory> builder)
    {
        builder.HasKey(wsh => wsh.Id);

        builder.Property(wsh => wsh.TransactionType)
            .IsRequired();

        builder.Property(wsh => wsh.QuantityChange)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(wsh => wsh.QuantityBefore)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(wsh => wsh.QuantityAfter)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(wsh => wsh.Reason)
            .HasMaxLength(500);

        builder.Property(wsh => wsh.PerformedBy)
            .HasMaxLength(100);

        builder.Property(wsh => wsh.TransactionDate)
            .IsRequired();

        // Relationships
        builder.HasOne(wsh => wsh.WarehouseStock)
            .WithMany(ws => ws.StockHistories)
            .HasForeignKey(wsh => wsh.WarehouseStockId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(wsh => wsh.Order)
            .WithMany()
            .HasForeignKey(wsh => wsh.OrderId)
            .OnDelete(DeleteBehavior.SetNull);

        // Index for faster queries
        builder.HasIndex(wsh => wsh.WarehouseStockId);
        builder.HasIndex(wsh => wsh.OrderId);
        builder.HasIndex(wsh => wsh.TransactionDate);
        builder.HasIndex(wsh => wsh.TransactionType);
    }
}

