using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Infrastructure.Data.Configurations;

public class CompanyProductConfiguration : IEntityTypeConfiguration<CompanyProduct>
{
    public void Configure(EntityTypeBuilder<CompanyProduct> builder)
    {
        builder.ToTable("CompanyProducts");
        
        builder.HasKey(cp => cp.Id);
        
        builder.Property(cp => cp.CommercialPrice)
            .HasPrecision(18, 2)
            .IsRequired();
        
        // Relationships
        builder.HasOne(cp => cp.Company)
            .WithMany(c => c.CompanyProducts)
            .HasForeignKey(cp => cp.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(cp => cp.Product)
            .WithMany(p => p.CompanyProducts)
            .HasForeignKey(cp => cp.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(cp => cp.CompanyLocation)
            .WithMany()
            .HasForeignKey(cp => cp.CompanyLocationId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
        
        // Global query filter to match Company's filter
        builder.HasQueryFilter(cp => !cp.IsDeleted);
    }
}

