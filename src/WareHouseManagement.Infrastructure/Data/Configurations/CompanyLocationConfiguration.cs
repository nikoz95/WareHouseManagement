using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Infrastructure.Data.Configurations;

public class CompanyLocationConfiguration : IEntityTypeConfiguration<CompanyLocation>
{
    public void Configure(EntityTypeBuilder<CompanyLocation> builder)
    {
        builder.ToTable("CompanyLocations");
        
        builder.HasKey(cl => cl.Id);
        
        builder.Property(cl => cl.LocationName)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(cl => cl.Address)
            .HasMaxLength(500);
        
        builder.Property(cl => cl.City)
            .HasMaxLength(100);
        
        builder.Property(cl => cl.Phone)
            .HasMaxLength(50);
        
        builder.Property(cl => cl.ContactPerson)
            .HasMaxLength(200);
        
        // Relationship with Company
        builder.HasOne(cl => cl.Company)
            .WithMany(c => c.CompanyLocations)
            .HasForeignKey(cl => cl.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Global query filter to match Company's filter
        builder.HasQueryFilter(cl => !cl.IsDeleted);
    }
}

