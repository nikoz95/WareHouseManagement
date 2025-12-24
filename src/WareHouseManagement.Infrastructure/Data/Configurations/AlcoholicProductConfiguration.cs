using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Infrastructure.Data.Configurations;

public class AlcoholicProductConfiguration : IEntityTypeConfiguration<AlcoholicProduct>
{
    public void Configure(EntityTypeBuilder<AlcoholicProduct> builder)
    {
        builder.HasKey(ap => ap.Id);

        // One-to-One relationship with Product
        builder.HasOne(ap => ap.Product)
            .WithOne(p => p.AlcoholicProduct)
            .HasForeignKey<AlcoholicProduct>(ap => ap.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(ap => ap.AlcoholPercentage)
            .HasPrecision(5, 2);

        builder.Property(ap => ap.CountryOfOrigin)
            .HasMaxLength(100);

        builder.Property(ap => ap.AlcoholType)
            .HasMaxLength(50);

        builder.HasQueryFilter(ap => !ap.IsDeleted);
    }
}

